using System;

public static class AppServices
{
    public static DependencyContainer Container { get; private set; }

    public static bool IsInitialized => Container != null;

    public static void Initialize(DependencyContainer container)
    {
        if (container == null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (Container != null)
        {
            throw new InvalidOperationException("AppServices is already initialized.");
        }

        Container = container;
    }

    public static T Resolve<T>() where T : class
    {
        if (Container == null)
        {
            throw new InvalidOperationException("AppServices is not initialized.");
        }

        return Container.Resolve<T>();
    }
}
