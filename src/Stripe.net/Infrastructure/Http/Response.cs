namespace Stripe.Infrastructure.Http
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;

    /// <summary>
    /// Represents a response from Stripe's API.
    /// </summary>
    public class Response
    {
        /// <summary>Initializes a new instance of the <see cref="Response"/> class.</summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="headers">The HTTP headers of the response.</param>
        /// <param name="content">The body of the response.</param>
        public Response(HttpStatusCode statusCode, HttpHeaders headers, string content)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
            this.Content = content;
        }

        /// <summary>The body of the response.</summary>
        public string Content { get; }

        /// <summary>The date key of the response, if any.</summary>
        public DateTime? Date
        {
            get
            {
                var dateString = GetHeader(this.Headers, "Date");
                if (string.IsNullOrEmpty(dateString))
                {
                    return null;
                }

                return Convert.ToDateTime(dateString, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>The HTTP headers of the response.</summary>
        public HttpHeaders Headers { get; }

        /// <summary>The idempotency key of the response, if any.</summary>
        public string IdempotencyKey => GetHeader(this.Headers, "Idempotency-Key");

        /// <summary>The request ID of the response, if any.</summary>
        public string RequestId => GetHeader(this.Headers, "Request-Id");

        /// <summary>The HTTP status code of the response.</summary>
        public HttpStatusCode StatusCode { get; }

        private static string GetHeader(HttpHeaders headers, string name)
        {
            if (headers == null || !headers.Contains(name))
            {
                return null;
            }

            return headers.GetValues(name).First();
        }
    }
}
