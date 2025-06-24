using System.Threading;
using UnityEngine;

public class PersistSystem : MonoBehaviour
{
    [SerializeField] private ScreenFader _screenFader;

    private CancellationTokenSource _cancellationTokenSource = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ServiceLocator.Register(new SceneLoader());
        ServiceLocator.Register(_screenFader);
    }

    private void OnDestroy()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}
