using UnityEngine;

namespace BroCollie.SaveLoad
{
    [CreateAssetMenu(fileName = "SaveLoadSetting", menuName = "BroCollie/SaveLoad/SaveLoadSetting")]
    public class SaveLoadSetting : ScriptableObject
    {
        public string SaveDirectory;
        public string SaveFileName;
        public string AesKeyName;
        public bool UseCryptoStream;
    }
}
