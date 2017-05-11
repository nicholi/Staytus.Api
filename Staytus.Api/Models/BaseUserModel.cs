using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class BaseUserModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "email_address")]
        public String Email { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }
    }
}
