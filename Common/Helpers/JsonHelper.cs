namespace Common
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    public static class JsonHelper
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T Deserialize<T>(this object value)
        {
            return value.ToJson().DeserializeJson<T>();
        }

        public static bool IsPropertyNull(this JObject jObject, string propertyName)
        {
            return jObject[propertyName].IsNullOrEmpty();
        }

        public static bool IsPropertyExists(this JToken jToken)
        {
            return jToken != null;
        }

        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString()?.Length == 0) ||
                   (token.Type == JTokenType.Null);
        }

        public static bool WhenPropertyOrValueNotNull(this JObject document, string propertyName)
        {
            return !document.IsPropertyNull(propertyName) && !document[propertyName].IsNullOrEmpty();
        }

        public static bool IsEmpty(this JObject keyValues)
        {
            var isEmpty = true;
            foreach (var keyValue in keyValues)
            {
                if (!keyValue.Value.IsNullOrEmpty())
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }
    }
}
