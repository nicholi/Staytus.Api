using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Staytus.Api.Models;

namespace Staytus.Api
{
    public partial class StaytusApiClient
    {
        protected const String SERVICES_SERVICE = "services";

        public Task<StaytusResponseModel<List<ServiceModel>>> ListServices()
        {
            return InternalPostAsync<Object, List<ServiceModel>>(GetServiceMethodPath(SERVICES_SERVICE, "all"),
                EMPTY_DATA);
        }

        public Task<StaytusResponseModel<ServiceModel>> GetService(String servicePermalink)
        {
            return InternalPostAsync<Object, ServiceModel>(GetServiceMethodPath(SERVICES_SERVICE, "info"),
                new { service = servicePermalink });
        }

        public Task<StaytusResponseModel<ServiceModel>> SetServiceStatus(String servicePermalink, String statusPermalink)
        {
            return InternalPostAsync<Object, ServiceModel>(GetServiceMethodPath(SERVICES_SERVICE, "set_status"),
                new { service = servicePermalink, status = statusPermalink });
        }
    }
}
