namespace Stripe.Infrastructure.FormEncoding
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Stripe.Infrastructure.Extensions;

    public class StripeOptionsFormUrlEncodedContent : ByteArrayContent
    {
        public StripeOptionsFormUrlEncodedContent(BaseOptions options)
            : base(GetContentByteArray(options))
        {
            this.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            this.Headers.ContentType.CharSet = "utf-8";
        }

        private static byte[] GetContentByteArray(BaseOptions options)
        {
            if (options == null)
            {
                return new byte[0];
            }

            return Encoding.UTF8.GetBytes(options.ToQueryString());
        }
    }
}
