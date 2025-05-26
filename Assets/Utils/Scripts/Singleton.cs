﻿using UnityEngine;

namespace BroCollie
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
                    s_instance = (T)FindFirstObjectByType(typeof(T));
                    if (s_instance == null)
                    {
                        GameObject singletoneObject = new(typeof(T).ToString());
                        s_instance = singletoneObject.AddComponent<T>();
                    }
                }
                return s_instance;
            }
        }
    }
}