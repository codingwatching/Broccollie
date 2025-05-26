using BroCollie;
using UnityEngine;

public class TestSceneController : MonoBehaviour
{
    [SerializeField] private string _sceneName = "MainMenu";

    private void Start()
    {
        ServiceLocator.GetService<IFaderAsync>()?.FadeAsync(0f);
    }

    public async void LoadScene()
    {
        await ServiceLocator.GetService<IFaderAsync>()?.FadeAsync(1f);
        ServiceLocator.GetService<ISceneLoader>()?.LoadScene(_sceneName);
    }
}