using BroCollie.SceneLoader;
using BroCollie.Utils;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private ServiceLocator _serviceLocator;
    [SerializeField] private string _sceneName = "MainMenu";

    private void Start()
    {
        _serviceLocator.GetService<IScreenFader>()?.FadeAsync(0);
    }

    public async void LoadScene()
    {
        await _serviceLocator.GetService<IScreenFader>()?.FadeAsync(1);
        await _serviceLocator.GetService<ISceneLoader>()?.LoadSceneAsync(_sceneName);
    }
}