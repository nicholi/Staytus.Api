using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Staytus.Api.Models;
using Staytus.Api.Models.Wire;

namespace Staytus.Api
{
    public partial class StaytusApiClient
    {
        protected const String ISSUES_SERVICE = "issues";

        public Task<StaytusResponseModel<List<PartialIssueModel>>> ListIssuesAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, List<PartialIssueModel>>(GetServiceMethodPath(ISSUES_SERVICE, "all"),
                EMPTY_DATA,
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<BaseIssueModel>> CreateIssueAsync(String title, List<String> servicePermalinks, String statusPermalink, 
            // documentation says its not required, but it is
            StaytusState serviceState,
            String initialUpdate = null, Nullable<Boolean> notify = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, BaseIssueModel>(GetServiceMethodPath(ISSUES_SERVICE, "create"),
                new CreateIssueModel(title, servicePermalinks, statusPermalink, serviceState, initialUpdate, notify),
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<IssueModel>> GetIssueAsync(Int32 id, 
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, IssueModel>(GetServiceMethodPath(ISSUES_SERVICE, "info"),
                new { issue = id },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<BaseIssueModel>> UpdateIssueAsync(Int32 id, 
            // documentation says not required, but it is
            String text,
            // same for this parameter, in fact if this is NOT passed, tons of 500 exceptions will be thrown
            // when attempting to query the issue
            StaytusState serviceState, 
            String statusPermalink = null, Nullable<Boolean> notify = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, BaseIssueModel>(GetServiceMethodPath(ISSUES_SERVICE, "update"),
                new UpdateIssueModel(id, text, serviceState, statusPermalink, notify),
                cancelToken: cancelToken);
        }
    }
}
