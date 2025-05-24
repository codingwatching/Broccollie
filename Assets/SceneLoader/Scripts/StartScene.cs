using BroCollie;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";

    private SceneLoader _sceneLoader;

    private void Start()
    {
        EventBus eventBus = new();
        ISceneEventPublisher sceneEventPublisher = new SceneEventAdapter(eventBus);
        _sceneLoader = new SceneLoader(sceneEventPublisher);
        ServiceLocator.Register<ISceneLoader>(_sceneLoader);
    }

    public void LoadScene()
    {
        _sceneLoader.LoadScene(sceneName);
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
