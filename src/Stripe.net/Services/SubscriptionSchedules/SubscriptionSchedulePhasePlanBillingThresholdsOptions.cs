namespace Stripe
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class SubscriptionSchedulePhasePlanBillingThresholdsOptions : StripeEntity
    {
        /// <summary>
        /// Usage threshold that triggers the subscription to advance to a new billing period.
        /// </summary>
        [JsonProperty("usage_gte")]
        public long? UsageGte { get; set; }
    }
}
