using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Launchbar;

public static class MetaData
{
    [NotNull]
    private static readonly Lazy<string> version = new Lazy<string>(() =>
        Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unknown");

    [NotNull]
    public static string Version => version.Value;
}