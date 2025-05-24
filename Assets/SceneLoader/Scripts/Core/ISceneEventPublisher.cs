using UnityEngine;

namespace BroCollie
{
    public interface ISceneEventPublisher
    {
        void Publish<T>(T sceneEvent) where T : ISceneEvent;
    }

    public interface ISceneEvent { }

    public class SceneLoadStartEvent : ISceneEvent
    {
        public string SceneName;

        public SceneLoadStartEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }

    public class SceneLoadProgreessEvent : ISceneEvent
    {
        public string SceneName;
        public float Progress;

        public SceneLoadProgreessEvent(string sceneName, float progress)
        {
            SceneName = sceneName;
            Progress = progress;
        }
    }

    public class SceneLoadCompleteEvent : ISceneEvent
    {
        public string SceneName;

        public SceneLoadCompleteEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }

    public class SceneLoadFailEvent : ISceneEvent
    {
        public string SceneName;
        public string ErrorMessage;

        public SceneLoadFailEvent(string sceneName, string errorMessage)
        {
            SceneName = sceneName;
            ErrorMessage = errorMessage;
        }
    }
}
