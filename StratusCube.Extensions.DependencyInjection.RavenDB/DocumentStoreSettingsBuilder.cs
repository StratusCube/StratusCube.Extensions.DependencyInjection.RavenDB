
namespace StratusCube.Extensions.DependencyInjection;

/// <summary>
/// A fluent builder to produce an instance of <see cref="IDocumentStoreSettings"/>
/// </summary>
public class DocumentStoreSettingsBuilder : IDocumentStoreSettingsBuilder {

    IDocumentStoreSettings Settings { get; set; }
        = new DocumentStoreSettings();

    public IDocumentStoreSettingsBuilder AddUrl(string url) {
        Settings.Urls = Settings.Urls.Concat(new[] { url })
            .ToArray();
        return this;
    }

    public IDocumentStoreSettingsBuilder AddUrl(Uri url) =>
        AddUrl(url.ToString());

    public IDocumentStoreSettings Build() =>
        Settings;

    public IDocumentStoreSettingsBuilder ConfigureDatabaseName(string databaseName) {
        Settings.DatabaseName = databaseName;
        return this;
    }

    public IDocumentStoreSettingsBuilder AddUrls(IEnumerable<string> urls) {
        Settings.Urls = Settings.Urls.Concat(urls).ToArray();
        return this;
    }

    public IDocumentStoreSettingsBuilder AddUrls(IEnumerable<Uri> urls) =>
        AddUrls(urls.Select(u => u.ToString()));

    public IDocumentStoreSettingsBuilder ConfigureSettings(
        Action<IDocumentStoreSettings> settings
    ) {
        settings(Settings);
        return this;
    }
}

