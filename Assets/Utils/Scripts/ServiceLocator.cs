using System;
using System.Collections.Generic;

namespace BroCollie
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T serviceInstance)
        {
            if (serviceInstance == null) return;

            Type serviceType = typeof(T);
            _services[serviceType] = serviceInstance;
        }

        public static void Unregister<T>()
        {
            Type serviceType = typeof(T);
            _services.Remove(serviceType);
        }

        public static T GetService<T>()
        {
            Type serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out object service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"[ServiceLocator] Service '{serviceType.Name}' not registered.");
        }

        public static void ClearServices()
        {
            _services.Clear();
        }
    }
}