namespace Stripe.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Stripe.Infrastructure.Http;

    public class StripeClient : IStripeClient
    {
        /// <summary>Initializes a new instance of the <see cref="StripeClient"/> class.</summary>
        /// <param name="httpClient">
        /// The <see cref="Stripe.Infrastructure.Http.HttpClient"/> client to use. If <c>null</c>,
        /// an HTTP client will be created with default parameters.
        /// </param>
        public StripeClient(Stripe.Infrastructure.Http.HttpClient httpClient = null)
        {
            this.HttpClient = httpClient ?? BuildDefaultHttpClient();
        }

        public Stripe.Infrastructure.Http.HttpClient HttpClient { get; }

        /// <summary>Sends a request to Stripe's API as an asynchronous operation.</summary>
        /// <typeparam name="T">Type of the Stripe entity returned by the API.</typeparam>
        /// <param name="method">The HTTP method.</param>
        /// <param name="path">The path of the request.</param>
        /// <param name="options">The parameters of the request.</param>
        /// <param name="requestOptions">The special modifiers of the request.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<T> RequestAsync<T>(
            HttpMethod method,
            string path,
            BaseOptions options,
            RequestOptions requestOptions,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : IStripeEntity
        {
            var request = new Request(method, path, options, requestOptions);

            var response = await this.HttpClient.MakeRequestAsync(request);

            return ProcessResponse<T>(response);
        }

        private static Stripe.Infrastructure.Http.HttpClient BuildDefaultHttpClient()
        {
            return new SystemNetHttpClient();
        }

        private static T ProcessResponse<T>(Response response)
            where T : IStripeEntity
        {
            var stripeResponse = BuildStripeResponse(response);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw BuildStripeException(response, stripeResponse);
            }

            var obj = StripeEntity.FromJson<T>(response.Content);
            obj.StripeResponse = stripeResponse;

            return obj;
        }

        private static StripeResponse BuildStripeResponse(Response response)
        {
            return new StripeResponse
            {
                RequestId = response.Headers.Contains("Request-Id")
                    ? response.Headers.GetValues("Request-Id").First()
                    : "n/a",
                RequestDate = response.Headers.Contains("Date")
                    ? Convert.ToDateTime(
                        response.Headers.GetValues("Date").First(),
                        CultureInfo.InvariantCulture)
                    : default(DateTime),
                ResponseJson = response.Content,
            };
        }

        private static StripeException BuildStripeException(
            Response response,
            StripeResponse stripeResponse)
        {
                var stripeError = false // TODO
                    ? StripeError.FromJson(response.Content)
                    : StripeError.FromJson(JObject.Parse(response.Content)["error"].ToString()); // TODO
                stripeError.StripeResponse = stripeResponse;

                return new StripeException(
                    response.StatusCode,
                    stripeError,
                    stripeError.Message)
                {
                    StripeResponse = stripeResponse,
                };
        }
    }
}
