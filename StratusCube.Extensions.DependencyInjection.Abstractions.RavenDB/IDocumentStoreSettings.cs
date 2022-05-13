using Raven.Client.Documents;

namespace StratusCube.Extensions.DependencyInjection;

public interface IDocumentStoreSettings {
    /// <summary>
    /// The name of the database which will be use by an
    /// instance of <see cref="IDocumentStore"/>
    /// </summary>
    string DatabaseName { get; set; }

    /// <summary>
    /// An array of URLs which will be used by an
    /// instance of <see cref="IDocumentStore"/>
    /// </summary>
    string[] Urls { get; set; }

    /// <summary>
    /// The <see cref="IDocumentStore"/> identifier
    /// </summary>
    string Identifier { get; set; }
}
