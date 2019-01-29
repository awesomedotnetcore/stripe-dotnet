namespace Stripe
{
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public class PersonVerificationDocument : StripeEntity
    {
        #region Expandable Back

        /// <summary>
        /// (ID of a <see cref="File"/>) A photo (jpg or png) of the back of an identifying
        /// document, either a passport or local ID card.
        /// <para>Expandable.</para>
        /// </summary>
        [JsonIgnore]
        public string BackId { get; set; }

        /// <summary>
        /// (Expanded) A photo (jpg or png) of the back of an identifying document, either a
        /// passport or local ID card.
        /// </summary>
        [JsonIgnore]
        public File Back { get; set; }

        [JsonProperty("back")]
        internal object InternalBack
        {
            get
            {
                return this.Back ?? (object)this.BackId;
            }

            set
            {
                StringOrObject<File>.Map(value, s => this.BackId = s, o => this.Back = o);
            }
        }
        #endregion

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("details_code")]
        public string DetailsCode { get; set; }

        #region Expandable Front

        /// <summary>
        /// (ID of a <see cref="File"/>) A photo (jpg or png) of the front of an identifying
        /// front, either a passport or local ID card.
        /// <para>Expandable.</para>
        /// </summary>
        [JsonIgnore]
        public string FrontId { get; set; }

        /// <summary>
        /// (Expanded) A photo (jpg or png) of the front of an identifying front, either a
        /// passport or local ID card.
        /// </summary>
        [JsonIgnore]
        public File Front { get; set; }

        [JsonProperty("front")]
        internal object InternalFront
        {
            get
            {
                return this.Front ?? (object)this.FrontId;
            }

            set
            {
                StringOrObject<File>.Map(value, s => this.FrontId = s, o => this.Front = o);
            }
        }
        #endregion
    }
}
