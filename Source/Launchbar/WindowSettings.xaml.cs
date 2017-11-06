using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using JetBrains.Annotations;
using Launchbar.Properties;
using Microsoft.Win32;

namespace Launchbar
{
    internal sealed partial class WindowSettings : Window
    {
        #region Properties

        /// <summary>
        /// Gets the currently selected <see cref="MenuEntry"/>.
        /// </summary>
        public MenuEntry SelectedMenuEntry
        {
            get { return (MenuEntry)this.GetValue(SelectedMenuEntryProperty); }
            set { this.SetValue(SelectedMenuEntryProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedMenuEntry"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedMenuEntryProperty = DependencyProperty.Register(
            @"SelectedMenuEntry", typeof(MenuEntry), typeof(WindowSettings));

        /// <summary>
        /// Gets whether the currently selected item is of type <see cref="Program"/>.
        /// </summary>
        public bool IsProgram
        {
            get { return (bool)this.GetValue(IsProgramProperty); }
            private set { this.SetValue(IsProgramProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsProgram"/> property.
        /// </summary>
        public static readonly DependencyProperty IsProgramProperty = DependencyProperty.Register(
            @"IsProgram", typeof(bool), typeof(WindowSettings));

        /// <summary>
        /// Gets the currently selected object.
        /// </summary>
        public Program SelectedProgram
        {
            get { return (Program)this.GetValue(SelectedProgramProperty); }
            private set { this.SetValue(SelectedProgramProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedProgram"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedProgramProperty = DependencyProperty.Register(
            @"SelectedProgram", typeof(Program), typeof(WindowSettings));

        /// <summary>
        /// Gets whether the currently selected item is of type <see cref="MenuEntryAdvanced"/>.
        /// </summary>
        public bool IsMenuEntryAdvanced
        {
            get { return (bool)this.GetValue(IsMenuEntryAdvancedProperty); }
            private set { this.SetValue(IsMenuEntryAdvancedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsMenuEntryAdvanced"/> property.
        /// </summary>
        public static readonly DependencyProperty IsMenuEntryAdvancedProperty = DependencyProperty.Register(
            @"IsMenuEntryAdvanced", typeof(bool), typeof(WindowSettings));

        /// <summary>
        /// Gets the currently selected object.
        /// </summary>
        public MenuEntryAdvanced SelectedMenuEntryAdvanced
        {
            get { return (MenuEntryAdvanced)this.GetValue(SelectedMenuEntryAdvancedProperty); }
            private set { this.SetValue(SelectedMenuEntryAdvancedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedMenuEntryAdvanced"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedMenuEntryAdvancedProperty = DependencyProperty.Register(
            @"SelectedMenuEntryAdvanced", typeof(MenuEntryAdvanced), typeof(WindowSettings));

        #endregion

        public WindowSettings()
        {
            this.InitializeComponent();
        }

        private void treeViewEntriesSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object newValue = e.NewValue;

            this.SelectedMenuEntry = newValue as MenuEntry;

            MenuEntryAdvanced mea = newValue as MenuEntryAdvanced;
            Program program = newValue as Program;

            this.SelectedMenuEntryAdvanced = mea;
            this.IsMenuEntryAdvanced = mea != null;
            this.SelectedProgram = program;
            this.IsProgram = program != null;
        }

        private void hyperlinkRequestNavigate([CanBeNull] object sender, [NotNull] RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        #region Buttons

        private void buttonOkClick(object sender, RoutedEventArgs e)
        {
            this.buttonApplyClick(null, null);
            this.Close();
        }

        private void buttonCancelClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
            this.Close();
        }

        private void buttonApplyClick(object sender, RoutedEventArgs e)
        {
            // Read the Menu property to force a settings file update on save.
            if (Settings.Default.Menu == null) {}

            Settings.Default.Save();
        }

        private void buttonShutdownClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Menu modification methods

        private void buttonMoveUpClick(object sender, RoutedEventArgs e)
        {
            this.SelectedMenuEntry.MoveUp();
        }

        private void buttonMoveDownClick(object sender, RoutedEventArgs e)
        {
            this.SelectedMenuEntry.MoveDown();
        }

        private void buttonAddProgramClick(object sender, RoutedEventArgs e)
        {
            this.addMenuEntry(new Program());
            this.selectTextTextBox();
        }

        private void buttonAddSubmenuClick(object sender, RoutedEventArgs e)
        {
            this.addMenuEntry(new Submenu());
            this.selectTextTextBox();
        }

        private void buttonAddSeparatorClick(object sender, RoutedEventArgs e)
        {
            this.addMenuEntry(new Separator());
        }

        private void buttonAddSettingsClick(object sender, RoutedEventArgs e)
        {
            this.addMenuEntry(new MenuEntrySettings());
        }

        private void buttonAddExitClick(object sender, RoutedEventArgs e)
        {
            this.addMenuEntry(new MenuEntryExit());
        }

        private void addMenuEntry(MenuEntry newEntry)
        {
            MenuEntry selected = this.SelectedMenuEntry;
            if (selected != null)
            {
                Submenu submenu = selected as Submenu;
                if (submenu != null)
                {
                    if (submenu.MenuEntries == null)
                    {
                        submenu.MenuEntries = new MenuEntryCollection();
                    }
                    submenu.MenuEntries.Add(newEntry);
                }
                else
                {
                    selected.Parent.Insert(selected.Parent.IndexOf(selected) + 1, newEntry);
                }
            }
            else
            {
                Settings.Default.Menu.Entries.Add(newEntry);
            }
        }

        private void buttonDeleteClick(object sender, RoutedEventArgs e)
        {
            if (this.SelectedMenuEntry != null)
            {
                this.SelectedMenuEntry.Parent.Remove(this.SelectedMenuEntry);
            }
        }

        private void selectTextTextBox()
        {
            this.textBoxName.SelectAll();
            this.textBoxName.Focus();
        }

        #endregion

        #region Menu entry modification methods

        private void buttonChooseIconClick(object sender, RoutedEventArgs e)
        {
            this.SelectedMenuEntryAdvanced.ChooseIcon();
        }

        private void buttonClearIconClick(object sender, RoutedEventArgs e)
        {
            this.SelectedMenuEntryAdvanced.ClearIcon();
        }

        #endregion

        private void buttonBrowseClick(object sender, RoutedEventArgs e)
        {
            if (this.SelectedProgram == null)
            {
                return; // Not possible.
            }

            OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = @"Executables (*.exe)|*.exe|All files (*.*)|*.*",
                    FilterIndex = 1,
                    // Text to be displayed in the file textbox.
                    FileName = "Use this directory.",
                    // Do not check for valid filename because we also want to accept directories.
                    CheckFileExists = false
                };

            if (ofd.ShowDialog() != true)
            {
                return; // Do nothing because the user canceled the dialog.
            }

            this.SelectedProgram.Path = ofd.FileName;
        }

        private void treeViewDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files == null || files.Length == 0)
            {
                return; // Nothing to add;
            }

            MenuEntry selected = this.SelectedMenuEntry;
            MenuEntryCollection parent;
            if (selected == null)
            {
                parent = Settings.Default.Menu.Entries;
            }
            else
            {
                Submenu submenu = selected as Submenu;
                if (submenu == null)
                {
                    parent = selected.Parent;
                }
                else
                {
                    parent = submenu.MenuEntries;
                }
            }
            foreach (string file in files)
            {
                parent.Add(new Program
                    {
                        Text = Path.GetFileNameWithoutExtension(file),
                        Path = file
                    });
            }
        }
    }
}