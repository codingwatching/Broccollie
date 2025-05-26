using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BroCollie
{
    public class SceneLoader : ISceneLoader, IDisposable
    {
        private readonly ISceneEventPublisher _eventPublisher;
        private CancellationTokenSource _cancellationTokenSource;

        public SceneLoader(ISceneEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void LoadScene(string sceneName)
        {
            _ = LoadSceneAsync(sceneName);
        }

        private async Awaitable LoadSceneAsync(string sceneName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _eventPublisher?.Publish(new SceneLoadFailEvent(sceneName, "Scene loading cancelled via token."));
                return;
            }

            _eventPublisher?.Publish(new SceneLoadStartEvent(sceneName));
            AsyncOperation loadOperation = null;
            try
            {
                loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                while (!loadOperation.isDone && !_cancellationTokenSource.IsCancellationRequested)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    _eventPublisher?.Publish(new SceneLoadProgreessEvent(sceneName, progress));
                    await Awaitable.NextFrameAsync(_cancellationTokenSource.Token);
                }
                _eventPublisher?.Publish(new SceneLoadCompleteEvent(sceneName));
            }
            catch (OperationCanceledException)
            {
                _eventPublisher?.Publish(new SceneLoadFailEvent(sceneName, "Scene loading cancelled via token."));
                Debug.LogWarning($"[SceneLoader] Scene loading cancelled via token. Target: '{sceneName}'");
            }
            catch (Exception ex)
            {
                _eventPublisher?.Publish(new SceneLoadFailEvent(sceneName, ex.Message));
                Debug.LogError($"[SceneLoader] Scene loading failed. Message: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }

    internal interface ISceneLoader
    {
        void LoadScene(string sceneName);
    }
}
