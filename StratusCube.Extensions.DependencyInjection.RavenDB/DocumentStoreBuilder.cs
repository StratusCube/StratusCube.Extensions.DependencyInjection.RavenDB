using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;

namespace StratusCube.Extensions.DependencyInjection;

/// <summary>
/// A fluent builder which produces an <see cref="IDocumentStore"/> 
/// when <see cref="Build"/> is called
/// </summary>
public class DocumentStoreBuilder : IDocumentStoreBuilder {

    /// <summary>
    /// The <see cref="IDocumentStoreOptions" /> instance
    /// for the fluent builder to reference. Can be used
    /// to reference the options at any time before
    /// <see cref="Build"/> is called.
    /// </summary>
    public IDocumentStoreOptions Options { get; private set; } =
        new DocumentStoreOptions();

    public IDocumentStoreBuilder Configure(IDocumentStoreOptions options) {
        Options = options;
        return this;
    }

    public IDocumentStoreBuilder Configure(Action<IDocumentStoreOptions> options) {
        ArgumentNullException.ThrowIfNull(options , nameof(options));
        options(Options);
        return this;
    }

    public IDocumentStoreBuilder ConfigureSettings(
            IConfiguration configuration
        ) {
        Options.Settings = configuration.Get<DocumentStoreSettings>() ?? new();
        return this;
    }

    public IDocumentStoreBuilder ConfigureSettings(IDocumentStoreSettings settings) {
        Options.Settings = settings;
        return this;
    }

    public IDocumentStoreBuilder ConfigureSettings(Action<IDocumentStoreSettings> settings) {
        settings(Options.Settings);
        return this;
    }

    public IDocumentStoreBuilder ConfigureSettings(Action<IDocumentStoreSettingsBuilder> settingsBuilder) {
        ArgumentNullException.ThrowIfNull(settingsBuilder , nameof(settingsBuilder));
        var builder = new DocumentStoreSettingsBuilder();
        settingsBuilder(builder);
        return this.ConfigureSettings(builder.Build());
    }

    public IDocumentStoreBuilder ConfigureConventions(Action<DocumentConventions> options) {
        var conventions = new DocumentConventions();
        options?.Invoke(conventions);
        Options.Conventions = conventions;
        return this;
    }

    public IDocumentStoreBuilder AddCertificate(X509Certificate2 cert) {
        ArgumentNullException.ThrowIfNull(cert , nameof(cert));
        Options.Certificate = cert;
        return this;
    }

    public IDocumentStoreBuilder AddCertificate(Func<X509Certificate2> cert) {
        ArgumentNullException.ThrowIfNull(cert , nameof(cert));
        Options.Certificate = new(cert());
        return this;
    }

    public IDocumentStoreBuilder AddCertificate(byte[] certBytes , string password) {
        ArgumentNullException.ThrowIfNull(certBytes , nameof(certBytes));
        ArgumentNullException.ThrowIfNull(password , nameof(password));

        var ss = password.ToSecureString();
        ss.MakeReadOnly();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        password = null; //. hopefully garbage collection gets to this
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        return this.AddCertificate(new X509Certificate2(certBytes , ss));
    }

    public IDocumentStoreBuilder AddCertificate(
        string filePath , string password
    ) {
        ArgumentNullException.ThrowIfNull(filePath , nameof(filePath));
        ArgumentNullException.ThrowIfNull(password , nameof(password));


        if (File.Exists(filePath)) {
            var cert = this.AddCertificate(File.ReadAllBytes(filePath) , password);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            password = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            return cert;
        }
        else {
            throw new FileNotFoundException(
                "Could not locate certificate file" , filePath);
        }
    }

    [SupportedOSPlatform("windows")]
    public IDocumentStoreBuilder AddCertificate(
        string thumprint ,
        StoreLocation location = StoreLocation.CurrentUser ,
        bool insecure = true
    ) {
        var store = new X509Store(location);
        X509Certificate2? certificate;
        try {
            store.Open(OpenFlags.ReadOnly);
            var collection = store.Certificates.Find(
                X509FindType.FindByThumbprint , thumprint , !insecure);

            if (!collection?.Any() ?? true)
                throw new ArgumentException(
                    $"Could not find certificat with thumprint {thumprint}" , nameof(thumprint));

#pragma warning disable CS8604 // Possible null reference argument.
            certificate = collection.First();
#pragma warning restore CS8604 // Possible null reference argument.
        }
        catch (Exception ex) {
            throw new Exception(
                "Error while retrieving certificate. See inner exception for details" , ex);
        }
        finally {
            store.Close();
        }

        return this.AddCertificate(certificate);
    }

    public IDocumentStore Build() {

        var settings = Options.Settings;

        if (string.IsNullOrEmpty(Options.Settings?.DatabaseName))
            throw new InvalidOperationException($"No database name has been configured");
        if (!Options.Settings.Urls.Any())
            throw new InvalidOperationException(
                $"No urls have been configured has been configured"
            );

        var docStore = new DocumentStore {
            Database = settings.DatabaseName ,
            Urls = settings.Urls ,
            Conventions = Options.Conventions
        };

        if (!string.IsNullOrEmpty(settings.Identifier))
            docStore.Identifier = settings.Identifier;

        if (Options.Certificate is not null)
            docStore.Certificate = Options.Certificate;

        return docStore;
    }
}
