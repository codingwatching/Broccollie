using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : IDisposable
{
    public event Action OnSceneLoadStart;
    public event Action<float> OnSceneLoadProgress;
    public event Action OnSceneLoadComplete;
    public event Action<string> OnSceneLoadFail;

    private CancellationTokenSource _cancellationTokenSource;

    public SceneLoader()
    {
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
            OnSceneLoadFail?.Invoke($"Scene loading cancelled via token. Target: '{sceneName}'");
            return;
        }

        OnSceneLoadStart?.Invoke();
        AsyncOperation loadOperation = null;
        try
        {
            loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!loadOperation.isDone && !_cancellationTokenSource.IsCancellationRequested)
            {
                float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                OnSceneLoadProgress?.Invoke(progress);
                await Awaitable.NextFrameAsync(_cancellationTokenSource.Token);
            }
            OnSceneLoadComplete?.Invoke();
        }
        catch (Exception ex)
        {
            OnSceneLoadFail?.Invoke(ex.Message);
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
