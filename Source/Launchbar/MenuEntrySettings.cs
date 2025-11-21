using System.Windows.Input;

namespace Launchbar;

/// <summary>
/// This class represents a settings entry.
/// </summary>
public sealed class MenuEntrySettings : MenuEntry, ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        App.OpenOrActivateSettings(); // Quit
    }
}