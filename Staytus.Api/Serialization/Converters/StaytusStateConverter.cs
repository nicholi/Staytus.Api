using System;
using Newtonsoft.Json;
using Staytus.Api.Models;

namespace Staytus.Api.Serialization.Converters
{
    public class StaytusStateConverter : JsonConverter
    {
        public const String INVESTIGATE_STATE = "investigating";
        public const String IDENTIFIED_STATE = "identified";
        public const String MONITOR_STATE = "monitoring";
        public const String RESOLVED_STATE = "resolved";
        public const String UNKNOWN_STATE = "unknown";

        public static String ToString(StaytusState state)
        {
            switch (state)
            {
                case StaytusState.Investigating:
                    return INVESTIGATE_STATE;
                case StaytusState.Identified:
                    return IDENTIFIED_STATE;
                case StaytusState.Monitoring:
                    return MONITOR_STATE;
                case StaytusState.Resolved:
                    return RESOLVED_STATE;
                default:
                    return UNKNOWN_STATE;
            }
        }

        public static StaytusState FromString(String state)
        {
            switch (state)
            {
                case INVESTIGATE_STATE:
                    return StaytusState.Investigating;
                case IDENTIFIED_STATE:
                    return StaytusState.Identified;
                case MONITOR_STATE:
                    return StaytusState.Monitoring;
                case RESOLVED_STATE:
                    return StaytusState.Resolved;
                default:
                    return StaytusState.Unknown;
            }
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            // bleh unboxing
            var enumValue = (StaytusState)value;
            var enumStrValue = ToString(enumValue);
            if (enumStrValue == UNKNOWN_STATE)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(enumStrValue);
            }
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            return FromString((String)reader.Value);
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(String);
        }
    }
}
