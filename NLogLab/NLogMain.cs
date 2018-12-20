using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Collections.Generic;

namespace NLogLab
{
    public class NLogMain
    {
        public ILogger Logger { get; set; }
        public NLogMain()
        {
            Logger = LogManager.GetLogger("MyLogger");
        }
        public void DemoDynamicWrite(string json)
        {
            IDictionary<string, object> result = DeserializeToObjectDictionary(json);
            Logger.Info("Response {@Response}", result);
        }

        private Dictionary<string, object> DeserializeToObjectDictionary(string json)
        {
            var dictNode = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> pair in
                 JsonConvert.DeserializeObject<Dictionary<string, object>>(json))
            {
                dictNode.Add(pair.Key, ParseJsonObject(pair.Value));
            }
            return dictNode;
        }

        private object ParseJsonObject(object jsonObejct)
        {
            if (jsonObejct is JObject)
            {
                return DeserializeToObjectDictionary(jsonObejct.ToString());

            }
            else if (jsonObejct is JArray)
            {
                return DeserializeToObjectList(jsonObejct.ToString());
            }
            else
            {
                return jsonObejct;
            }
        }

        private List<object> DeserializeToObjectList(string json)
        {
            var listNode = new List<object>();
            foreach (object node in
                JsonConvert.DeserializeObject<List<object>>(json))
            {
                listNode.Add(ParseJsonObject(node));
            }
            return listNode;
        }
    }

}
