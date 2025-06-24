using UnityEngine;

public class TestSceneController : MonoBehaviour
{
    [SerializeField] private string _sceneName = "MainMenu";

    private void Start()
    {
        ServiceLocator.GetService<ScreenFader>()?.FadeOutAsync();
    }

    public async void LoadScene()
    {
        await ServiceLocator.GetService<ScreenFader>()?.FadeInAsync();
        ServiceLocator.GetService<SceneLoader>()?.LoadScene(_sceneName);
    }
}