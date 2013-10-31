using CavemanTools;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
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
                            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                            
                        };
                    _settings.Converters.Add(new SessionIdConverter());
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

        internal class SessionIdConverter : CustomCreationConverter<SessionId>
        {
            /// <summary>
            /// Creates an object which will then be populated by the serializer.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>
            /// The created object.
            /// </returns>
            public override SessionId Create(Type objectType)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
            /// <returns>
            /// The object value.
            /// </returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var j = JObject.Load(reader);
                var b = j["Bytes"].ToObject<Byte[]>();
                return new SessionId(b);                
            }
        }


    }
}