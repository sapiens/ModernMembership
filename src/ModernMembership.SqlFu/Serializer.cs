using Newtonsoft.Json;

namespace ModernMembership.SqlFu
{
    internal static class Serializer
    {
        private static JsonSerializerSettings _settings;
        internal static JsonSerializerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new JsonSerializerSettings()
                        {

                            TypeNameHandling =
                                TypeNameHandling.Objects,
                            PreserveReferencesHandling =
                                PreserveReferencesHandling.Arrays,
                            DateTimeZoneHandling = DateTimeZoneHandling.Utc
                        };
                }
                return _settings;
            }
        }

        internal static string Serialize<T>(this T data)
        {
            var rez = JsonConvert.SerializeObject(data, Settings);
            return rez;
        }

        internal static T Deserialize<T>(this string data)
        {
            return (T)JsonConvert.DeserializeObject(data, Settings);
        }

     
      
    }
}