using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Staytus.Api.Models
{
    [DataContract]
    public class BaseServiceStatusModel
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "permalink")]
        public String Permalink { get; set; }

        [DataMember(Name = "color")]
        public String Color { get; set; }
    }
}
