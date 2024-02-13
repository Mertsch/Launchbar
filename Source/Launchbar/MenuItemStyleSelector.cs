using System.Windows;
using System.Windows.Controls;

namespace Launchbar;

public sealed class MenuItemStyleSelector : StyleSelector
{
    public Style? ProgramStyle { get; set; }

    public Style? SubmenuStyle { get; set; }

    public Style? SeparatorStyle { get; set; }

    public Style? SettingsStyle { get; set; }

    public Style? ExitStyle { get; set; }

    [MustUseReturnValue]
    public override Style? SelectStyle(object? item, DependencyObject? container)
    {
        return item switch
            {
                Program => this.ProgramStyle,
                Submenu => this.SubmenuStyle,
                Separator => this.SeparatorStyle,
                MenuEntrySettings => this.SettingsStyle,
                MenuEntryExit => this.ExitStyle,
                _ => null,
            };
    }
}