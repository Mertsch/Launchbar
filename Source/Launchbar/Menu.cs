using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Launchbar;

/// <summary>
/// Dummy class.
/// This class contains the menu entries and will be saved with the application settings.
/// </summary>
public sealed class Menu : NotifyBase
{
    #region Fields

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

    [MustUseReturnValue]
    public static Menu CreateDefault()
    {
        return new Menu
            {
                entries =
                    [
                        new MenuEntrySettings(),
                        new MenuEntryExit(),
                    ],
            };
    }

    public void FillIconCacheAsync(Dispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        Task.Run(() => getIcons(this.Entries, dispatcher));
    }

    private static async Task getIcons(ObservableCollection<MenuEntry>? entries, Dispatcher dispatcher)
    {
        if (entries is null)
        {
            return; // Nothing to do
        }
        MenuEntry[] entriesa = entries.ToArray();
        for (int i = 0; i < entriesa.Length; i++)
        {
            if (entriesa[i] is MenuEntryAdvanced mea)
            {
                // We are not interested in the actual value, but getting it will fill the cache.
                await dispatcher.InvokeAsync(mea.UpdateIcon, DispatcherPriority.ApplicationIdle);

                // Also Refresh sub entries.
                if (mea is Submenu submenu)
                {
                    await getIcons(submenu.MenuEntries, dispatcher);
                }
            }
        }
    }
}