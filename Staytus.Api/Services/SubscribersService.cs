using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Staytus.Api.Models;

namespace Staytus.Api
{
    public partial class StaytusApiClient
    {
        protected const String SUBSCRIBERS_SERVICE = "subscribers";

        public Task<StaytusResponseModel<SubscriberModel>> GetSubscriberByEmailAsync(String email,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, SubscriberModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "info"),
                new { email_address = email },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<SubscriberModel>> GetSubscriberByTokenAsync(String verificationToken,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, SubscriberModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "info"),
                new { verification_token = verificationToken },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<SubscriberModel>> AddSubscriberAsync(String email, Boolean verified = false,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, SubscriberModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "add"),
                new { email_address = email, verified = verified ? 1 : 0 },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<Boolean>> VerifySubscriberAsync(String email,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, Boolean>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "verify"),
                new { email_address = email },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<VerifiedEmailModel>> SendSubscriberVerificationAsync(String email,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, VerifiedEmailModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "send_verification_email"),
                new { email_address = email },
                cancelToken: cancelToken);
        }

        public Task<StaytusResponseModel<List<BaseSubscriberModel>>> DeleteSubscriberAsync(String email,
            CancellationToken cancelToken = default(CancellationToken))
        {
            return InternalPostAsync<Object, List<BaseSubscriberModel>>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "delete"),
                new { email_address = email },
                cancelToken: cancelToken);
        }
    }
}
