using System.Collections.Generic;
using Newtonsoft.Json;

namespace Satisfy.Managers
{
    public class DataHandler
    {
        private readonly Dictionary<string, string> dataDictionary;
        private readonly Dictionary<string, IResettable> trackedObjects = new Dictionary<string, IResettable>();

        public DataHandler(Dictionary<string, string> initialDict)
        {
            dataDictionary = initialDict;
        }

        public void SaveData<T>(string key, T data)
        {
            if (dataDictionary.ContainsKey(key))
            {
                dataDictionary[key] = Serialize(data);
                return;
            }

            dataDictionary.Add(key, Serialize(data));
        }

        public void SaveTrackedObjects()
        {
            foreach (var data in trackedObjects)
            {
                if (dataDictionary.ContainsKey(data.Key))
                {
                    dataDictionary[data.Key] = data.Value.GetSerializedValue();
                    continue;
                }

                dataDictionary.Add(data.Key, data.Value.GetSerializedValue());
            }
        }

        public bool TryLoad<T>(string key, out T data)
        {
            if (dataDictionary.TryGetValue(key, out var value))
            {
                data = Deserialize<T>(value);
                return true;
            }

            data = default;
            return false;
        }

        public static string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static string SerializeReadable<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public void HardResetData()
        {
            foreach (var data in trackedObjects)
            {
                data.Value.ResetToDefault();

                if (dataDictionary.ContainsKey(data.Key))
                    dataDictionary[data.Key] = data.Value.GetSerializedValue();
            }

            trackedObjects.Clear();
        }

        public void Track(string key, IResettable data)
        {
            if (trackedObjects.ContainsKey(key))
                return;

            trackedObjects.Add(key, data);
        }
    }
}