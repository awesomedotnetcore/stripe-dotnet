namespace Stripe
{
    using System;

    public class StripeResponse
    {
        public string ResponseJson { get; set; }

        public string RequestId { get; set; }

        public DateTime RequestDate { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} {{ RequestId={1}, RequestDate={2} }}",
                this.GetType().FullName,
                this.RequestId,
                this.RequestDate.ToString("s"));
        }
    }
}
