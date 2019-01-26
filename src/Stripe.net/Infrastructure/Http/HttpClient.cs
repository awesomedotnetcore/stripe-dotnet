namespace Stripe.Infrastructure.Http
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract base class for HTTP clients used to make requests to Stripe's API.
    /// </summary>
    public abstract class HttpClient
    {
        /// <summary>The last request made by this client.</summary>
        public Request LastRequest { get; protected set; }

        /// <summary>The last response received by this client.</summary>
        public Response LastResponse { get; protected set; }

        /// <summary>Sends a request to Stripe's API as an asynchronous operation.</summary>
        /// <param name="request">The parameters of the request to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public abstract Task<Response> MakeRequestAsync(
            Request request,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
