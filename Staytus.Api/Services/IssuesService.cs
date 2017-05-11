using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Staytus.Api.Models;
using Staytus.Api.Models.Wire;

namespace Staytus.Api
{
    public partial class StaytusApiClient
    {
        protected const String ISSUES_SERVICE = "issues";

        public Task<StaytusResponseModel<List<IssueModel>>> ListIssues()
        {
            return InternalPostAsync<Object, List<IssueModel>>(GetServiceMethodPath(ISSUES_SERVICE, "all"),
                EMPTY_DATA);
        }

        public Task<StaytusResponseModel<BaseIssueModel>> CreateIssue(String title, List<String> servicePermalinks, String statusPermalink, 
            // documentation says its not required, but it is
            StaytusState serviceState,
            String initialUpdate = null, Nullable<Boolean> notify = null)
        {
            return InternalPostAsync<Object, BaseIssueModel>(GetServiceMethodPath(ISSUES_SERVICE, "create"),
                new CreateIssueModel(title, servicePermalinks, statusPermalink, serviceState, initialUpdate, notify));
        }

        public Task<StaytusResponseModel<IssueModel>> GetIssue(Int32 id)
        {
            return InternalPostAsync<Object, IssueModel>(GetServiceMethodPath(ISSUES_SERVICE, "info"),
                new { issue = id });
        }

        public Task<StaytusResponseModel<IssueModel>> UpdateIssue(Int32 id, 
            // documentation says not required, but it is
            String text,
            // same for this parameter, in fact if this is NOT passed, tons of 500 exceptions will be thrown
            // when attempting to query the issue
            StaytusState serviceState, 
            String statusPermalink = null, Nullable<Boolean> notify = null)
        {
            return InternalPostAsync<Object, IssueModel>(GetServiceMethodPath(ISSUES_SERVICE, "update"),
                new UpdateIssueModel(id, text, serviceState, statusPermalink, notify));
        }
    }
}
