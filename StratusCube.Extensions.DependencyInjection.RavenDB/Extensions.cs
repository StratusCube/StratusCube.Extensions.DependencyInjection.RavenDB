using System.Security;

namespace StratusCube.Extensions.DependencyInjection;

internal static class Extensions {
    public static SecureString ToSecureString(this string s) {
        SecureString ss = new();
        s.ToList().ForEach(c => ss.AppendChar(c));
        return ss;
    }
}
