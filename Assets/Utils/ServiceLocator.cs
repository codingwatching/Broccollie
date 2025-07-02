using System;
using System.Collections.Generic;
using UnityEngine;

namespace BroCollie.Utils
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> s_services = new();

        public static void Register<T>(T service)
        {
            if (service == null)
            {
                Debug.LogError($"[ServiceLocator] Cannot register a null service. Type: {typeof(T).Name}.");
                return;
            }

            if (s_services.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"[ServiceLocator] Overriding service. Type: {typeof(T).Name}");
            }

            s_services.Add(typeof(T), service);
        }

        public static void Unregister<T>()
        {
            if (!s_services.Remove(typeof(T)))
            {
                Debug.LogWarning($"[ServiceLocator] Service is not registered. Type: {typeof(T).Name}");
            }
        }

        public static T GetService<T>()
        {
            if (s_services.TryGetValue(typeof(T), out object service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"[ServiceLocator] Service is not registered. Type: {typeof(T).Name}");
        }

        public static bool IsServiceRegistered<T>()
        {
            return s_services.ContainsKey(typeof(T));
        }

        public static void ClearServices()
        {
            s_services.Clear();
            Debug.Log("[ServiceLocator] All services cleared.");
        }
    }
}
