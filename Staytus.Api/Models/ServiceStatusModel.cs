using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Staytus.Api.Models
{
    [DataContract]
    public class ServiceStatusModel : BaseServiceStatusModel
    {
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
