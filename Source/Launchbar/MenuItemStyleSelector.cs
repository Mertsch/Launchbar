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
            switch (item)
            {
                case Program _:
                    return this.ProgramStyle;
                case Submenu _:
                    return this.SubmenuStyle;
                case Separator _:
                    return this.SeparatorStyle;
                case MenuEntrySettings _:
                    return this.SettingsStyle;
                case MenuEntryExit _:
                    return this.ExitStyle;
                default:
                    return null;
            }
        }
    }
}