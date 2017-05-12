using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Models
{
    [DataContract]
    public class IssueModel : PartialIssueModel
    {
        [DataMember(Name = "updates")]
        public List<IssueUpdateModel> Updates { get; set; }
    }
}
