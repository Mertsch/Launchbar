using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace Launchbar
{
    /// <summary>
    /// Dummy class.
    /// This class contains the menu entries and will be saved with the application settings.
    /// </summary>
    public class Menu : NotifyBase
    {
        #region Fields

        private readonly object lockThread = new object();

        private Thread threadCacheIcons;

        private MenuEntryCollection entries;

        #endregion

        /// <summary>
        /// Gets or sets a list of menu entries.
        /// </summary>
        public MenuEntryCollection Entries
        {
            get { return this.entries; }
            set
            {
                if (this.entries == value)
                {
                    return;
                }
                this.entries = value;
                this.OnPropertyChanged(nameof(this.Entries));
            }
        }

        public static Menu CreateDefault()
        {
            return new Menu
            {
                entries = new MenuEntryCollection
                {
                    new MenuEntrySettings(),
                    new MenuEntryExit(),
                }
            };
        }

        public void FillIconCacheAsync(Dispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            lock (this.lockThread)
            {
                if (this.threadCacheIcons != null && this.threadCacheIcons.IsAlive)
                {
                    this.threadCacheIcons.Abort();
                    this.threadCacheIcons.Join();
                }
                this.threadCacheIcons = new Thread(new ThreadStart(() => getIcons(this.Entries, dispatcher)));
                this.threadCacheIcons.Start();
            }
        }

        private static void getIcons(ObservableCollection<MenuEntry> entries, Dispatcher dispatcher)
        {
            if (entries == null)
            {
                return; // Nothing to do
            }
            MenuEntry[] entriesa = entries.ToArray();
            for (int i = 0; i < entriesa.Length; i++)
            {
                MenuEntryAdvanced mea = entriesa[i] as MenuEntryAdvanced;
                if (mea != null)
                {
                    // We are not interested in the actual value, but getting it will fill the cache.
                    dispatcher.BeginInvoke(new Action(mea.UpdateIcon), DispatcherPriority.ApplicationIdle, null);

                    // Also Refresh sub entries.
                    Submenu submenu = mea as Submenu;
                    if (submenu != null)
                    {
                        getIcons(submenu.MenuEntries, dispatcher);
                    }
                }
            }
        }
    }
}