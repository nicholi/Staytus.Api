using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Models
{
    [DataContract]
    public class IssueModel : BaseIssueModel
    {
        [DataMember(Name = "user")]
        public BaseUserModel User { get; set; }

        [DataMember(Name = "service_status")]
        public BaseServiceStatusModel Status { get; set; }

        [DataMember(Name = "updates")]
        public List<IssueUpdateModel> Updates { get; set; }
    }
}
