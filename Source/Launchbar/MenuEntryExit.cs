using System;
using System.Windows;
using System.Windows.Input;

namespace Launchbar;

/// <summary>
/// This class represents an exit entry.
/// </summary>
public sealed class MenuEntryExit : MenuEntry, ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        Application.Current.Shutdown(); // Quit
    }
}