using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations;

namespace Launchbar
{
    /// <summary>
    /// Contains all informations to start an application
    /// </summary>
    public sealed class Program : MenuEntryAdvanced, ICommand
    {
        #region Fields

        private string path;

        private string pathAbsolute;

        private string arguments;

        private ProcessPriorityClass priority = ProcessPriorityClass.Normal;

        #endregion

        #region Properties

        /// <summary>
        /// Path to the file to start.
        /// </summary>
        public string Path
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
        public string PathAbsolute => this.pathAbsolute;

        /// <summary>
        /// Arguments to pass when starting the file.
        /// </summary>
        public String Arguments
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

        #region Reserved for future use

        ///// <summary>
        ///// Get or set a working directory to be used when starting the program.
        ///// </summary>
        //public string UserWorkingDir { get; set; }

        ///// <summary>
        ///// Has the user chosen a 
        ///// </summary>
        //public bool HasUserWorkingDir
        //{
        //    get { return !string.IsNullOrEmpty(this.UserWorkingDir); }
        //}

        ///// <summary>
        ///// Directory to start in.
        ///// </summary>
        //public String WorkingDir
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(this.UserWorkingDir))
        //        {
        //            return this.UserWorkingDir;
        //        }
        //        try
        //        {
        //            return System.IO.Path.GetDirectoryName(this.Path);
        //        }
        //        catch (ArgumentException) {}
        //        return null;
        //    }
        //}

        #endregion

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
        public Boolean IsValidFile => File.Exists(this.pathAbsolute);

        /// <summary>
        /// Does the specified path point to an existing directory?
        /// </summary>
        public Boolean IsValidPath => Directory.Exists(this.pathAbsolute);

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
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public void Run()
        {
            Process process = new Process();
            process.StartInfo.FileName = this.pathAbsolute;
            process.StartInfo.Arguments = this.arguments;

            string workingDir = System.IO.Path.GetDirectoryName(this.pathAbsolute);
            if (workingDir != null)
            {
                process.StartInfo.WorkingDirectory = workingDir;
            }

            if (process.Start()) // Start the process and set the priority (if a new process has been created).
            {
                if (this.Priority != ProcessPriorityClass.Normal) // Only set priority if not default (as setting priority may cause an exception).
                {
                    process.PriorityClass = this.Priority;
                }
            }
        }

        /// <summary>
        /// Calls <see cref="Run"/> and display <see cref="MessageBox"/> if there is a failure.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
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

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute([CanBeNull] object parameter)
        {
            return true;
        }

        public void Execute([CanBeNull] object parameter)
        {
            this.TryRun();
        }
    }
}