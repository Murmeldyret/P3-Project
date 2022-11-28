using Microsoft.Extensions.DependencyInjection;

namespace zenref.Core.Services;

public sealed class ServiceProvider
{
    private readonly Microsoft.Extensions.DependencyInjection.ServiceProvider _serviceProvider;
    
    public ServiceProvider()
    {
        ServiceCollection serviceCollection = new();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(typeof(IReferencesService), typeof(ReferencesService));
    }

    // public void ConfigureServices(IServiceCollection services)
    // {
    //     services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    //         
    //     services.AddTransient<IReferencesRepository, ReferencesRepository>();
    //     services.AddTransient<IReferencesService, ReferencesService>();
    // }
    
    
    public object GetService(Type serviceType)
    {
        if (serviceType is null) throw new ArgumentException("", nameof(serviceType));
        return _serviceProvider.GetService(serviceType) ?? throw new NotSupportedException(serviceType.Name);
    }

    public T GetService<T>()
    {
        return _serviceProvider.GetService<T>() ?? throw new NotSupportedException(typeof(T).Name);
    }
}