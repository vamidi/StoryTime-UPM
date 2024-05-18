using Newtonsoft.Json.Linq;

namespace StoryTime.Domains.Extensions.JSON
{
    public static class JsonUtility
    {
        public static bool TryGet<T>(this JObject jObject, string key, out T value)
        {
            if (jObject.ContainsKey(key))
            {
                value = jObject[key].Value<T>();
                return true;
            }

            value = default;
            return false;
        }
    }
}