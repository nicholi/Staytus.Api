using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class ServiceModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "permalink")]
        public String Permalink { get; set; }


        [DataMember(Name = "position")]
        public Int32 Position { get; set; }

        [DataMember(Name = "description")]
        public String Description { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }


        [DataMember(Name = "status")]
        public ServiceStatusModel Status { get; set; }
    }
}
