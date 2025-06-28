using System;
using System.Collections.Generic;
using UnityEngine;

namespace BroCollie.Utils
{
    [CreateAssetMenu(fileName = "ServiceLocator", menuName = "Utils/ServiceLocator")]
    public class ServiceLocator : ScriptableObject
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T serviceInstance)
        {
            if (serviceInstance == null) return;

            Type serviceType = typeof(T);
            _services[serviceType] = serviceInstance;
        }

        public void Unregister<T>()
        {
            Type serviceType = typeof(T);
            _services.Remove(serviceType);
        }

        public T GetService<T>()
        {
            Type serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out object service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"[ServiceLocator] Service '{serviceType.Name}' not registered.");
        }

        public void ClearServices()
        {
            _services.Clear();
        }
    }
}
