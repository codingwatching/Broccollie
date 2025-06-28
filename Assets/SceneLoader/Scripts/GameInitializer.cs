using BroCollie.SceneLoader;
using BroCollie.Utils;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private ServiceLocator _serviceLocator;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private ScreenFader _screenFader;

    private void Awake()
    {
        DontDestroyOnLoad(_sceneLoader);
        _serviceLocator.Register<ISceneLoader>(_sceneLoader);

        DontDestroyOnLoad(_screenFader);
        _serviceLocator.Register<IScreenFader>(_screenFader);
    }
}
