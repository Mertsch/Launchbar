using fm.Extensions;
using Launchbar.Properties;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Launchbar;

internal sealed partial class WindowSettings : Window
{
    #region Properties

    /// <summary>
    /// Gets the currently selected <see cref="MenuEntry"/>.
    /// </summary>
    public MenuEntry? SelectedMenuEntry
    {
        get { return this.GetValue<MenuEntry?>(SelectedMenuEntryProperty); }
        set { this.SetValue(SelectedMenuEntryProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="SelectedMenuEntry"/> property.
    /// </summary>
    public static readonly DependencyProperty SelectedMenuEntryProperty = DependencyProperty.Register(
        nameof(SelectedMenuEntry), typeof(MenuEntry), typeof(WindowSettings));

    /// <summary>
    /// Gets whether the currently selected item is of type <see cref="Program"/>.
    /// </summary>
    public bool IsProgram
    {
        get { return this.GetValue<bool>(IsProgramProperty); }
        private set { this.SetValueBox(IsProgramProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="IsProgram"/> property.
    /// </summary>
    public static readonly DependencyProperty IsProgramProperty = DependencyProperty.Register(
        nameof(IsProgram), typeof(bool), typeof(WindowSettings));

    /// <summary>
    /// Gets the currently selected object.
    /// </summary>
    public Program? SelectedProgram
    {
        get { return this.GetValue<Program?>(SelectedProgramProperty); }
        private set { this.SetValue(SelectedProgramProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="SelectedProgram"/> property.
    /// </summary>
    public static readonly DependencyProperty SelectedProgramProperty = DependencyProperty.Register(
        nameof(SelectedProgram), typeof(Program), typeof(WindowSettings));

    /// <summary>
    /// Gets whether the currently selected item is of type <see cref="MenuEntryAdvanced"/>.
    /// </summary>
    public bool IsMenuEntryAdvanced
    {
        get { return this.GetValue<bool>(IsMenuEntryAdvancedProperty); }
        private set { this.SetValueBox(IsMenuEntryAdvancedProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="IsMenuEntryAdvanced"/> property.
    /// </summary>
    public static readonly DependencyProperty IsMenuEntryAdvancedProperty = DependencyProperty.Register(
        nameof(IsMenuEntryAdvanced), typeof(bool), typeof(WindowSettings));

    /// <summary>
    /// Gets the currently selected object.
    /// </summary>
    public MenuEntryAdvanced? SelectedMenuEntryAdvanced
    {
        get { return this.GetValue<MenuEntryAdvanced?>(SelectedMenuEntryAdvancedProperty); }
        private set { this.SetValue(SelectedMenuEntryAdvancedProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="SelectedMenuEntryAdvanced"/> property.
    /// </summary>
    public static readonly DependencyProperty SelectedMenuEntryAdvancedProperty = DependencyProperty.Register(
        nameof(SelectedMenuEntryAdvanced), typeof(MenuEntryAdvanced), typeof(WindowSettings));

    #endregion

    public WindowSettings()
    {
        this.InitializeComponent();
    }

    private void treeViewEntriesSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        object newValue = e.NewValue;

        this.SelectedMenuEntry = newValue as MenuEntry;

        MenuEntryAdvanced? mea = newValue as MenuEntryAdvanced;
        Program? program = newValue as Program;

        this.SelectedMenuEntryAdvanced = mea;
        this.IsMenuEntryAdvanced = mea is { };
        this.SelectedProgram = program;
        this.IsProgram = program is { };
    }

    private void requestNavigateHyperlink(object? sender, RequestNavigateEventArgs e)
    {
        requestNavigate(e);
    }

    private void requestNavigateCommand(object? sender, ExecutedRoutedEventArgs e)
    {
        requestNavigate(e);
    }

    private static void requestNavigate(RoutedEventArgs e)
    {
        string? uri = null;
        switch (e)
        {
            case RequestNavigateEventArgs en:
                uri = en.Uri.ToString();
                break;
            case ExecutedRoutedEventArgs er:
                uri = er.Parameter switch
                    {
                        Uri u => u.ToString(),
                        string s => s,
                        _ => uri,
                    };
                break;
            default:
                if (e.OriginalSource is Hyperlink { NavigateUri: { } navUri })
                {
                    uri = navUri.ToString();
                }
                break;
        }
        if (string.IsNullOrEmpty(uri))
        {
            return;
        }
        try
        {
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            e.Handled = true;
        }
        catch
        {
            // We do not care if this crashes
        }
    }

    #region Buttons

    private void buttonOkClick(object? sender, RoutedEventArgs? e)
    {
        this.buttonApplyClick(null, null);
        this.Close();
    }

    private void buttonCancelClick(object? sender, RoutedEventArgs? e)
    {
        Settings.Default.Reload();
        this.Close();
    }

    private void buttonApplyClick(object? sender, RoutedEventArgs? e)
    {
        // Read the Menu property to force a settings file update on save.
        if (Settings.Default.Menu == null) { }

        Settings.Default.Save();
    }

    private void buttonShutdownClick(object? sender, RoutedEventArgs? e)
    {
        Application.Current.Shutdown();
    }

    #endregion

    #region Menu modification methods

    private void buttonMoveUpClick(object sender, RoutedEventArgs e)
    {
        this.SelectedMenuEntry?.MoveUp();
    }

    private void buttonMoveDownClick(object sender, RoutedEventArgs e)
    {
        this.SelectedMenuEntry?.MoveDown();
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
        MenuEntry? selected = this.SelectedMenuEntry;
        if (selected is { })
        {
            if (selected is Submenu submenu)
            {
                submenu.MenuEntries.Add(newEntry);
            }
            else
            {
                selected.Parent?.Insert(selected.Parent.IndexOf(selected) + 1, newEntry);
            }
        }
        else
        {
            Settings.Default.Menu.Entries.Add(newEntry);
        }
    }

    private void buttonDeleteClick(object sender, RoutedEventArgs e)
    {
        this.SelectedMenuEntry?.Parent?.Remove(this.SelectedMenuEntry);
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
        this.SelectedMenuEntryAdvanced?.ChooseIcon();
    }

    private void buttonClearIconClick(object sender, RoutedEventArgs e)
    {
        this.SelectedMenuEntryAdvanced?.ClearIcon();
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
                CheckFileExists = false,
            };

        if (ofd.ShowDialog() != true)
        {
            return; // Do nothing because the user canceled the dialog.
        }

        this.SelectedProgram.Path = ofd.FileName;
    }

    private void treeViewDrop(object sender, DragEventArgs e)
    {
        string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];

        if (files.IsNullOrEmpty())
        {
            return; // Nothing to add;
        }

        MenuEntryCollection? parent = this.SelectedMenuEntry switch
            {
                null => Settings.Default.Menu.Entries,
                Submenu submenu => submenu.MenuEntries,
                    { } selectedMenu => selectedMenu.Parent,
            };
        foreach (string file in files)
        {
            parent?.Add(new Program
                {
                    Text = Path.GetFileNameWithoutExtension(file),
                    Path = file,
                });
        }
    }
}