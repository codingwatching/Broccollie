using System;
using UnityEngine;

namespace BroCollie.SceneLoad
{
    public interface ISceneLoader
    {
        event Action OnSceneLoadStart;
        event Action<float> OnSceneLoadProgress;
        event Action OnSceneLoadComplete;

        Awaitable LoadSceneAsync(string sceneName);
    }
}