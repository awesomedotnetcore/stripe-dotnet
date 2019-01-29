namespace Stripe
{
    using Newtonsoft.Json;

    // TODO: Decide if we want to rename that class
    public class BirthDay : StripeEntity
    {
        [JsonProperty("day")]
        public long? Day { get; set; }

        [JsonProperty("month")]
        public long? Month { get; set; }

        [JsonProperty("year")]
        public long? Year { get; set; }
    }
}
