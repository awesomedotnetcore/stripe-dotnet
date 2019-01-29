namespace Stripe
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Stripe.Infrastructure;

    public abstract class AccountSharedOptions : BaseOptions
    {
        [JsonProperty("account_token")]
        public string AccountToken { get; set; }

        [JsonProperty("business_logo")]
        public string BusinessLogoFileId { get; set; }

        [JsonProperty("business_name")]
        public string BusinessName { get; set; }

        [JsonProperty("business_primary_color")]
        public string BusinessPrimaryColor { get; set; }

        [JsonProperty("business_url")]
        public string BusinessUrl { get; set; }

        [JsonProperty("debit_negative_balances")]
        public bool? DebitNegativeBalances { get; set; }

        [JsonProperty("decline_charge_on")]
        public AccountDeclineOptions DeclineChargeOn { get; set; }

        [JsonProperty("default_currency")]
        public string DefaultCurrency { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// <para>
        /// A card or bank account to attach to the account. You can provide either a token, like
        /// the ones returned by <a href="https://stripe.com/docs/stripe.js">Stripe.js</a>, or a
        /// <see cref="AccountBankAccountOptions"/> or <see cref="AccountCardOptions"/> instance.
        /// </para>
        /// <para>
        /// By default, providing an external account sets it as the new default external account
        /// for its currency, and deletes the old default if one exists. To add additional external
        /// accounts without replacing the existing default for the currency, use the bank account
        /// or card creation API.
        /// </para>
        /// </summary>
        [JsonProperty("external_account")]
        [JsonConverter(typeof(AnyOfConverter))]
        public AnyOf<string, AccountBankAccountOptions, AccountCardOptions> ExternalAccount { get; set; }

        [JsonProperty("legal_entity")]
        public AccountLegalEntityOptions LegalEntity { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("payout_schedule")]
        public AccountPayoutScheduleOptions PayoutSchedule { get; set; }

        [JsonProperty("payout_statement_descriptor")]
        public string PayoutStatementDescriptor { get; set; }

        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }

        [JsonProperty("statement_descriptor")]
        public string StatementDescriptor { get; set; }

        [JsonProperty("support_email")]
        public string SupportEmail { get; set; }

        [JsonProperty("support_phone")]
        public string SupportPhone { get; set; }

        [JsonProperty("support_url")]
        public string SupportUrl { get; set; }

        [JsonProperty("tos_acceptance")]
        public AccountTosAcceptanceOptions TosAcceptance { get; set; }
    }
}
