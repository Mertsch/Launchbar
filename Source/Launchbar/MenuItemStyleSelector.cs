using System;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace Launchbar
{
    public sealed class MenuItemStyleSelector : StyleSelector
    {
        [CanBeNull]
        public Style ProgramStyle { get; set; }

        [CanBeNull]
        public Style SubmenuStyle { get; set; }

        [CanBeNull]
        public Style SeparatorStyle { get; set; }

        [CanBeNull]
        public Style SettingsStyle { get; set; }

        [CanBeNull]
        public Style ExitStyle { get; set; }

        [CanBeNull, MustUseReturnValue]
        public override Style SelectStyle([CanBeNull] object item, [CanBeNull] DependencyObject container)
        {
            return item switch
                {
                    Program => this.ProgramStyle,
                    Submenu => this.SubmenuStyle,
                    Separator => this.SeparatorStyle,
                    MenuEntrySettings => this.SettingsStyle,
                    MenuEntryExit => this.ExitStyle,
                    _ => null
                };
        }
    }
}