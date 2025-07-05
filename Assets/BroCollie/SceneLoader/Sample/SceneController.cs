using BroCollie.SceneLoad;
using BroCollie.Util;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string _sceneName = "MainMenu";

    private void Start()
    {
        ServiceLocator.GetService<IScreenFader>()?.FadeAsync(0);
    }

    public async void LoadScene()
    {
        await ServiceLocator.GetService<IScreenFader>()?.FadeAsync(1);
        await ServiceLocator.GetService<ISceneLoader>()?.LoadSceneAsync(_sceneName);
    }
}