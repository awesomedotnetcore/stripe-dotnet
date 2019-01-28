namespace Stripe
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public class SubscriptionSchedulePhasePlan : StripeEntity
    {
        /// <summary>
        /// Define thresholds at which an invoice will be sent, and the subscription advanced to a
        /// new billing period.
        /// </summary>
        [JsonProperty("billing_thresholds")]
        public SubscriptionBillingThresholds BillingThresholds { get; set; }

        // TODO: figure out how we document those
        #region Expandable Plan
        [JsonIgnore]
        public string PlanId { get; set; }

        [JsonIgnore]
        public Plan Plan { get; set; }

        [JsonProperty("plan")]
        internal object InternalPlan
        {
            get
            {
                return this.Plan ?? (object)this.PlanId;
            }

            set
            {
                StringOrObject<Plan>.Map(value, s => this.PlanId = s, o => this.Plan = o);
            }
        }
        #endregion

        /// <summary>
        /// Quantity of the plan to which the customer should be subscribed.
        /// </summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
