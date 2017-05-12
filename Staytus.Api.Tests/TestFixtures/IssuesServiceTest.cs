using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Staytus.Api.Models;

namespace Staytus.Api.Tests.TestFixtures
{
    [TestFixture]
    public class IssuesServiceTest : BaseServiceTests
    {
        public IssuesServiceTest()
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

        static readonly String RANDOM_ISSUE1_TITLE = "Just a Test1--" + NextString(10);
        static Object[] RANDOM_ISSUE1_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE1_TITLE }
            };

        static Object[] RANDOM_CREATE_ISSUE1_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE1_TITLE, StaytusState.Monitoring, true, null }
            };
        static Object[] RANDOM_UPDATE_ISSUE1_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE1_TITLE, StaytusState.Investigating }
            };
        static Object[] RANDOM_RESOLVE_ISSUE1_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE1_TITLE, 3 }
            };

        static Object[] RANDOM_FINAL_ISSUE1_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE1_TITLE, 3 }
            };

        static readonly String RANDOM_ISSUE2_TITLE = "Just a Test2--" + NextString(10);
        static Object[] RANDOM_CREATE_ISSUE2_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE2_TITLE, StaytusState.Identified, false, "Everything is fine. " + NextString(20) }
            };
        static Object[] RANDOM_RESOLVE_ISSUE2_PARAMS = new Object[]
            {
                new Object[] { RANDOM_ISSUE2_TITLE, 2 }
            };

        static Object[] RANDOM_FINAL_ISSUE2_PARAMS = new Object[]
    {
                new Object[] { RANDOM_ISSUE2_TITLE, 2 }
    };

        [TestCaseSource(nameof(RANDOM_CREATE_ISSUE1_PARAMS))]
        [TestCaseSource(nameof(RANDOM_CREATE_ISSUE2_PARAMS))]
        [Order(0)]
        public async Task CreateIssue(String title, StaytusState serviceState, Boolean assertInitialStatus, String initialText)
        {
            var servicePermalinks = Configuration.GetSection("staytusApi:tests:issues:servicePermalinks").Get<List<String>>();
            Assert.That(servicePermalinks, Is.Not.Null);
            Assert.That(servicePermalinks, Has.Count.Positive);
            var statusPermalink = Configuration.GetValue<String>("staytusApi:tests:issues:statusPermalink", null);
            Assert.That(statusPermalink, Is.Not.Null);

            if (assertInitialStatus)
            {
                var initialServices = await ApiClient.ListServicesAsync();
                var initialAffectedServices = initialServices.Data.Where(x => servicePermalinks.Contains(x.Permalink)).ToList();
                foreach (var affectedService in initialAffectedServices)
                {
                    // so we can see the change after creating issue
                    Assert.That(affectedService.Status.Permalink, Is.Not.EqualTo(statusPermalink));
                }
            }

            var newIssue = await ApiClient.CreateIssueAsync(title, servicePermalinks, statusPermalink, serviceState, initialText);
            Assert.That(newIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(newIssue.Data, Is.Not.Null);
            Assert.That(newIssue.Data.Title, Is.EqualTo(title));

            var finalServices = await ApiClient.ListServicesAsync();
            var finalAffectedServices = finalServices.Data.Where(x => servicePermalinks.Contains(x.Permalink)).ToList();
            foreach (var affectedService in finalAffectedServices)
            {
                // we've modified services with issue
                Assert.That(affectedService.Status.Permalink, Is.EqualTo(statusPermalink));
            }

            var fullIssue = await ApiClient.GetIssueAsync(newIssue.Data.Id);
            Assert.That(fullIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(fullIssue.Data, Is.Not.Null);
            Assert.That(fullIssue.Data.Updates, Has.Count.EqualTo(1));
            if (initialText != null)
            {
                Assert.That(fullIssue.Data.Updates.FirstOrDefault().Text, Is.EqualTo(initialText));
            }
        }

        [TestCaseSource(nameof(RANDOM_UPDATE_ISSUE1_PARAMS))]
        [Order(10)]
        public async Task UpdateIssueBareParams(String findIssueByTitle, StaytusState serviceState)
        {
            var issues = await ApiClient.ListIssuesAsync();
            Assert.That(issues.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(issues.Data, Is.Not.Null);

            var foundIssue = issues.Data.SingleOrDefault(x => String.Equals(x.Title, findIssueByTitle));
            Assert.That(foundIssue, Is.Not.Null);

            var updateIssue = await ApiClient.UpdateIssueAsync(foundIssue.Id, "Testy test test. " + NextString(10), serviceState);
            Assert.That(updateIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(updateIssue.Data, Is.Not.Null);

            var modifiedIssue = await ApiClient.GetIssueAsync(foundIssue.Id);
            Assert.That(modifiedIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(modifiedIssue.Data, Is.Not.Null);
            Assert.That(modifiedIssue.Data.Updates, Has.Count.EqualTo(2));
        }

        [TestCaseSource(nameof(RANDOM_RESOLVE_ISSUE1_PARAMS))]
        [TestCaseSource(nameof(RANDOM_RESOLVE_ISSUE2_PARAMS))]
        [Order(11)]
        public async Task UpdateIssueResolve(String findIssueByTitle, int numUpdates)
        {
            var statusPermalink = Configuration.GetValue<String>("staytusApi:tests:issues:resolvedNormalStatusPermalink", null);
            Assert.That(statusPermalink, Is.Not.Null);

            var issues = await ApiClient.ListIssuesAsync();
            Assert.That(issues.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(issues.Data, Is.Not.Null);

            var foundIssue = issues.Data.SingleOrDefault(x => String.Equals(x.Title, findIssueByTitle));
            Assert.That(foundIssue, Is.Not.Null);

            var updateIssue = await ApiClient.UpdateIssueAsync(foundIssue.Id, "Resolvy resolve resolve. " + NextString(10), StaytusState.Resolved, statusPermalink);
            Assert.That(updateIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(updateIssue.Data, Is.Not.Null);

            var modifiedIssue = await ApiClient.GetIssueAsync(foundIssue.Id);
            Assert.That(modifiedIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(modifiedIssue.Data, Is.Not.Null);
            Assert.That(modifiedIssue.Data.Updates, Has.Count.EqualTo(numUpdates));
        }

        [TestCaseSource(nameof(RANDOM_ISSUE1_PARAMS))]
        [Order(20)]
        public async Task ListIssues(String insertedIssueTitle)
        {
            var issues = await ApiClient.ListIssuesAsync();
            Assert.That(issues.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(issues.Data, Is.Not.Null);
            // definitely should be 1 issue since we added it
            Assert.That(issues.Data, Has.Count.Positive);
            Assert.That(issues.Data.Any(x => String.Equals(x.Title, insertedIssueTitle)));
        }

        [TestCaseSource(nameof(RANDOM_FINAL_ISSUE1_PARAMS))]
        [TestCaseSource(nameof(RANDOM_FINAL_ISSUE2_PARAMS))]
        [Order(40)]
        public async Task GetIssue(String findIssueByTitle, int numUpdates)
        {
            var statusPermalink = Configuration.GetValue<String>("staytusApi:tests:issues:resolvedNormalStatusPermalink", null);
            Assert.That(statusPermalink, Is.Not.Null);

            var issues = await ApiClient.ListIssuesAsync();
            Assert.That(issues.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(issues.Data, Is.Not.Null);

            var foundIssue = issues.Data.SingleOrDefault(x => String.Equals(x.Title, findIssueByTitle));
            Assert.That(foundIssue, Is.Not.Null);

            // now query the issue by ID
            var getIssue = await ApiClient.GetIssueAsync(foundIssue.Id);
            Assert.That(getIssue.Status, Is.EqualTo(SystemMessages.SUCCESS));
            Assert.That(getIssue.Data, Is.Not.Null);
            Assert.That(getIssue.Data.Title, Is.EqualTo(findIssueByTitle));
            Assert.That(getIssue.Data.State, Is.EqualTo(StaytusState.Resolved));
            Assert.That(getIssue.Data.Status.Permalink, Is.EqualTo(statusPermalink));
            Assert.That(getIssue.Data.Updates, Has.Count.EqualTo(numUpdates));
        }
    }
}
