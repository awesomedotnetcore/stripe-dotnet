namespace Stripe.Infrastructure.Http
{
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

        /// <summary>The HTTP status code of the response.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>The HTTP headers of the response.</summary>
        public HttpHeaders Headers { get; }

        /// <summary>The body of the response.</summary>
        public string Content { get; }
    }
}
