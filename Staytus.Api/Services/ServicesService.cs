using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Staytus.Api.Models;

namespace Staytus.Api
{
    public partial class StaytusApiClient
    {
        protected const String SERVICES_SERVICE = "services";

        public Task<StaytusResponseModel<List<ServiceModel>>> ListServicesAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, List<ServiceModel>>(GetServiceMethodPath(SERVICES_SERVICE, "all"),
                EMPTY_DATA,
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<ServiceModel>> GetServiceAsync(String servicePermalink,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, ServiceModel>(GetServiceMethodPath(SERVICES_SERVICE, "info"),
                new { service = servicePermalink },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<ServiceModel>> SetServiceStatusAsync(String servicePermalink, String statusPermalink,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, ServiceModel>(GetServiceMethodPath(SERVICES_SERVICE, "set_status"),
                new { service = servicePermalink, status = statusPermalink },
                cancelToken: cancelToken);
        }
    }
}
