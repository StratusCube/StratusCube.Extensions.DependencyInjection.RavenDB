
using Raven.Client.Documents;
using StratusCube.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {

    public static IServiceCollection AddDocumentStore(
        this IServiceCollection services ,
        IDocumentStoreOptions options ,
        Action<IDocumentStore>? preInitialization = null ,
        Action<IDocumentStore>? postInitialization = null
    ) =>
        services.AddDocumentStore(
            _ => _ = options ,
            preInitialization ,
            postInitialization
        );
    public static IServiceCollection AddDocumentStore(
        this IServiceCollection services ,
        Action<IDocumentStoreOptions> configure ,
        Action<IDocumentStore>? preInitialization = null ,
        Action<IDocumentStore>? postInitialization = null
    ) {
        var options = new DocumentStoreOptions();
        configure(options);
        services.AddDocumentStore(
            _ => _.Configure(options) ,
            preInitialization ,
            postInitialization
        );

        return services;
    }

    public static IServiceCollection AddDocumentStore(
        this IServiceCollection services ,
        Func<IDocumentStoreBuilder , IDocumentStoreBuilder> configure ,
        Action<IDocumentStore>? preInitialization = null ,
        Action<IDocumentStore>? postInitialization = null
    ) {
        var optionsBuilder = new DocumentStoreBuilder();
        configure?.Invoke(optionsBuilder);
        services.Configure<IDocumentStoreOptions>(_ => _ = optionsBuilder.Options);
        var documentStore = optionsBuilder.Build();

        services.AddSingleton(sp => {
            preInitialization?.Invoke(documentStore);
            documentStore.Initialize();
            postInitialization?.Invoke(documentStore);
            return documentStore;
        });

        return services;
    }

    public static IServiceCollection AddAsyncDocumentSession(
        this IServiceCollection services
    ) => services.AddScoped(
        sp => {
            var docStore = sp.GetRequiredService<IDocumentStore>();
            return docStore.OpenAsyncSession();
        });

    public static IServiceCollection AddAsyncDocumentSession(
        this IServiceCollection services ,
        IDocumentStore documentStore
    ) => services.AddScoped(
        sp => documentStore.OpenAsyncSession());

    public static IServiceCollection AddDocumentSession(
        this IServiceCollection services
    ) => services.AddScoped(
        sp => {
            var docStore = sp.GetRequiredService<IDocumentStore>();
            return docStore.OpenSession();
        });

    public static IServiceCollection AddDocumentSession(
        this IServiceCollection services ,
        IDocumentStore documentStore
    ) => services.AddScoped(
        sp => documentStore.OpenSession());
}
