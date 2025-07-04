using BroCollie.SaveLoad;
using TMPro;
using UnityEngine;

public class SaveLoadTester : MonoBehaviour
{
    [SerializeField] private SaveData _saveData;
    [SerializeField] private SaveLoadSetting _saveLoadSetting;
    [SerializeField] private TextMeshProUGUI _text;
    private SaveLoader<SaveData> _saveLoader;

    private void Awake()
    {
        ISaveSerializer saveSerializer = new JsonSaveDataSerializer();
        _saveLoader = new SaveLoader<SaveData>(_saveData, saveSerializer, _saveLoadSetting);
    }

    [ContextMenu("Save")]
    public async void Save()
    {
        await _saveLoader.SaveDataAsync();
    }

    [ContextMenu("Load")]
    public async void Load()
    {
        await _saveLoader.LoadDataAsync();
        _text.text = _saveData.PlayerName;
    }
}
