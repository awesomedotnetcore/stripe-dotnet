namespace StripeTests
{
    using System;
    using Stripe;
    using Stripe.Infrastructure.Extensions;
    using Xunit;

    public class ListOptionsWithCreatedTest : BaseStripeTest
    {
        [Fact]
        public void SerializeNull()
        {
            var options = new ListOptionsWithCreated
            {
                Created = null,
            };

            Assert.Equal(string.Empty, options.ToQueryString());
        }

        [Fact]
        public void SerializeDateTime()
        {
            var options = new ListOptionsWithCreated
            {
                Created = DateTime.Parse("Fri, 13 Feb 2009 23:31:30Z"),
            };

            Assert.Equal("created=1234567890", options.ToQueryString());
        }

        [Fact]
        public void SerializeDateTimeNull()
        {
            var options = new ListOptionsWithCreated
            {
                Created = (DateTime?)null,
            };

            Assert.Equal("created=", options.ToQueryString());
        }

        [Fact]
        public void SerializeDateRangeOptions()
        {
            var options = new ListOptionsWithCreated
            {
                Created = new DateRangeOptions
                {
                    GreaterThanOrEqual = DateTime.Parse("Fri, 13 Feb 2009 23:31:30Z"),
                    LessThan = DateTime.Parse("Sun, 1 May 2044 01:28:21Z"),
                },
            };

            Assert.Equal("created[gte]=1234567890&created[lt]=2345678901", options.ToQueryString());
        }
    }
}