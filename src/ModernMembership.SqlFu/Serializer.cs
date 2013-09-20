using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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
            var tp = typeof (T);
            var rez = JsonConvert.DeserializeObject(data, Settings);
            if (tp.IsArray)
            {
                return rez.As<JArray>().ToObject<T>();
            }
            return (T)rez;
        }

     
      
    }
}