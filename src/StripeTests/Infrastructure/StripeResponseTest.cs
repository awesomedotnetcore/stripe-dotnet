namespace StripeTests
{
    using System.Collections.Generic;
    using Stripe;
    using Xunit;

    public class StripeResponseTest : BaseStripeTest
    {
        private readonly StripeList<Charge> charges;

        public StripeResponseTest()
        {
            this.charges = new ChargeService().List();
        }

        [Fact]
        public void Initializes()
        {
            Assert.NotNull(this.charges);
            Assert.NotNull(this.charges.StripeResponse);
            Assert.NotNull(this.charges.StripeResponse.RequestId);
            Assert.NotNull(this.charges.StripeResponse.Content);
            Assert.True(this.charges.StripeResponse.Date?.Year > 0);
        }
    }
}
