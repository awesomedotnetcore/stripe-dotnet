namespace Stripe.Infrastructure.Http
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Standard client to make requests to Stripe's API, using
    /// <see cref="System.Net.Http.HttpClient"/> to send HTTP requests.
    /// </summary>
    public class SystemNetHttpClient : HttpClient
    {
        private static readonly string UserAgentString
            = $"Stripe/v1 .NetBindings/{StripeConfiguration.StripeNetVersion}";

        private static readonly string StripeClientUserAgentString
            = BuildStripeClientUserAgentString();

        private readonly System.Net.Http.HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemNetHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">
        /// The <see cref="System.Net.Http.HttpClient"/> client to use. If <c>null</c>, an HTTP
        /// client will be created with default parameters.
        /// </param>
        public SystemNetHttpClient(System.Net.Http.HttpClient httpClient = null)
        {
            this.httpClient = httpClient ?? BuildDefaultSystemNetHttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Net.Http.HttpClient"/> class
        /// with default parameters.
        /// </summary>
        /// <returns>The new instance of the <see cref="System.Net.Http.HttpClient"/> class.</returns>
        public static System.Net.Http.HttpClient BuildDefaultSystemNetHttpClient()
        {
            return new System.Net.Http.HttpClient()
            {
                Timeout = StripeConfiguration.DefaultHttpTimeout,
            };
        }

        /// <summary>Sends a request to Stripe's API as an asynchronous operation.</summary>
        /// <param name="request">The parameters of the request to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override async Task<Response> MakeRequestAsync(
            Request request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var httpRequest = BuildRequestMessage(request);

            this.LastRequest = request;
            this.LastResponse = null;

            // TODO: telemetry
            // TODO: request retries
            var response = await this.httpClient.SendAsync(httpRequest, cancellationToken)
                .ConfigureAwait(false);
            var reader = new StreamReader(
                await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
            this.LastResponse = new Response(
                response.StatusCode,
                response.Headers,
                await reader.ReadToEndAsync().ConfigureAwait(false));

            return this.LastResponse;
        }

        private static System.Net.Http.HttpRequestMessage BuildRequestMessage(Request request)
        {
            var requestMessage = new System.Net.Http.HttpRequestMessage(
                request.Method,
                request.Uri);

            // User agent headers
            // These are the same for every request. We prefer to set them here rather than through
            // the client's `DefaultRequestHeaders` so that they're emitted even when a custom
            // client is used
            requestMessage.Headers.UserAgent.ParseAdd(UserAgentString);
            requestMessage.Headers.Add("X-Stripe-Client-User-Agent", StripeClientUserAgentString);

            // Request headers
            requestMessage.Headers.Authorization = request.AuthorizationHeader;
            foreach (var header in request.StripeHeaders)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            // Request body
            if (request.Content != null)
            {
                requestMessage.Content = request.Content;
            }

            return requestMessage;
        }

        private static string BuildStripeClientUserAgentString()
        {
            var values = new Dictionary<string, string>
            {
                { "bindings_version", StripeConfiguration.StripeNetVersion },
                { "lang", ".net" },
                { "publisher", "stripe" },
                { "lang_version", RuntimeInformation.GetLanguageVersion() },
                { "os_version", RuntimeInformation.GetOSVersion() },
            };

#if NET45
            string monoVersion = RuntimeInformation.GetMonoVersion();
            if (!string.IsNullOrEmpty(monoVersion))
            {
                values.Add("mono_version", monoVersion);
            }
#endif

            return JsonConvert.SerializeObject(values, Formatting.None);
        }
    }
}
