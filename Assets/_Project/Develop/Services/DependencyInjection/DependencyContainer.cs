using System;
using System.Collections.Generic;

public sealed class DependencyContainer
{
    private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public void Register<T>(T instance) where T : class
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        _services[typeof(T)] = instance;
    }

    public T Resolve<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out object service))
        {
            return (T)service;
        }

        throw new InvalidOperationException($"Service not registered: {typeof(T).FullName}");
    }
}
