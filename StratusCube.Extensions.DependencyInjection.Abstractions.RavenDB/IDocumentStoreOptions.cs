using Raven.Client.Documents.Conventions;
using Raven.Client.Documents;
using System.Security.Cryptography.X509Certificates;

namespace StratusCube.Extensions.DependencyInjection;

public interface IDocumentStoreOptions {
    /// <summary>
    /// The certificate to be used by an instance of <see cref="IDocumentStore"/>
    /// </summary>
    X509Certificate2? Certificate { get; set; }

    /// <summary>
    /// The settings instance to be used by an instance of <see cref="IDocumentStore"/>
    /// </summary>
    IDocumentStoreSettings Settings { get; set; }

    /// <summary>
    /// The <see cref="DocumentConventions"/> conventions to be used by an instance of
    /// <see cref="IDocumentStore"/>
    /// </summary>
    DocumentConventions? Conventions { get; set; }
}
