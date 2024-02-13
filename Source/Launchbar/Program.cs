using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Launchbar;

/// <summary>
/// Contains all informations to start an application
/// </summary>
public sealed class Program : MenuEntryAdvanced, ICommand
{
    #region Fields

    private string? path;

    private string? pathAbsolute;

    private string? arguments;

    private ProcessPriorityClass priority = ProcessPriorityClass.Normal;

    #endregion

    #region Properties

    /// <summary>
    /// Path to the file to start.
    /// </summary>
    public string? Path
    {
        get { return this.path; }
        set
        {
            if (value == string.Empty)
            {
                value = null;
            }
            if (this.path == value)
            {
                return;
            }
            this.path = value;
            if (value == null)
            {
                this.pathAbsolute = null;
            }
            else
            {
                this.pathAbsolute = Environment.ExpandEnvironmentVariables(value);
            }

            this.OnPropertyChanged(nameof(this.Path));
            this.OnPropertyChanged(nameof(this.IsValidFile));
            this.OnPropertyChanged(nameof(this.IsValidPath));
            this.UpdateIcon();
        }
    }

    /// <summary>
    /// Absolute path to the file to start (resolves environment variables).
    /// </summary>
    public string? PathAbsolute => this.pathAbsolute;

    /// <summary>
    /// Arguments to pass when starting the file.
    /// </summary>
    public string? Arguments
    {
        get { return this.arguments; }
        set
        {
            if (value == string.Empty)
            {
                value = null;
            }
            if (this.arguments == value)
            {
                return;
            }
            this.arguments = value;
            this.OnPropertyChanged(nameof(this.Arguments));
        }
    }

    /// <summary>
    /// Priority to start the program with.
    /// </summary>
    public ProcessPriorityClass Priority
    {
        get { return this.priority; }
        set
        {
            if (this.priority == value)
            {
                return;
            }
            this.priority = value;
            this.OnPropertyChanged(nameof(this.Priority));
        }
    }

    /// <summary>
    /// Does the specified path point to an existing file?
    /// </summary>
    public bool IsValidFile => File.Exists(this.pathAbsolute);

    /// <summary>
    /// Does the specified path point to an existing directory?
    /// </summary>
    public bool IsValidPath => Directory.Exists(this.pathAbsolute);

    #endregion

    /// <summary>
    /// Create a new instance of this class.
    /// </summary>
    public Program()
    {
        this.Text = "Program";
    }

    /// <summary>
    /// Run the specified application with its arguments, working directory and priority
    /// </summary>
    public void Run()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = this.pathAbsolute,
                Arguments = this.arguments
            };

        string? workingDir = System.IO.Path.GetDirectoryName(this.pathAbsolute);
        if (workingDir is { })
        {
            startInfo.WorkingDirectory = workingDir;
        }

        if (Process.Start(startInfo) is { } process) // Start the process and set the priority (if a new process has been created).
        {
            if (this.Priority is not ProcessPriorityClass.Normal) // Only set priority if not default (as setting priority may cause an exception).
            {
                process.PriorityClass = this.Priority;
            }
        }
    }

    /// <summary>
    /// Calls <see cref="Run"/> and display <see cref="MessageBox"/> if there is a failure.
    /// </summary>
    public void TryRun()
    {
        try
        {
            this.Run();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occured while trying to start the application:\n\n{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

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
        this.TryRun();
    }
}