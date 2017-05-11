using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Extensions.Logging;

namespace Staytus.Api.Tests.TestFixtures
{
    public abstract class BaseServiceTests
    {
        private static readonly ILoggerFactory LoggerFactory;

        protected static readonly IConfigurationRoot Configuration;
        protected static readonly StaytusApiClient ApiClient;

        static BaseServiceTests()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddNLog();

            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddJsonFile("config.production.json", optional: true, reloadOnChange: true);

            Configuration = configBuilder.Build();

            ApiClient = new StaytusApiClient(
                Configuration.GetValue<String>("staytusApi:url", null),
                Configuration.GetValue<String>("staytusApi:token", null),
                Configuration.GetValue<String>("staytusApi:secret", null),
                LoggerFactory.CreateLogger(typeof(StaytusApiClient)));
        }

        protected ILogger Logger { get; }

        internal BaseServiceTests()
        {
            this.Logger = LoggerFactory.CreateLogger(this.GetType());
        }

        protected bool CompareObjectsFromJsonObject<TItem1, TItem2>(TItem1 item1, TItem2 item2)
        {
            var item1Json = JToken.FromObject(item1);
            var item2Json = JToken.FromObject(item2);

            return JToken.DeepEquals(item1Json, item2Json);
        }

        protected bool CompareObjectsWithJsonSerialization<TItem1, TItem2>(TItem1 item1, TItem2 item2)
        {
            var item1Json = (JToken)
                JsonConvert.DeserializeObject(
                    JsonConvert.SerializeObject(item1));
            var item2Json = (JToken)
                JsonConvert.DeserializeObject(
                    JsonConvert.SerializeObject(item2));

            return JToken.DeepEquals(item1Json, item2Json);
        }

        protected const String UPPERCASE_LATIN_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        protected const String LOWERCASE_LATIN_ALPHABET = "abcdefghijklmnopqrstuvwxyz";
        protected const String ARABIC_NUMERALS = "0123456789";
        protected static readonly char[] DEFAULT_CHARSET = (UPPERCASE_LATIN_ALPHABET + LOWERCASE_LATIN_ALPHABET + ARABIC_NUMERALS).ToCharArray();

        protected static readonly Random RNG = new Random();

        public static String NextString(int length)
        {
            return NextString(length, DEFAULT_CHARSET);
        }

        public static String NextString(int length, char[] charset)
        {
            const int charSize = sizeof(UInt32);
            int dataSize = length * charSize;
            byte[] secureData = new byte[dataSize];
            RNG.NextBytes(secureData);

            var sb = new StringBuilder(length);
            for (int i = 0; i < dataSize; i += charSize)
            {
                int charSetIndex = Convert.ToInt32(BitConverter.ToUInt32(secureData, i) % charset.Length);
                sb.Append(charset[charSetIndex]);
            }

            return sb.ToString();
        }
    }
}
