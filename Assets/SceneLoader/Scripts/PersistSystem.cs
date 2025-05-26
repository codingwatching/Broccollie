using BroCollie;
using System.Threading;
using UnityEngine;

public class PersistSystem : MonoBehaviour
{
    [SerializeField] private ScreenFader _screenFader;

    private EventBus _eventBus;
    private CancellationTokenSource _cancellationTokenSource = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _eventBus = new EventBus();
        _eventBus.Subscribe<SceneLoadStartEvent>(LogSceneName);
        _eventBus.Subscribe<SceneLoadCompleteEvent>(LogSceneName);

        ISceneEventPublisher sceneEventPublisher = new SceneEventAdapter(_eventBus);
        SceneLoader sceneLoader = new(sceneEventPublisher);
        ServiceLocator.Register<ISceneLoader>(sceneLoader);

        ServiceLocator.Register<IFaderAsync>(_screenFader);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SceneLoadStartEvent>(LogSceneName);
        _eventBus.Unsubscribe<SceneLoadCompleteEvent>(LogSceneName);

        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void LogSceneName(SceneLoadStartEvent e)
    {
        Debug.Log($"Scene '{e.SceneName}' started loading.");
    }

    private void LogSceneName(SceneLoadCompleteEvent e)
    {
        Debug.Log($"Scene '{e.SceneName}' finished loading.");
    }
}

public class SceneEventAdapter : ISceneEventPublisher
{
    private readonly IPublisher _publisher;

    public SceneEventAdapter(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public void Publish<T>(T sceneEvent) where T : ISceneEvent
    {
        _publisher.Publish(sceneEvent);
    }
}
