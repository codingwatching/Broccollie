using System.Threading;
using UnityEngine;

namespace BroCollie.SceneLoader
{
    public interface ISceneLoader
    {
        Awaitable LoadSceneAsync(string sceneName, CancellationToken cancellationToken = default);
    }
}