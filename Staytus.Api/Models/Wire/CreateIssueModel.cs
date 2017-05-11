using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Models.Wire
{
    [DataContract]
    public class CreateIssueModel
    {
        [DataMember(Name = "title")]
        public String Title { get; set; }

        [DataMember(Name = "services")]
        public List<String> ServicePermalinks { get; set; }

        [DataMember(Name = "status")]
        public String StatusPermalink { get; set; }

        [JsonConverter(typeof(StaytusStateConverter))]
        [DataMember(Name = "state")]
        public StaytusState ServiceState { get; set; }

        [DataMember(Name = "initial_update")]
        public String InitialUpdate { get; set; }

        [DataMember(Name = "notify")]
        public Nullable<Boolean> Notify { get; set; }

        public CreateIssueModel()
        {
        }

        public CreateIssueModel(String title, List<String> servicePermalinks, String statusPermalink, StaytusState serviceState,
            String initialUpdate = null,Nullable<Boolean> notify = null)
        {
            this.Title = title;
            this.ServicePermalinks = new List<String>(servicePermalinks);
            this.StatusPermalink = statusPermalink;
            this.ServiceState = serviceState;
            this.InitialUpdate = initialUpdate;
            this.Notify = notify;
        }
    }
}
