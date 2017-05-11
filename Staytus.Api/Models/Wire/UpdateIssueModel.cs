using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Models.Wire
{
    [DataContract]
    public class UpdateIssueModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "text")]
        public String Text { get; set; }

        [JsonConverter(typeof(StaytusStateConverter))]
        [DataMember(Name = "state")]
        public StaytusState ServiceState { get; set; }

        [DataMember(Name = "status")]
        public String StatusPermalink { get; set; }

        [DataMember(Name = "notify")]
        public Nullable<Boolean> Notify { get; set; }

        public UpdateIssueModel()
        {
        }

        public UpdateIssueModel(Int32 id, String text, StaytusState serviceState, 
            String statusPermalink = null, Nullable<Boolean> notify = null)
        {
            this.Id = id;
            this.Text = text;
            this.ServiceState = serviceState;
            this.StatusPermalink = statusPermalink;
            this.Notify = notify;
        }
    }
}
