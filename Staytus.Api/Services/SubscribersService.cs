using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Staytus.Api.Models;

namespace Staytus.Api
{
    public partial class StaytusApiClient
    {
        protected const String SUBSCRIBERS_SERVICE = "subscribers";

        public Task<StaytusResponseModel<SubscriberModel>> GetSubscriberByEmail(String email)
        {
            return InternalPostAsync<Object, SubscriberModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "info"),
                new { email_address = email });
        }

        public Task<StaytusResponseModel<SubscriberModel>> GetSubscriberByToken(String verificationToken)
        {
            return InternalPostAsync<Object, SubscriberModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "info"),
                new { verification_token = verificationToken });
        }

        public Task<StaytusResponseModel<SubscriberModel>> AddSubscriber(String email, Boolean verified = false)
        {
            return InternalPostAsync<Object, SubscriberModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "add"),
                new { email_address = email, verified = verified ? 1 : 0 });
        }

        public Task<StaytusResponseModel<Boolean>> VerifySubscriber(String email)
        {
            return InternalPostAsync<Object, Boolean>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "verify"),
                new { email_address = email });
        }

        public Task<StaytusResponseModel<VerifiedEmailModel>> SendSubscriberVerification(String email)
        {
            return InternalPostAsync<Object, VerifiedEmailModel>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "send_verification_email"),
                new { email_address = email });
        }

        public Task<StaytusResponseModel<List<BaseSubscriberModel>>> DeleteSubscriber(String email)
        {
            return InternalPostAsync<Object, List<BaseSubscriberModel>>(GetServiceMethodPath(SUBSCRIBERS_SERVICE, "delete"),
                new { email_address = email });
        }
    }
}
