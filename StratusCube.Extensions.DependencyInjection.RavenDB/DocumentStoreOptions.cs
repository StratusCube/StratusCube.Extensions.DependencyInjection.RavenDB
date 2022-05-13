using Raven.Client.Documents.Conventions;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace StratusCube.Extensions.DependencyInjection;

public class DocumentStoreOptions : 
    IDocumentStoreOptions , 
    IEqualityComparer<IDocumentStoreOptions>,
    IEquatable<IDocumentStoreOptions> 
{
    public IDocumentStoreSettings Settings { get; set; } = new DocumentStoreSettings();
    public X509Certificate2? Certificate { get; set; }
    public DocumentConventions? Conventions { get; set; } = new();

    public bool Equals(IDocumentStoreOptions? x , IDocumentStoreOptions? y) =>
        (x?.Settings.Equals(y?.Settings) ?? false)
            && x?.Certificate?.GetCertHashString() == y?.Certificate?.GetCertHashString()
            && (x?.Conventions?.Equals(y?.Conventions) ?? false);

    public bool Equals(IDocumentStoreOptions? other) =>
        Equals(this, other);

    public int GetHashCode([DisallowNull] IDocumentStoreOptions obj) =>
        12 * (obj.Certificate?.GetHashCode() ?? 1) *
        (obj.Conventions?.GetHashCode() ?? 2) +
        obj.Settings.GetHashCode();
}
