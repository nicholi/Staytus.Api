using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace Staytus.Api.Models
{
    [DataContract]
    public class StaytusResponseModel<TData>
    {
        [DataMember(Name = "status")]
        public String Status { get; set; }

        [DataMember(Name = "time")]
        public Nullable<Double> Time { get; set; }

        [DataMember(Name = "data")]
        public TData Data { get; set; }

        [DataMember(Name = "flags")]
        public Dictionary<String, Object> Flags { get; set; }

        [DataMember(Name = "headers")]
        public Dictionary<String, Object> Headers { get; set; }

        [IgnoreDataMember]
        public String ErrorResponse { get; set; }

        public StaytusResponseModel()
        {
        }

        public StaytusResponseModel(String status, String errorResponse, TData data)
        {
            this.Status = status;
            this.ErrorResponse = errorResponse;
            this.Data = data;
        }

        public StaytusResponseModel(StaytusResponseModel<TData> other)
        {
            this.Status = other.Status;
            this.Time = other.Time;
            this.Data = other.Data;
            this.Flags = other.Flags;
            this.Headers = other.Headers;
        }
    }
}
