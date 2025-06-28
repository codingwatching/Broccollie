using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BroCollie.SceneLoader
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        public event Action OnSceneLoadStart;
        public event Action<float> OnSceneLoadProgress;
        public event Action OnSceneLoadComplete;

        public async Awaitable LoadSceneAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = destroyCancellationToken;

            OnSceneLoadStart?.Invoke();
            AsyncOperation loadOperation = null;
            try
            {
                loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                while (!loadOperation.isDone)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    OnSceneLoadProgress?.Invoke(progress);
                    await Awaitable.NextFrameAsync(cancellationToken);
                }
                OnSceneLoadComplete?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[SceneLoader] Scene loading canceled.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader] Scene loading failed. Message: {ex.Message}");
            }
        }
    }
}