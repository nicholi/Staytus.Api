using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class BaseSubscriberModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "email_address")]
        public String Email { get; set; }
    }
}
