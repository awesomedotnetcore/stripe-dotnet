namespace Stripe
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public class AccountBusinessProfile : StripeEntity
    {
        #region Expandable Logo

        /// <summary>
        /// (ID of a <see cref="File"/>) A logo for this account (at least 128px x 128px).
        /// <para>Expandable.</para>
        /// </summary>
        [JsonIgnore]
        public string LogoId { get; set; }

        /// <summary>
        /// (Expanded) A logo for this account (at least 128px x 128px).
        /// </summary>
        [JsonIgnore]
        public File Logo { get; set; }

        [JsonProperty("logo")]
        internal object InternalLogo
        {
            get
            {
                return this.Logo ?? (object)this.LogoId;
            }

            set
            {
                StringOrObject<File>.Map(value, s => this.LogoId = s, o => this.Logo = o);
            }
        }
        #endregion

        [JsonProperty("mcc")]
        public string Mcc { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }

        [JsonProperty("support_address")]
        public Address SupportAddress { get; set; }

        [JsonProperty("support_email")]
        public string SupportEmail { get; set; }

        [JsonProperty("support_phone")]
        public string SupportPhone { get; set; }

        [JsonProperty("support_url")]
        public string SupportUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
