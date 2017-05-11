using System;
using System.Runtime.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class UserModel : BaseUserModel
    {
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime Updatedat { get; set; }
    }
}
