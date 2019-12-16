using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Launchbar.Properties;
using Microsoft.Win32;
using WpfScreenHelper;

namespace Launchbar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#if DEBUG
        private const string MutexName = @"Global\LaunchbarSingleInstanceMutex(Debug)";
#else
        private const string MutexName = @"Global\LaunchbarSingleInstanceMutex";
#endif

        #region Fields

        private static SplashScreen splashScreen;

        // ReSharper disable NotAccessedField.Local
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static Mutex instanceMutex;
        // ReSharper restore NotAccessedField.Local

        private WindowBar barLeft;

        private WindowBar barTop;

        private WindowBar barRight;

        private WindowBar barBottom;

        private WindowBar barLeftSecondary;

        private WindowBar barTopSecondary;

        private WindowBar barRightSecondary;

        private WindowBar barBottomSecondary;

        private readonly Area primaryArea = new Area();

        private readonly Area secondaryArea = new Area();

        private int currentDisplayCount;

        private readonly ContextMenu contextMenu;

        #endregion

        static App()
        {
            splashScreen = new SplashScreen(@"Resources/SplashScreen.png");
            splashScreen.Show(false);
        }

        /// <summary>
        /// Create a new instance of this class and upgrade application settings if possible.
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand)]
        public App()
        {
            this.InitializeComponent();

            #region Make sure that only one instance of the application is running at any given time.

            instanceMutex = new Mutex(false, MutexName, out bool instantiated);
            if (!instantiated)
            {
                MessageBox.Show(Launchbar.Properties.Resources.SingleInstanceWarning,
                    Launchbar.Properties.Resources.ApplicationName, MessageBoxButton.OK,
                    MessageBoxImage.Information);
                try
                {
                    splashScreen.Close(TimeSpan.Zero);
                }
                catch (Win32Exception) { }
                this.Shutdown();
                return;
            }

            #endregion

            Settings setting = Settings.Default;
            if (setting.Menu == null)
            {
                setting.Upgrade();

                if (setting.Menu == null)
                {
                    setting.Menu = Menu.CreateDefault();
                }
            }

            setting.Menu.FillIconCacheAsync(this.Dispatcher);

            try
            {
                SystemEvents.DisplaySettingsChanged += this.displaySettingsChanged;
            }
            catch (SecurityException) { }
            this.displaySettingsChanged(null, null); // Do primary initialization.

            setting.PropertyChanged += this.settingsPropertyChanged;

            // ReSharper disable PossibleNullReferenceException
            this.contextMenu = (ContextMenu)this.FindResource("contextMenuTemplate");
            // ReSharper restore PossibleNullReferenceException
            this.forceContextMenuLayout();

            try
            {
                splashScreen.Close(new TimeSpan(TimeSpan.TicksPerSecond));
            }
            catch (Win32Exception)
            {
                splashScreen.Close(TimeSpan.Zero);
            }
            finally
            {
                splashScreen = null;
            }
        }

        #region Force context menu layout on load

        private void forceContextMenuLayout()
        {
            this.contextMenu.Visibility = Visibility.Hidden;
            this.contextMenu.Opened += this.contextMenuOpened;
            this.contextMenu.IsOpen = true;
        }

        private void contextMenuOpened(object sender, RoutedEventArgs e)
        {
            this.contextMenu.Opened -= this.contextMenuOpened;
            this.contextMenu.Closed += this.contextMenuClosed;
            this.contextMenu.IsOpen = false;
        }

        private void contextMenuClosed(object sender, RoutedEventArgs e)
        {
            this.contextMenu.Closed -= this.contextMenuClosed;
            this.contextMenu.Visibility = Visibility.Visible;
        }

        #endregion

        private void settingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BarLeft":
                    openOrCloseBar(Dock.Left, Settings.Default.BarLeft, this.primaryArea, ref this.barLeft);
                    break;
                case "BarTop":
                    openOrCloseBar(Dock.Top, Settings.Default.BarTop, this.primaryArea, ref this.barTop);
                    break;
                case "BarRight":
                    openOrCloseBar(Dock.Right, Settings.Default.BarRight, this.primaryArea, ref this.barRight);
                    break;
                case "BarBottom":
                    openOrCloseBar(Dock.Bottom, Settings.Default.BarBottom, this.primaryArea, ref this.barBottom);
                    break;
                case "BarLeftSecondary":
                    openOrCloseBar(Dock.Left, Settings.Default.BarLeftSecondary, this.secondaryArea,
                        ref this.barLeftSecondary);
                    break;
                case "BarTopSecondary":
                    openOrCloseBar(Dock.Top, Settings.Default.BarTopSecondary, this.secondaryArea,
                        ref this.barTopSecondary);
                    break;
                case "BarRightSecondary":
                    openOrCloseBar(Dock.Right, Settings.Default.BarRightSecondary, this.secondaryArea,
                        ref this.barRightSecondary);
                    break;
                case "BarBottomSecondary":
                    openOrCloseBar(Dock.Bottom, Settings.Default.BarBottomSecondary, this.secondaryArea,
                        ref this.barBottomSecondary);
                    break;
            }
        }

        private void displaySettingsChanged(object sender, EventArgs e)
        {
            ImmutableArray<Screen> screens = Screen.AllScreens.ToImmutableArray();
            int count = screens.Length;

            if (count > 0)
            {
                Rect area = screens[0].WorkingArea;
                this.primaryArea.Update(area.X, area.Y, area.Width, area.Height);

                if (this.currentDisplayCount < 1)
                {
                    openOrCloseBar(Dock.Left, Settings.Default.BarLeft, this.primaryArea, ref this.barLeft);
                    openOrCloseBar(Dock.Top, Settings.Default.BarTop, this.primaryArea, ref this.barTop);
                    openOrCloseBar(Dock.Right, Settings.Default.BarRight, this.primaryArea, ref this.barRight);
                    openOrCloseBar(Dock.Bottom, Settings.Default.BarBottom, this.primaryArea, ref this.barBottom);
                    this.currentDisplayCount = 1;
                }
            }
            else
            {
                openOrCloseBar(Dock.Left, false, null, ref this.barLeft);
                openOrCloseBar(Dock.Top, false, null, ref this.barTop);
                openOrCloseBar(Dock.Right, false, null, ref this.barRight);
                openOrCloseBar(Dock.Bottom, false, null, ref this.barBottom);
            }

            if (count > 1)
            {
                Rect area = screens[1].WorkingArea;
                this.secondaryArea.Update(area.X, area.Y, area.Width, area.Height);

                if (this.currentDisplayCount < 2)
                {
                    openOrCloseBar(Dock.Left, Settings.Default.BarLeftSecondary, this.secondaryArea,
                        ref this.barLeftSecondary);
                    openOrCloseBar(Dock.Top, Settings.Default.BarTopSecondary, this.secondaryArea,
                        ref this.barTopSecondary);
                    openOrCloseBar(Dock.Right, Settings.Default.BarRightSecondary, this.secondaryArea,
                        ref this.barRightSecondary);
                    openOrCloseBar(Dock.Bottom, Settings.Default.BarBottomSecondary, this.secondaryArea,
                        ref this.barBottomSecondary);
                    this.currentDisplayCount = 2;
                }
            }
            else
            {
                openOrCloseBar(Dock.Left, false, null, ref this.barLeftSecondary);
                openOrCloseBar(Dock.Top, false, null, ref this.barTopSecondary);
                openOrCloseBar(Dock.Right, false, null, ref this.barRightSecondary);
                openOrCloseBar(Dock.Bottom, false, null, ref this.barBottomSecondary);
            }
        }

        private static void openOrCloseBar(Dock dock, bool open, Area area, ref WindowBar windowBar)
        {
            if (windowBar == null && open && area != null && area.IsActive)
            {
                windowBar = new WindowBar(dock, area);
                windowBar.Show();
            }
            else if (windowBar != null && !open)
            {
                windowBar.Close();
                windowBar = null;
            }
        }

        public static void OpenOrActivateSettings()
        {
            WindowCollection wc = Current.Windows;
            lock (wc.SyncRoot)
            {
                foreach (Window w in wc)
                {
                    if (w is WindowSettings)
                    {
                        w.Activate();
                        return;
                    }
                }
            }
            new WindowSettings().Show();
        }

        private void contextMenuKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                OpenOrActivateSettings();
            }
        }

        public static ContextMenu RequestContextMenu()
        {
            App app = (App)Current;
            // Detach from the current parent.
            ((FrameworkElement)app.contextMenu.Parent).ContextMenu = null;
            // Attach to this object.
            return app.contextMenu;
        }
    }
}