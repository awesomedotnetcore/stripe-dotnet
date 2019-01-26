namespace Stripe.Infrastructure.FormEncoding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using Newtonsoft.Json;
    using Stripe.Infrastructure.Http;

    /// <summary>
    /// This class provides methods to serialize various objects with
    /// <c>application/x-www-form-urlencoded</c> encoding. This is used to encode request
    /// parameters to be sent to Stripe's API.
    /// </summary>
    internal static class FormEncoder
    {
        /// <summary>Creates the HTTP query string for a given options class.</summary>
        /// <param name="options">The options class for which to create the query string.</param>
        /// <returns>The query string.</returns>
        public static string EncodeOptions(BaseOptions options)
        {
            return EncodeValue(options, null);
        }

        public static HttpContent EncodeOptionsContent(BaseOptions options)
        {
            if (options is MultipartOptions multipartOptions)
            {
                return EncodeOptionsMultipart(multipartOptions);
            }

            return new FormUrlEncodedUTF8Content(options);
        }

        public static MultipartFormDataContent EncodeOptionsMultipart(MultipartOptions options)
        {
            var content = new MultipartFormDataContent();

            foreach (var property in options.GetType().GetRuntimeProperties())
            {
                // Skip properties not annotated with `[JsonProperty]`
                var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                var key = attribute.PropertyName;
                var value = property.GetValue(options);

                if (value is Stream stream)
                {
                    string fileName = "blob";
#if NET45 || NETSTANDARD2_0
                    FileStream fileStream = stream as FileStream;
                    if ((fileStream != null) && (!string.IsNullOrEmpty(fileStream.Name)))
                    {
                        fileName = fileStream.Name;
                    }
#endif
                    var streamContent = new StreamContent(stream);

                    content.Add(streamContent, key, fileName);
                }
                else
                {
                    var flatParams = FlattenParamsValue(value, key);

                    foreach (var flatParam in flatParams)
                    {
                        content.Add(new StringContent(flatParam.Value), flatParam.Key);
                    }
                }
            }

            return content;
        }

        /// <summary>Creates the HTTP query string for a given dictionary.</summary>
        /// <param name="dictionary">The dictionary for which to create the query string.</param>
        /// <returns>The query string.</returns>
        public static string EncodeDictionary(IDictionary dictionary)
        {
            return EncodeValue(dictionary, null);
        }

        /// <summary>Creates the HTTP query string for a given list.</summary>
        /// <param name="list">The list for which to create the query string.</param>
        /// <param name="key">The key to use for the list's elements.</param>
        /// <param name="encodeEmptyList">
        /// Whether to encode empty lists or not. If this parameter is <c>true</c> and the list
        /// is empty, then the list is encoded as <c>key=</c>; otherwise, return <c>null</c>
        /// </param>
        /// <returns>The query string.</returns>
        public static string EncodeList(IEnumerable list, string key, bool encodeEmptyList = false)
        {
            if (!encodeEmptyList && !list.Cast<object>().Any())
            {
                return null;
            }

            return EncodeValue(list, key);
        }

        /// <summary>
        /// Join several query strings together, omitting <c>null</c> or empty strings.
        /// </summary>
        /// <param name="queries">One or more query strings to be joined.</param>
        /// <returns>The joined query string.</returns>
        public static string JoinQueries(params string[] queries)
        {
            return string.Join("&", queries.Where(q => !string.IsNullOrEmpty(q)));
        }

        /// <summary>Creates the HTTP query string for a given value.</summary>
        /// <param name="value">The value to encode.</param>
        /// <param name="key">
        /// The key under which the value should be encoded. Optional if the value is a
        /// <see cref="INestedOptions" /> or <see cref="IDictionary" />, required otherwise.
        /// </param>
        /// <returns>A string representing the value with form encoding.</returns>
        private static string EncodeValue(object value, string key)
        {
            if (string.IsNullOrEmpty(key) && !(value is INestedOptions) && !(value is IDictionary))
            {
                throw new ArgumentException(
                    "key cannot be null or empty when value is neither an INestedOptions or an IDictionary",
                    nameof(key));
            }

            return string.Join(
                "&",
                FlattenParamsValue(value, key)
                    .Select(p => $"{UrlEncode(p.Key)}={UrlEncode(p.Value)}"));
        }

        /// <summary>URL-encodes a string.</summary>
        /// <param name="value">The string to URL-encode.</param>
        /// <returns>The URL-encoded string.</returns>
        private static string UrlEncode(string value)
        {
            return WebUtility.UrlEncode(value)
                /* Don't use strict form encoding by changing the square bracket control
                 * characters back to their literals. This is fine by the server, and
                 * makes these parameter strings easier to read. */
                .Replace("%5B", "[")
                .Replace("%5D", "]");
        }

        /// <summary>
        /// Returns a list of parameters for a given value. The value can be basically anything, as
        /// long as it can be encoded in some way.
        /// </summary>
        /// <param name="value">The value for which to create the list of parameters.</param>
        /// <param name="keyPrefix">The key under which new keys should be nested, if any.</param>
        /// <returns>The list of parameters</returns>
        private static List<KeyValuePair<string, string>> FlattenParamsValue(object value, string keyPrefix)
        {
            List<KeyValuePair<string, string>> flatParams = null;

            switch (value)
            {
                case null:
                    flatParams = SingleParam(keyPrefix, string.Empty);
                    break;

                case INestedOptions options:
                    flatParams = FlattenParamsOptions(options, keyPrefix);
                    break;

                case IDictionary dictionary:
                    flatParams = FlattenParamsDictionary(dictionary, keyPrefix);
                    break;

                case string s:
                    flatParams = SingleParam(keyPrefix, s);
                    break;

                case IEnumerable enumerable:
                    flatParams = FlattenParamsList(enumerable, keyPrefix);
                    break;

                case DateTime dateTime:
                    flatParams = SingleParam(
                        keyPrefix,
                        dateTime.ConvertDateTimeToEpoch().ToString(CultureInfo.InvariantCulture));
                    break;

                case Enum e:
                    flatParams = SingleParam(keyPrefix, JsonConvert.SerializeObject(e).Trim('"'));
                    break;

                default:
                    flatParams = SingleParam(
                        keyPrefix,
                        string.Format(CultureInfo.InvariantCulture, "{0}", value));
                    break;
            }

            return flatParams;
        }

        /// <summary>
        /// Returns a list of parameters for a given options class. If a key prefix is provided, the
        /// keys for the new parameters will be nested under the key prefix. E.g. if the key prefix
        /// `foo` is passed and the options class contains a parameter `bar`, then a parameter
        /// with key `foo[bar]` will be returned.
        /// </summary>
        /// <param name="options">The options class for which to create the list of parameters.</param>
        /// <param name="keyPrefix">The key under which new keys should be nested, if any.</param>
        /// <returns>The list of parameters</returns>
        private static List<KeyValuePair<string, string>> FlattenParamsOptions(
            INestedOptions options,
            string keyPrefix)
        {
            List<KeyValuePair<string, string>> flatParams = new List<KeyValuePair<string, string>>();
            if (options == null)
            {
                return flatParams;
            }

            foreach (var property in options.GetType().GetRuntimeProperties())
            {
                // Skip properties not annotated with `[JsonProperty]`
                var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                var value = property.GetValue(options, null);

                // Fields on a class which are never set by the user will contain null values (for
                // reference types), so skip those to avoid encoding them in the request.
                if (value == null)
                {
                    continue;
                }

                string key = attribute.PropertyName;
                string newPrefix = NewPrefix(key, keyPrefix);

                flatParams.AddRange(FlattenParamsValue(value, newPrefix));
            }

            return flatParams;
        }

        /// <summary>
        /// Returns a list of parameters for a given dictionary. If a key prefix is provided, the
        /// keys for the new parameters will be nested under the key prefix. E.g. if the key prefix
        /// `foo` is passed and the dictionary contains a key `bar`, then a parameter with key
        /// `foo[bar]` will be returned.
        /// </summary>
        /// <param name="dictionary">The dictionary for which to create the list of parameters.</param>
        /// <param name="keyPrefix">The key under which new keys should be nested, if any.</param>
        /// <returns>The list of parameters</returns>
        private static List<KeyValuePair<string, string>> FlattenParamsDictionary(
            IDictionary dictionary,
            string keyPrefix)
        {
            List<KeyValuePair<string, string>> flatParams = new List<KeyValuePair<string, string>>();
            if (dictionary == null)
            {
                return flatParams;
            }

            foreach (DictionaryEntry entry in dictionary)
            {
                string key = string.Format(CultureInfo.InvariantCulture, "{0}", entry.Key);
                object value = entry.Value;

                string newPrefix = NewPrefix(key, keyPrefix);

                flatParams.AddRange(FlattenParamsValue(value, newPrefix));
            }

            return flatParams;
        }

        /// <summary>
        /// Returns a list of parameters for a given list of objects. The parameter keys will be
        /// indexed under the `keyPrefix` parameter. E.g. if the `keyPrefix` is `foo`, then the
        /// key for the first element's will be `foo[0]`, etc.
        /// </summary>
        /// <param name="list">The list for which to create the list of parameters.</param>
        /// <param name="keyPrefix">The key under which new keys should be nested.</param>
        /// <returns>The list of parameters</returns>
        private static List<KeyValuePair<string, string>> FlattenParamsList(
            IEnumerable list,
            string keyPrefix)
        {
            List<KeyValuePair<string, string>> flatParams = new List<KeyValuePair<string, string>>();
            if (list == null)
            {
                return flatParams;
            }

            int index = 0;
            foreach (object value in list)
            {
                string newPrefix = $"{keyPrefix}[{index}]";
                flatParams.AddRange(FlattenParamsValue(value, newPrefix));
                index += 1;
            }

            /* Because application/x-www-form-urlencoded cannot represent an empty list, convention
             * is to take the list parameter and just set it to an empty string. (E.g. A regular
             * list might look like `a[0]=1&b[1]=2`. Emptying it would look like `a=`.) */
            if (!flatParams.Any())
            {
                flatParams.Add(new KeyValuePair<string, string>(keyPrefix, string.Empty));
            }

            return flatParams;
        }

        /// <summary>Creates a list containing a single parameter.</summary>
        /// <param name="key">The parameter's key.</param>
        /// <param name="value">The parameter's value.</param>
        /// <returns>A list containg the single parameter.</returns>
        private static List<KeyValuePair<string, string>> SingleParam(string key, string value)
        {
            List<KeyValuePair<string, string>> flatParams = new List<KeyValuePair<string, string>>();
            flatParams.Add(new KeyValuePair<string, string>(key, value));
            return flatParams;
        }

        /// <summary>
        /// Computes the new key prefix, given a key and an existing prefix (if any).
        /// E.g. if the key is `bar` and the existing prefix is `foo`, then `foo[bar]` is returned.
        /// If a key already contains nested values, then only the non-nested part is nested under
        /// the prefix, e.g. if the key is `bar[baz]` and the prefix is `foo`, then `foo[bar][baz]`
        /// is returned.
        /// If no prefix is provided, the key is returned unchanged.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="keyPrefix">The existing key prefix, if any.</param>
        /// <returns>The new key prefix.</returns>
        private static string NewPrefix(string key, string keyPrefix)
        {
            if (string.IsNullOrEmpty(keyPrefix))
            {
                return key;
            }

            int i = key.IndexOf("[", StringComparison.Ordinal);
            if (i == -1)
            {
                return $"{keyPrefix}[{key}]";
            }
            else
            {
                return $"{keyPrefix}[{key.Substring(0, i)}]{key.Substring(i)}";
            }
        }
    }
}
