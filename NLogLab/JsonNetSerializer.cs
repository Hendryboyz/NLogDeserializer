using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Text;

namespace NLogLab
{
    internal class JsonNetSerializer : NLog.IJsonConverter
    {
        private NLog.IJsonConverter _originalConverter;
        readonly JsonSerializerSettings _settings;

        public JsonNetSerializer(NLog.IJsonConverter originalConverter)
        {
            _originalConverter = originalConverter;
            _settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        /// <summary>Serialization of an object into JSON format.</summary>
        /// <param name="value">The object to serialize to JSON.</param>
        /// <param name="builder">Output destination.</param>
        /// <returns>Serialize succeeded (true/false)</returns>
        public bool SerializeObject(object value, StringBuilder builder)
        {
            if (value is DynamicObject)
            {
                try
                {
                    var jsonSerializer = JsonSerializer.CreateDefault(_settings);
                    var sw = new System.IO.StringWriter(builder, System.Globalization.CultureInfo.InvariantCulture);
                    using (var jsonWriter = new JsonTextWriter(sw))
                    {
                        jsonWriter.Formatting = jsonSerializer.Formatting;
                        jsonSerializer.Serialize(jsonWriter, value, null);
                    }
                }
                catch (Exception e)
                {
                    NLog.Common.InternalLogger.Error(e, "Error when custom JSON serialization");
                    return false;
                }
                return true;
            }

            return _originalConverter.SerializeObject(value, builder);
        }
    }
}
