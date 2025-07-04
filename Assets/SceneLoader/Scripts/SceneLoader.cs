using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BroCollie.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public event Action OnSceneLoadStart;
        public event Action<float> OnSceneLoadProgress;
        public event Action OnSceneLoadComplete;

        public async Awaitable LoadSceneAsync(string sceneName)
        {
            OnSceneLoadStart?.Invoke();
            AsyncOperation loadOperation = null;
            try
            {
                loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                while (!loadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    OnSceneLoadProgress?.Invoke(progress);

                    await Awaitable.NextFrameAsync();
                }
                OnSceneLoadComplete?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader] Scene loading failed. Message: {ex.Message}");
            }
        }
    }
}