using System;
using System.Collections.Generic;
using System.Linq;

namespace Staytus.Api.Extensions
{
    public static class ExtensionMethods
    {
        public static String ToHttpString(this IEnumerable<KeyValuePair<String, String>> collection)
        {
            if (collection == null)
            {
                return String.Empty;
            }

            // simplified version of what ToString in HttpValueCollection does
            var items = new List<String>(collection.Count());
            foreach (KeyValuePair<String, String> kvp in collection
                // so that duplicate keys are at least serialized into the querystring together
                .OrderBy(x => x.Key))
            {
                String key = kvp.Key;
                String value = kvp.Value;

                if (String.IsNullOrEmpty(key))
                {
                    continue;
                }

                String keyPrefix = Uri.EscapeDataString(key) + "=";
                if (String.IsNullOrEmpty(value))
                {
                    items.Add(keyPrefix);
                }
                else
                {
                    items.Add(String.Concat(keyPrefix, Uri.EscapeDataString(value) ?? String.Empty));
                }
            }

            return String.Join("&", items);
        }
    }
}
