namespace Launchbar;

public static class Design
{
    public static Program Program => new Program() { Text = "DesignProgram" };
    public static Separator Separator => new Separator();
    public static MenuEntry MenuEntry => new Submenu();
    public static Submenu Submenu => new Submenu();
    public static MenuEntrySettings Settings => new MenuEntrySettings();
    public static MenuEntryExit Exit => new MenuEntryExit();
}