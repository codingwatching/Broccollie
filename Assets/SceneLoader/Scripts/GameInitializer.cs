using BroCollie.SceneLoader;
using BroCollie.Utils;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private ScreenFader _screenFader;

    private void Awake()
    {
        DontDestroyOnLoad(_sceneLoader);
        ServiceLocator.Register<ISceneLoader>(_sceneLoader);

        DontDestroyOnLoad(_screenFader);
        ServiceLocator.Register<IScreenFader>(_screenFader);
    }
}
