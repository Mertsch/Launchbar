using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Launchbar
{
    public static class MetaData
    {
        [NotNull]
        public static Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}