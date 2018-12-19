using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NLogLab
{
    public class NLogMain
    {
        public ILogger Logger { get; set; }
        public NLogMain()
        {
            var oldJsonConverter = NLog.Config.ConfigurationItemFactory.Default.JsonConverter;
            var newConverter = new JsonNetSerializer(oldJsonConverter);
            ConfigurationItemFactory.Default.JsonConverter = newConverter;
            Logger = LogManager.GetLogger("MyLogger");
        }
        public void DemoDynamicWrite(string json)
        {
            IDictionary<string, object> result = deserializeToDictionary(json);
            Logger.Info("Response {@Response}", result);
        }

        private Dictionary<string, object> deserializeToDictionary(string jo)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
            var values2 = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> d in values)
            {
                // if (d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject"))
                if (d.Value is JObject)
                {
                    values2.Add(d.Key, deserializeToDictionary(d.Value.ToString()));
                }
                else if (d.Value is JArray)
                {
                    values2.Add(d.Key, deserializeToListDictionary(d.Value.ToString()));
                }
                else
                {
                    values2.Add(d.Key, d.Value);
                }
            }
            return values2;
        }

        private List<object> deserializeToListDictionary(string json)
        {
            var values = JsonConvert.DeserializeObject<List<object>>(json);
            var values2 = new List<object>();
            foreach (object d in values)
            {
                // if (d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject"))
                if (d is JObject)
                {
                    values2.Add(deserializeToDictionary(d.ToString()));
                }
                else if (d is JArray)
                {
                    values2.Add(deserializeToListDictionary(d.ToString()));
                }
                else
                {
                    values2.Add(d);
                }
            }
            return values2;
        }
    }

}
