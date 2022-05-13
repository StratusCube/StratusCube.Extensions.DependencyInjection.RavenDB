
namespace StratusCube.Extensions.DependencyInjection;

public interface IDocumentStoreSettingsBuilder {

    /// <summary>
    /// Concats a collection of urls to the current
    /// configured urls
    /// Urls will be concatinated together
    /// <code>
    /// builder
    ///     .AddUrl("http://localhost:8080")
    ///     .AddUrls(new [] { "https://localhost:8443" }); //. <- Will concat collection with previously provided <see cref="AddUrl(string)"/>
    /// </code>
    /// </summary>
    /// <param name="urls">The collection of urls to concate to the cureent collection of urls</param>
    /// <returns>This <see cref="IDocumentStoreSettingsBuilder"/></returns>
    IDocumentStoreSettingsBuilder AddUrls(IEnumerable<string> urls);

    /// <summary>
    /// Concats a collection of urls as <see cref="Uri"/>
    /// <code>
    /// builder
    ///     .AddUrl("http://localhost:8080")
    ///     .AddUrls(new Uri[] { new Uri("https://localhost:8443") }); //. <- Will concat collection with previously provided <see cref="AddUrl(string)"/>
    /// </code>
    /// </summary>
    /// <returns>This <see cref="IDocumentStoreSettingsBuilder"/></returns>
    IDocumentStoreSettingsBuilder AddUrls(IEnumerable<Uri> urls);

    /// <summary>
    /// Concats a single url to the <see cref="IDocumentStoreSettingsBuilder"/>
    /// and can be changed together.
    /// <code>
    /// builder
    ///     .AddUrl("http://localhost:8080")
    ///     .AddUrl("https://localhost:8443");
    /// </code>
    /// </summary>
    /// <param name="url">The string in proper URL format</param>
    /// <returns>This <see cref="IDocumentStoreSettingsBuilder"/></returns>
    IDocumentStoreSettingsBuilder AddUrl(string url);

    /// <summary>
    /// Concats a url using the <see cref="Uri"/> object to the
    /// urls to be used.
    /// </summary>
    /// <param name="url">
    /// The <see cref="Uri"/> object to use to add to the list of
    /// configured urls
    /// </param>
    /// <returns>This <see cref="IDocumentStoreSettingsBuilder"/></returns>
    IDocumentStoreSettingsBuilder AddUrl(Uri url);

    /// <summary>
    /// Configures the database name to be used
    /// </summary>
    /// <param name="databaseName">
    /// The database name to be used
    /// </param>
    /// <returns>This <see cref="IDocumentStoreSettingsBuilder"/></returns>
    IDocumentStoreSettingsBuilder ConfigureDatabaseName(string databaseName);

    /// <summary>
    /// Configures and instance of <see cref="IDocumentStoreSettings"/> an action 
    /// <code>
    /// builder.ConfigureSettings(settings => {
    ///     settings.ConfigureDatabaseName("SampleDB");
    ///     //. additional settings can be configured here
    /// });
    /// </code>
    /// </summary>
    /// <param name="settings">
    /// And action which takes an input of <see cref="IDocumentStoreSettings"/>
    /// </param>
    /// <returns>This <see cref="IDocumentStoreSettingsBuilder"/></returns>
    IDocumentStoreSettingsBuilder ConfigureSettings(Action<IDocumentStoreSettings> settings);

    /// <summary>
    /// Creates a new instance of <see cref="IDocumentStoreSettings"/>
    /// from what has been configured in <see cref="IDocumentStoreSettingsBuilder"/>
    /// </summary>
    /// <returns>A new instance of <see cref="IDocumentStoreSettings"/></returns>
    IDocumentStoreSettings Build();
}
