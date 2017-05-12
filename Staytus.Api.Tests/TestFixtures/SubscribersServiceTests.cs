using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Staytus.Api.Tests.TestFixtures
{
    public class SubscribersServiceTests : BaseServiceTests
    {
        private static readonly String USER1_EMAIL = "staytusUser1" + NextString(6) + "@influxis.com";
        private static readonly String USER2_EMAIL = "staytusUser2" + NextString(6) + "@influxis.com";

        static Object[] RANDOM_EMAIL1_PARAMS = new Object[]
            {
                new Object[] { USER1_EMAIL }
            };
        static Object[] RANDOM_EMAIL2_PARAMS = new Object[]
            {
                new Object[] { USER2_EMAIL }
            };

        static Object[] RANDOM_ADD_EMAIL1_PARAMS = new Object[]
            {
                new Object[] { USER1_EMAIL, false }
            };
        static Object[] RANDOM_ADD_EMAIL2_PARAMS = new Object[]
            {
                new Object[] { USER2_EMAIL, true }
            };

        public SubscribersServiceTests()
        {
        }

        [OneTimeSetUp]
        public Task SetUp()
        {
            return Task.FromResult((Object)null);
        }

        [OneTimeTearDown]
        public Task TearDown()
        {
            return Task.FromResult((Object)null);
        }

        [Test]
        [Order(0)]
        public async Task FindSubscriberByEmail()
        {
            var subscriberEmail = Configuration.GetValue<String>("staytusApi:tests:subscribers:findByEmail", null);
            Assert.That(subscriberEmail, Is.Not.Null);

            var subscriber = await ApiClient.GetSubscriberByEmailAsync(subscriberEmail);
            Assert.That(subscriber.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(subscriber.Data, Is.Not.Null);
            Assert.That(subscriber.Data.Email, Is.EqualTo(subscriberEmail));
        }

        [Test]
        [Order(1)]
        public async Task FindSubscriberByVerification()
        {
            var subscriberVerification = Configuration.GetValue<String>("staytusApi:tests:subscribers:findByVerification", null);
            Assert.That(subscriberVerification, Is.Not.Null);

            var subscriber = await ApiClient.GetSubscriberByTokenAsync(subscriberVerification);
            Assert.That(subscriber.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(subscriber.Data, Is.Not.Null);
            Assert.That(subscriber.Data.VerificationToken, Is.EqualTo(subscriberVerification));
        }

        [TestCaseSource(nameof(RANDOM_ADD_EMAIL1_PARAMS))]
        [TestCaseSource(nameof(RANDOM_ADD_EMAIL2_PARAMS))]
        [Order(20)]
        public async Task AddSubscriber(String email, Boolean verified)
        {
            var subscriber = await ApiClient.AddSubscriberAsync(email, verified);
            Assert.That(subscriber.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(subscriber.Data, Is.Not.Null);
            Assert.That(subscriber.Data.Email, Is.EqualTo(email));
            if (verified)
            {
                Assert.That(subscriber.Data.VerifiedAt, Is.Not.Null);
            }
            else
            {
                Assert.That(subscriber.Data.VerifiedAt, Is.Null);
            }
        }

        [TestCaseSource(nameof(RANDOM_EMAIL1_PARAMS))]
        [Order(40)]
        public async Task VerifySubscriber(String email)
        {
            var subscriber = await ApiClient.GetSubscriberByEmailAsync(email);
            Assert.That(subscriber.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(subscriber.Data, Is.Not.Null);
            Assert.That(subscriber.Data.Email, Is.EqualTo(email));
            Assert.That(subscriber.Data.VerifiedAt, Is.Null);

            var verifiedSub = await ApiClient.VerifySubscriberAsync(email);
            Assert.That(verifiedSub.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(verifiedSub.Data, Is.True);

            var subscriberAfterVerification = await ApiClient.GetSubscriberByEmailAsync(email);
            Assert.That(subscriberAfterVerification.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(subscriberAfterVerification.Data, Is.Not.Null);
            Assert.That(subscriberAfterVerification.Data.Email, Is.EqualTo(email));
            Assert.That(subscriberAfterVerification.Data.VerifiedAt, Is.Not.Null);
        }

        [TestCaseSource(nameof(RANDOM_EMAIL1_PARAMS))]
        [Order(60)]
        public async Task SendSubscriberVerification(String email)
        {
            var sendEmail = await ApiClient.SendSubscriberVerificationAsync(email);
            Assert.That(sendEmail.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(sendEmail.Data, Is.Not.Null);
        }

        [TestCaseSource(nameof(RANDOM_EMAIL1_PARAMS))]
        [TestCaseSource(nameof(RANDOM_EMAIL2_PARAMS))]
        [Order(80)]
        public async Task DeleteSubscriber(String email)
        {
            // assure email exists because you can run the call on emails that don't exist successfully
            var getSub = await ApiClient.GetSubscriberByEmailAsync(email);
            Assert.That(getSub.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(getSub.Data, Is.Not.Null);
            
            var sendEmail = await ApiClient.DeleteSubscriberAsync(email);
            Assert.That(sendEmail.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(sendEmail.Data, Has.Count.Positive);
        }
    }
}
