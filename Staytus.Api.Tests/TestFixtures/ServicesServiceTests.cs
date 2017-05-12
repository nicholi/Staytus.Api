using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Staytus.Api.Models;

namespace Staytus.Api.Tests.TestFixtures
{
    [TestFixture]
    public class ServicesServiceTests : BaseServiceTests
    {
        public ServicesServiceTests()
        {
        }

        [OneTimeSetUp]
        public Task SetUp()
        {
            return Task.FromResult((Object) null);
        }

        [OneTimeTearDown]
        public Task TearDown()
        {
            return Task.FromResult((Object)null);
        }

        [Test]
        [Order(0)]
        public async Task ListServices()
        {
            var listServices = await ApiClient.ListServicesAsync();
            Assert.That(listServices.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(listServices.Data, Has.Count.Positive);
        }

        [Test]
        [Order(20)]
        public async Task GetService()
        {
            var servicePermalink = Configuration.GetValue<String>("staytusApi:tests:services:findByPermalink", null);
            Assert.That(servicePermalink, Is.Not.Null);

            var getService = await ApiClient.GetServiceAsync(servicePermalink);
            Assert.That(getService.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(getService.Data, Is.Not.Null);
            Assert.That(getService.Data.Permalink, Is.EqualTo(servicePermalink));
        }

        [Test]
        [Order(40)]
        public async Task SetServiceStatus()
        {
            var servicePermalink = Configuration.GetValue<String>("staytusApi:tests:services:findByPermalink", null);
            Assert.That(servicePermalink, Is.Not.Null);
            var newStatusPermalink = Configuration.GetValue<String>("staytusApi:tests:services:newStatusPermalink", null);
            Assert.That(newStatusPermalink, Is.Not.Null);

            var getService = await ApiClient.GetServiceAsync(servicePermalink);
            Assert.That(getService.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(getService.Data, Is.Not.Null);
            Assert.That(getService.Data.Permalink, Is.EqualTo(servicePermalink));

            var origStatus = getService.Data.Status;
            Assert.That(origStatus.Permalink, Is.Not.EqualTo(newStatusPermalink));

            var modifyStatus1 = await ApiClient.SetServiceStatusAsync(servicePermalink, newStatusPermalink);
            Assert.That(modifyStatus1.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(modifyStatus1.Data, Is.Not.Null);
            Assert.That(modifyStatus1.Data.Status.Permalink, Is.EqualTo(newStatusPermalink));

            // switch status back
            var modifyStatus2 = await ApiClient.SetServiceStatusAsync(servicePermalink, origStatus.Permalink);
            Assert.That(modifyStatus2.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(modifyStatus2.Data, Is.Not.Null);
            Assert.That(modifyStatus2.Data.Status.Permalink, Is.EqualTo(origStatus.Permalink));
        }
    }
}
