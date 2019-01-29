namespace Stripe
{
    using System;
    using Newtonsoft.Json;

    public class AccountCapabilities : StripeEntity
    {
        [JsonProperty("card_payments")]
        public string CardPayments { get; set; }

        [JsonProperty("legacy_payments")]
        public string LegacyPayments { get; set; }

        [JsonProperty("platform_payments")]
        public string PlatformPayments { get; set; }
    }
}
