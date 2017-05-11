using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Models
{
    [DataContract]
    public class IssueUpdateModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [JsonConverter(typeof(StaytusStateConverter))]
        [DataMember(Name = "state")]
        public StaytusState State { get; set; }

        [DataMember(Name = "text")]
        public String Text { get; set; }

        [DataMember(Name = "identifier")]
        public String Identifier { get; set; }


        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }


        [DataMember(Name = "notify")]
        public Boolean Notify { get; set; }

        [DataMember(Name = "user")]
        public BaseUserModel User { get; set; }

        [DataMember(Name = "service_status")]
        public BaseServiceStatusModel Status { get; set; }
    }
}
