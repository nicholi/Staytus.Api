using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class VerifiedEmailModel
    {
        [DataMember(Name = "message_id")]
        public String MessageId { get; set; }

        [DataMember(Name = "subject")]
        public String Subject { get; set; }

        [DataMember(Name = "body")]
        public String Body { get; set; }
    }
}
