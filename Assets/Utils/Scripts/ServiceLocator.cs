using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> s_services = new();

    public static void Register<T>(T serviceInstance)
    {
        if (serviceInstance == null) return;

        Type serviceType = typeof(T);
        s_services[serviceType] = serviceInstance;
    }

    public static void Unregister<T>()
    {
        Type serviceType = typeof(T);
        s_services.Remove(serviceType);
    }

    public static T GetService<T>()
    {
        Type serviceType = typeof(T);
        if (s_services.TryGetValue(serviceType, out object service))
        {
            return (T)service;
        }
        throw new InvalidOperationException($"[ServiceLocator] Service '{serviceType.Name}' not registered.");
    }

    public static void ClearServices()
    {
        s_services.Clear();
    }
}