namespace StripeTests
{
    using Newtonsoft.Json;
    using Stripe;
    using Xunit;

    public class SubscriptionScheduleTest : BaseStripeTest
    {
        public SubscriptionScheduleTest(StripeMockFixture stripeMockFixture)
            : base(stripeMockFixture)
        {
        }

        [Fact]
        public void Deserialize()
        {
            string json = this.GetFixture("/v1/subscription_schedules/sub_sched_123");
            var schedule = JsonConvert.DeserializeObject<SubscriptionSchedule>(json);
            Assert.NotNull(schedule);
            Assert.IsType<SubscriptionSchedule>(schedule);
            Assert.NotNull(schedule.Id);
            Assert.Equal("subscription_schedule", schedule.Object);
        }

        [Fact]
        public void DeserializeWithExpansions()
        {
            string[] expansions =
            {
              "customer",
              "phases.coupon",
              "phases.plans.plan",
              "subscription"
            };

            string json = this.GetFixture("/v1/subscription_schedules/sub_sched_123", expansions);
            var schedule = JsonConvert.DeserializeObject<SubscriptionSchedule>(json);
            Assert.NotNull(schedule);
            Assert.IsType<SubscriptionSchedule>(schedule);
            Assert.NotNull(schedule.Id);
            Assert.Equal("subscription_schedule", schedule.Object);

            Assert.NotNull(schedule.Customer);
            Assert.Equal("customer", schedule.Customer.Object);

            Assert.NotNull(schedule.Phases[0].Coupon);
            Assert.Equal("coupon", schedule.Phases[0].Coupon.Object);

            Assert.NotNull(schedule.Phases[0].Plans[0].Plan);
            Assert.Equal("plan", schedule.Phases[0].Plans[0].Plan.Object);

            Assert.NotNull(schedule.Subscription);
            Assert.Equal("subscription", schedule.Subscription.Object);
        }
    }
}
