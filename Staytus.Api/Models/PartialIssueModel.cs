using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class PartialIssueModel : BaseIssueModel
    {
        [DataMember(Name = "user")]
        public BaseUserModel User { get; set; }

        [DataMember(Name = "service_status")]
        public BaseServiceStatusModel Status { get; set; }
    }
}
