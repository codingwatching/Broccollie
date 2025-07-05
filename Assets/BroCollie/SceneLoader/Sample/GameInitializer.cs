using BroCollie.SceneLoad;
using BroCollie.Util;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private ScreenFader _screenFader;
    
    private void Awake()
    {
        ServiceLocator.Register<ISceneLoader>(new SceneLoader());

        DontDestroyOnLoad(_screenFader);
        ServiceLocator.Register<IScreenFader>(_screenFader);
    }
}
