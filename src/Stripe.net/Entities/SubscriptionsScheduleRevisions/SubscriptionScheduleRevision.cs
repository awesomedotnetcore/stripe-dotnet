namespace Stripe
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public class SubscriptionScheduleRevision : StripeEntity, IHasId, IHasObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// Time at which the object was created. Measured in seconds since the Unix epoch.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Created { get; set; }

        /// <summary>
        /// TODO: Figure out docs.
        /// </summary>
        [JsonProperty("footer")]
        public SubscriptionScheduleInvoiceSettings InvoiceSettings { get; set; }

        /// <summary>
        /// Has the value <code>true</code> if the object exists in live mode or the value
        /// <code>false</code> if the object exists in test mode.
        /// </summary>
        [JsonProperty("livemode")]
        public bool Livemode { get; set; }

        /// <summary>
        /// Configuration for the subscription schedule’s phases.
        /// </summary>
        [JsonProperty("phases")]
        public List<SubscriptionSchedulePhase> Phases { get; set; }

        /// <summary>
        /// TODO: Figure out docs
        /// </summary>
        [JsonProperty("previous_revision")]
        public string PreviousRevisionId { get; set; }

        /// <summary>
        /// Interval and duration at which the subscription schedule renews for when it ends if
        /// <code>renewal_behavior</code> is <code>renew</code>.
        /// </summary>
        [JsonProperty("renewal_behavior")]
        public string RenewalBehavior { get; set; }

        /// <summary>
        /// Interval and duration at which the subscription schedule renews for when it ends if
        /// <code>renewal_behavior</code> is <code>renew</code>.
        /// </summary>
        [JsonProperty("renewal_interval")]
        public SubscriptionScheduleRenewalInterval RenewalInterval { get; set; }

        /// <summary>
        /// ID of the subscription schedule the revision points to.
        /// </summary>
        [JsonProperty("schedule")]
        public string ScheduleId { get; set; }
    }
}
