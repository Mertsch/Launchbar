using System.Reflection;

namespace Launchbar;

public static class MetaData
{
    private static readonly Lazy<string> version = new Lazy<string>(() =>
        Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unknown");

    public static string Version => version.Value;
}