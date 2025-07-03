using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace BroCollie.SaveLoad
{
    public class SaveLoader<T> : ISaveLoader where T : class
    {
        public event Action OnSave;
        public event Action OnLoad;

        private T _saveData;
        private ISaveSerializer _saveSerializer;
        private SaveLoadSetting _setting;
        private string _savePath;

        public SaveLoader(T saveData, ISaveSerializer saveSerializer, SaveLoadSetting setting)
        {
            _saveData = saveData;
            _saveSerializer = saveSerializer;
            _setting = setting;
            _savePath = Path.Combine(Application.persistentDataPath, setting.SaveDirectory);
        }

        public async Task LoadDataAsync()
        {
            CreateSaveDirectoryIfNeeded(_savePath);
            if (!File.Exists(_savePath))
            {
                Debug.Log("[SaveLoader] Saved data does not exist.");
                return;
            }

            try
            {
                using FileStream fileStream = new(_savePath, FileMode.Open);
                if (_setting.UseCryptoStream)
                {
                    using Aes aes = Aes.Create();
                    byte[] iv = new byte[aes.IV.Length];
                    fileStream.Read(iv, 0, iv.Length);

                    byte[] key = await LoadKeyAsync(_setting.AesKeyName);
                    if (key == null)
                    {
                        Debug.Log("[SaveLoader] Key does not exist.");
                        return;
                    }

                    using CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                    await StreamReadAsync(cryptoStream, _saveData);
                }
                else
                {
                    await StreamReadAsync(fileStream, _saveData);
                }
                OnLoad?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log($"[SaveLoader] {e.Message}");
            }
        }

        public async Task SaveDataAsync()
        {
            CreateSaveDirectoryIfNeeded(_savePath);
            try
            {
                using Aes aes = Aes.Create();
                byte[] key = aes.Key;
                byte[] iv = aes.IV;
                await SaveKeyAsync(_setting.AesKeyName, key);

                using FileStream fileStream = new(_savePath, FileMode.Create);
                if (_setting.UseCryptoStream)
                {
                    await fileStream.WriteAsync(iv, 0, iv.Length);
                    using CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                    await StreamWriterAsync(cryptoStream, _saveData);
                }
                else
                {
                    await StreamWriterAsync(fileStream, _saveData);
                }
                OnSave?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log($"[SaveLoader] {e.Message}");
            }
        }

        private void CreateSaveDirectoryIfNeeded(string path)
        {
            if (Directory.Exists(path)) return;

            Directory.CreateDirectory(path);
            Debug.Log("[SaveLoader] Save directory created.");
        }

        private async Task StreamReadAsync(Stream stream, T data)
        {
            using StreamReader streamReader = new(stream);
            string serialized = await streamReader.ReadToEndAsync();
            _saveSerializer.Deserialize(serialized, data);
            Debug.Log("[SaveLoader] Data loaded.");
        }

        private async Task StreamWriterAsync(Stream stream, T data)
        {
            using StreamWriter streamWriter = new(stream);
            string serialized = _saveSerializer.Serialize(data);
            await streamWriter.WriteAsync(serialized);
            Debug.Log("[SaveLoader] Data saved.");
        }

        private async Task SaveKeyAsync(string keyName, byte[] key)
        {
#if UNITY_STANDALONE_WIN
            byte[] encrypted = ProtectedData.Protect(key, null, DataProtectionScope.CurrentUser);
            await File.WriteAllBytesAsync(GetKeyPath(keyName), encrypted);
#elif UNITY_STANDALONE_OSX

#else
            Debug.Log("[SaveLoader] Platform not supported. Unable to save key.");
            return;
#endif
        }

        private async Task<byte[]> LoadKeyAsync(string keyName)
        {
#if UNITY_STANDALONE_WIN
            string keyPath = GetKeyPath(keyName);
            if (!File.Exists(keyPath)) return null;

            byte[] encrypted = await File.ReadAllBytesAsync(keyPath);
            return ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
#elif UNITY_STANDALONE_OSX
            return null;
#else
            Debug.Log("[SaveLoader] Platform not supported. Unable to load key.");
            return null;
#endif
        }

        private string GetKeyPath(string keyName)
        {
            return Path.Combine(Application.persistentDataPath, keyName + ".key");
        }
    }
}
