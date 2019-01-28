namespace StripeTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Stripe;
    using Xunit;

    public class SubscriptionScheduleServiceTest : BaseStripeTest
    {
        private const string ScheduleId = "sub_sched_123";

        private readonly SubscriptionScheduleService service;
        private readonly SubscriptionScheduleCancelOptions cancelOptions;
        private readonly SubscriptionScheduleCreateOptions createOptions;
        private readonly SubscriptionScheduleListOptions listOptions;
        private readonly SubscriptionScheduleReleaseOptions releaseOptions;
        private readonly SubscriptionScheduleUpdateOptions updateOptions;

        public SubscriptionScheduleServiceTest(MockHttpClientFixture mockHttpClientFixture)
            : base(mockHttpClientFixture)
        {
            this.service = new SubscriptionScheduleService();

            this.cancelOptions = new SubscriptionScheduleCancelOptions
            {
                InvoiceNow = true,
                Prorate = true,
            };

            this.createOptions = new SubscriptionScheduleCreateOptions
            {
                CustomerId = "cus_123",
            };

            this.releaseOptions = new SubscriptionScheduleReleaseOptions
            {
                PreserveCancelDate = true,
            };

            this.updateOptions = new SubscriptionScheduleUpdateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "key", "value" },
                },
            };

            this.listOptions = new SubscriptionScheduleListOptions
            {
                Limit = 1,
            };
        }

        [Fact]
        public void Cancel()
        {
            var subscription = this.service.Cancel(ScheduleId, this.cancelOptions);
            this.AssertRequest(HttpMethod.Delete, "/v1/subscriptions/sub_123");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public async Task CancelAsync()
        {
            var subscription = await this.service.CancelAsync(ScheduleId, this.cancelOptions);
            this.AssertRequest(HttpMethod.Delete, "/v1/subscriptions/sub_123");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public void Create()
        {
            var subscription = this.service.Create(this.createOptions);
            this.AssertRequest(HttpMethod.Post, "/v1/subscriptions");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var subscription = await this.service.CreateAsync(this.createOptions);
            this.AssertRequest(HttpMethod.Post, "/v1/subscriptions");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public void Get()
        {
            var subscription = this.service.Get(ScheduleId);
            this.AssertRequest(HttpMethod.Get, "/v1/subscriptions/sub_123");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public async Task GetAsync()
        {
            var subscription = await this.service.GetAsync(ScheduleId);
            this.AssertRequest(HttpMethod.Get, "/v1/subscriptions/sub_123");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public void List()
        {
            var subscriptions = this.service.List(this.listOptions);
            this.AssertRequest(HttpMethod.Get, "/v1/subscriptions");
            Assert.NotNull(subscriptions);
            Assert.Equal("list", subscriptions.Object);
            Assert.Single(subscriptions.Data);
            Assert.Equal("subscription", subscriptions.Data[0].Object);
        }

        [Fact]
        public async Task ListAsync()
        {
            var subscriptions = await this.service.ListAsync(this.listOptions);
            this.AssertRequest(HttpMethod.Get, "/v1/subscriptions");
            Assert.NotNull(subscriptions);
            Assert.Equal("list", subscriptions.Object);
            Assert.Single(subscriptions.Data);
            Assert.Equal("subscription", subscriptions.Data[0].Object);
        }

        [Fact]
        public void ListAutoPaging()
        {
            var subscriptions = this.service.ListAutoPaging(this.listOptions).ToList();
            Assert.NotNull(subscriptions);
            Assert.Equal("subscription", subscriptions[0].Object);
        }

        [Fact]
        public void Update()
        {
            var subscription = this.service.Update(ScheduleId, this.updateOptions);
            this.AssertRequest(HttpMethod.Post, "/v1/subscriptions/sub_123");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var subscription = await this.service.UpdateAsync(ScheduleId, this.updateOptions);
            this.AssertRequest(HttpMethod.Post, "/v1/subscriptions/sub_123");
            Assert.NotNull(subscription);
            Assert.Equal("subscription", subscription.Object);
        }
    }
}
