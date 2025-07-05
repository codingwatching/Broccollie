using UnityEngine;

namespace BroCollie.Util
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance = null;

        public static T Instance
        {
            get
            {
                if ((object)s_instance == null)
                {
                    s_instance = FindAnyObjectByType<T>();
                    if (s_instance == null)
                    {
                        s_instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }
                return s_instance;
            }
        }
    }
}
