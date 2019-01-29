namespace Stripe
{
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public class PersonVerification : StripeEntity
    {
        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("details_code")]
        public string DetailsCode { get; set; }

        [JsonProperty("document")]
        public PersonVerificationDocument Document { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
