namespace Stripe
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public class AccountSettingsPayments : StripeEntity
    {
        [JsonProperty("statement_descriptor")]
        public string StatementDescriptor { get; set; }
    }
}
