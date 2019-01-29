namespace Stripe
{
    using System.Collections.Generic;
    using Stripe.Infrastructure.Http;

    /// <summary>
    /// Interface that identifies all entities returned by Stripe.
    /// </summary>
    public interface IStripeEntity
    {
        Response StripeResponse { get; set; }
    }
}
