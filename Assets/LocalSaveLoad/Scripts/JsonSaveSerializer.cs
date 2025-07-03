using UnityEngine;

namespace BroCollie.SaveLoad
{
    public class JsonSaveDataSerializer : ISaveSerializer
    {
        public string Serialize(object data)
        {
            return JsonUtility.ToJson(data);
        }
        
        public void Deserialize(string serializedData, object target)
        {
            JsonUtility.FromJsonOverwrite(serializedData, target);
        }
    }
}