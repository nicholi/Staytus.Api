using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class SubscriberModel : BaseSubscriberModel
    {
        [DataMember(Name = "verification_token")]
        public String VerificationToken { get; set; }

        [DataMember(Name = "verified_at")]
        public Nullable<DateTime> VerifiedAt { get; set; }


        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
