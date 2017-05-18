using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Staytus.Api.Models;
using Staytus.Api.Serialization.Converters;

namespace Staytus.Api.Extensions
{
    public static class JsonNetExtensions
    {
        private static readonly JsonSerializerSettings s_JsonSerializerSettings;

        private static readonly JsonSerializer s_JsonSerializer;

        static JsonNetExtensions()
        {
            s_JsonSerializerSettings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.None,

                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                };

            s_JsonSerializer = JsonSerializer.CreateDefault(s_JsonSerializerSettings);
        }

        public static String SerializeObject(this Object value)
        {
            var stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture);
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = s_JsonSerializer.Formatting;
                s_JsonSerializer.Serialize(jsonTextWriter, value, null);
            }
            return stringWriter.ToString();
        }

        public static TObject DeserializeObject<TObject>(this String value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            /*
            if (!jsonSerializer.IsCheckAdditionalContentSet())
                jsonSerializer.CheckAdditionalContent = true;
            */
            using (var jsonTextReader = new JsonTextReader(new StringReader(value)))
            {
                return s_JsonSerializer.Deserialize<TObject>(jsonTextReader);
            }
        }

        public static String ToWireValue(this StaytusState state)
        {
            return StaytusStateConverter.ToString(state);
        }

        public static StaytusState FromWireValue(this String stateStr)
        {
            return StaytusStateConverter.FromString(stateStr);
        }
    }
}
