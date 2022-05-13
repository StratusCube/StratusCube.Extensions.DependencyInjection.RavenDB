using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using System.Security.Cryptography.X509Certificates;

namespace StratusCube.Extensions.DependencyInjection;

/// <summary>
/// A fluent builder which produces an <see cref="IDocumentStore"/> 
/// when <see cref="Build"/> is called
/// </summary>
public interface IDocumentStoreBuilder {

    /// <summary>
    /// Adds an x509 certificate to <see cref="IDocumentStore.Certificate"/>
    /// from a <see cref="byte"/>[] and certificate password
    /// <code>
    /// var bytes = File.ReadAllBytes("./mycert.pfx");
    /// builder.AddCertificate(bytes, password);
    /// </code>
    /// </summary>
    /// <param name="certBytes">
    /// Bytes representing the certificate
    /// </param>
    /// <param name="password">
    /// The certificate password
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder AddCertificate(byte[] certBytes , string password);

    /// <summary>
    /// Configures an <see cref="X509Certificate2"/> certificate from a function
    /// <code>
    /// builder.AddCertificate(() => new <see cref="X509Certificate2"/>() {
    ///     //. hydrations here
    /// })
    /// </code>
    /// </summary>
    /// <param name="cert">
    /// A function that has no input which produces an <see cref="X509Certificate2"/>
    /// as the output
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder AddCertificate(Func<X509Certificate2> cert);

    /// <summary>
    /// Adds a certificate from the certificate specified thumbprint and <see cref="StoreLocation"/>
    /// <code>
    /// builder.AddCertificate(thumbprint, <see cref="StoreLocation.CurrentUser"/>);
    /// </code>
    /// </summary>
    /// <param name="thumprint">The thumbprint of the certificate to use</param>
    /// <param name="location">Specify which location to use of the certificate store</param>
    /// <param name="insecure">Specifies whether or not to use an insecure/self-signed certificate</param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder AddCertificate(string thumprint , StoreLocation location = StoreLocation.CurrentUser , bool insecure = true);

    /// <summary>
    /// Adds a certificate to the <see cref="IDocumentStore.Certificate"/>
    /// using from a file and a password
    /// <code>
    /// builder.AddCertificate("./path/to/my-cert.pfx", password);
    /// </code>
    /// </summary>
    /// <param name="filePath">
    /// The file path to the certificate
    /// </param>
    /// <param name="password">
    /// The certificate password to use
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/> </returns>
    IDocumentStoreBuilder AddCertificate(string filePath , string password);

    /// <summary>
    /// Configures/adds an <see cref="X509Certificate2"/> instance to
    /// <see cref="IDocumentStoreOptions.Certificate"/>
    /// <code>
    /// builder.AddCertificate(new <see cref="X509Certificate2"/> {
    ///     //. hydrations here
    /// });
    /// </code>
    /// </summary>
    /// <param name="cert">
    /// The certificate to use for <see cref="IDocumentStore.Certificate"/>
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder AddCertificate(X509Certificate2 cert);

    /// <summary>
    /// Configures the conventions for <see cref="IDocumentStore"/>
    /// <code>
    /// builder.ConfigureConventions(conventions => {
    ///     conventions.FindCollectionNameForDynamic = type =>
    ///         "@empty";
    /// });
    /// </code>
    /// </summary>
    /// <param name="options">
    /// The action which takes an input of <see cref="DocumentConventions"/>
    /// and allows the setting of variaus <see cref="IDocumentStore.Conventions"/>
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder ConfigureConventions(Action<DocumentConventions> options);

    /// <summary>
    /// Configures the instance of <see cref="IDocumentStoreOptions"/>
    /// though an action.
    /// <code>
    /// builder.Configure(options => {
    ///     options.Settings = new <see cref="DocumentStoreSettings"/>();
    ///     //. additional options can be instanciated here
    /// });
    /// </code>
    /// </summary>
    /// <param name="options">The action which passes an instance of the builder's options to be configured</param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder Configure(Action<IDocumentStoreOptions> options);

    /// <summary>
    /// Configures the document store options with an instance of
    /// <see cref="IDocumentStoreOptions"/>
    /// <code>
    /// builder.Configure(new DocumentStoreOptions {
    ///     //. hydrations here
    /// });
    /// </code>
    /// </summary>
    /// <param name="options">
    /// The options to use for the builder. There is no need for
    /// additional functions.
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder Configure(IDocumentStoreOptions options);

    /// <summary>
    /// Configures the <see cref="IDocumentStoreOptions.Settings"/> instance
    /// to use a passed in instance of <see cref="IDocumentStoreSettings"/>
    /// <code>
    /// builder.ConfigureSettings(new <see cref="DocumentStoreSettings"/>{
    ///     //. hydrations here
    /// });
    /// </code>
    /// </summary>
    /// <param name="settings">
    /// An <see cref="IDocumentStoreSettings"/> instance which
    /// will be used to configure the <see cref="IDocumentStoreOptions.Settings"/>
    /// instance
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder ConfigureSettings(IDocumentStoreSettings settings);

    /// <summary>
    /// Configures the <see cref="IDocumentStoreOptions.Settings"/> instance
    /// though an action.
    /// <code>
    /// builder.ConfigureSettings(settings => {
    ///     settings.DatabaseName = "SampleDB";
    ///     //. configure more <see cref="IDocumentStoreSettings"/> fields here
    /// });
    /// </code>
    /// </summary>
    /// <param name="settings"></param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder ConfigureSettings(Action<IDocumentStoreSettings> settings);

    /// <summary>
    /// Configures settings from an insance of <see cref="IConfiguration"/>
    /// <code>
    /// var config = new <see cref="IConfigurationBuilder"/>()
    ///     .AddJsonFile("appsettings.json").Build();
    ///     
    /// builder.ConfigureSettings(config);
    /// </code>
    /// </summary>
    /// <param name="configuration">
    /// The insance of <see cref="IConfiguration"/> 
    /// to be used to configure the <see cref="IDocumentStoreSettings"/>
    /// of the this.<see cref="IDocumentStoreOptions"/>
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder ConfigureSettings(IConfiguration configuration);

    /// <summary>
    /// Configure settings using the <see cref="IDocumentStoreSettingsBuilder"/>
    /// There is no need to call <see cref="IDocumentStoreSettingsBuilder.Build"/>
    /// <code>
    /// builder.ConfigureSettings(settingsBuilder => 
    ///     settingsBuilder.AddDatabaseName("SampleDB")
    /// );
    /// </code>
    /// </summary>
    /// <param name="settingsBuilder">
    /// An action which take an <see cref="IDocumentStoreSettingsBuilder"/> 
    /// to production the instance of <see cref="IDocumentStoreOptions.Settings"/>
    /// </param>
    /// <returns>This <see cref="IDocumentStoreBuilder"/></returns>
    IDocumentStoreBuilder ConfigureSettings(Action<IDocumentStoreSettingsBuilder> settingsBuilder);

    /// <summary>
    /// Produces an <see cref="IDocumentStore"/> from the <see cref="IDocumentStoreOptions"/>
    /// configuration
    /// <code>
    /// var documentStore = builder.Build();
    /// </code>
    /// </summary>
    /// <returns>
    /// A new instance of an <see cref="IDocumentStore"/> 
    /// using the build configuration
    /// </returns>
    IDocumentStore Build();
}
