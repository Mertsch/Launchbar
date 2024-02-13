// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable CS0414 // Field is assigned but its value is never used
namespace Launchbar.Win32;

internal static class GWL
{
    public static readonly int GWL_WNDPROC = -4;
    public static readonly int GWL_HINSTANCE = -6;
    public static readonly int GWL_HWNDPARENT = -8;
    public static readonly int GWL_STYLE = -16;
    public static readonly int GWL_EXSTYLE = -20;
    public static readonly int GWL_USERDATA = -21;
    public static readonly int GWL_ID = -12;
}