
using System.Diagnostics.CodeAnalysis;

namespace StratusCube.Extensions.DependencyInjection;

/// <summary>
/// Contains settings for RavenDB, such as the URL to the database.
/// </summary>
public class DocumentStoreSettings : 
    IDocumentStoreSettings ,
    IEqualityComparer<IDocumentStoreSettings> ,
    IEquatable<IDocumentStoreSettings> 
{
    private string[] urls = Array.Empty<string>();

    /// <summary>
    /// The URLs where the database resides.
    /// </summary>
    public string[] Urls {
        get => urls;
        set {
            ArgumentNullException.ThrowIfNull(Urls , nameof(Urls));
            if (!value?.ToList()?.TrueForAll(u => Uri.TryCreate(u , UriKind.RelativeOrAbsolute , out _)) ?? true)
                throw new ArgumentException("Please provide valid urls" , nameof(Urls));
            urls = value ?? Array.Empty<string>();
        }
    }
    /// <summary>
    /// The name of the database.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public bool Equals(IDocumentStoreSettings? x , IDocumentStoreSettings? y) =>
        Enumerable.SequenceEqual(
            x?.Urls ?? Enumerable.Empty<string>() , y?.Urls ?? Enumerable.Empty<string>()
        ) && x?.DatabaseName == y?.DatabaseName
          && x?.Identifier == y?.Identifier;

    public bool Equals(IDocumentStoreSettings? other) =>
        Equals(this, other);

    public int GetHashCode([DisallowNull] IDocumentStoreSettings obj) =>
        urls.GetHashCode() * 
        DatabaseName.GetHashCode() * 
        Identifier.GetHashCode();
}
