using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Models
{
    [DataContract]
    public class BaseIssueModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "title")]
        public String Title { get; set; }

        [JsonConverter(typeof(StaytusStateConverter))]
        [DataMember(Name = "state")]
        public StaytusState State { get; set; }

        [DataMember(Name = "identifier")]
        public String Identifier { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "notify")]
        public Nullable<Boolean> Notify { get; set; }
    }
}
