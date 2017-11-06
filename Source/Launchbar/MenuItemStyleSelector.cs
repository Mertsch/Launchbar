using System;
using System.Windows;
using System.Windows.Controls;

namespace Launchbar
{
    public class MenuItemStyleSelector : StyleSelector
    {
        public Style ProgramStyle { get; set; }

        public Style SubmenuStyle { get; set; }

        public Style SeparatorStyle { get; set; }

        public Style SettingsStyle { get; set; }

        public Style ExitStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Program)
            {
                return this.ProgramStyle;
            }
            if (item is Submenu)
            {
                return this.SubmenuStyle;
            }
            if (item is Separator)
            {
                return this.SeparatorStyle;
            }
            if (item is MenuEntrySettings)
            {
                return this.SettingsStyle;
            }
            if (item is MenuEntryExit)
            {
                return this.ExitStyle;
            }

            return null;
        }
    }
}