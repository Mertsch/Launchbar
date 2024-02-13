using fm.Extensions;
using Launchbar.Properties;
using Launchbar.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launchbar;

public sealed partial class WindowBar : Window
{
    #region Fields

    private static bool isContextMenuOpen;

    private bool canClose;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the side of the screen this window docks to.
    /// </summary>
    public Dock Dock
    {
        get { return this.GetValue<Dock>(DockProperty); }
        private init { this.SetValue(DockProperty, value); }
    }

    public static readonly DependencyProperty DockProperty = DependencyProperty.Register(
        nameof(Dock), typeof(Dock), typeof(WindowBar));

    /// <summary>
    /// Gets the working area (desktop without task bar).
    /// </summary>
    public Area WorkArea
    {
        get { return this.GetValue<Area>(WorkAreaProperty); }
        private init { this.SetValue(WorkAreaProperty, value); }
    }

    public static readonly DependencyProperty WorkAreaProperty = DependencyProperty.Register(
        nameof(WorkArea), typeof(Area), typeof(WindowBar));

    #endregion

    /// <summary>
    /// This constructor is reserved for the WPF designer.
    /// </summary>
    public WindowBar()
    {
        this.InitializeComponent();

        this.SetAsToolWindow();
    }

    /// <summary>
    /// Create a new instance of this class.
    /// </summary>
    /// <param name="dock">Side of the work area to dock to.</param>
    /// <param name="area">Area of the screen.</param>
    public WindowBar(Dock dock, Area area) : this()
    {
        this.Dock = dock;
        this.WorkArea = area;
        area.Updated += this.areaUpdated;
        this.areaUpdated(null, EventArgs.Empty);
    }

    private void areaUpdated(object? sender, EventArgs e)
    {
        Area area = this.WorkArea;

        switch (this.Dock)
        {
            case Dock.Left:
                this.Left = area.Left - 1;
                this.Top = area.Top + 1;
                this.Width = 2;
                this.Height = area.Height - 2;
                break;
            case Dock.Top:
                this.Left = area.Left + 1;
                this.Top = area.Top - 1;
                this.Width = area.Width - 2;
                this.Height = 2;
                break;
            case Dock.Right:
                this.Left = area.Left + area.Width - 1;
                this.Top = area.Top + 1;
                this.Width = 2;
                this.Height = area.Height - 2;
                break;
            case Dock.Bottom:
                this.Left = area.Left + 1;
                this.Top = area.Top + area.Height - 1;
                this.Width = area.Width - 1;
                this.Height = 2;
                break;
        }
    }

    /// <summary>
    /// Save context menu state as the <see cref="ContextMenu.IsOpen"/> property is reset before <see cref="OnPreviewMouseLeftButtonDown"/>.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContextMenuOpening(ContextMenuEventArgs e)
    {
        isContextMenuOpen = true;
        base.OnContextMenuOpening(e);
    }

    /// <summary>
    /// Reset context menu state.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContextMenuClosing(ContextMenuEventArgs e)
    {
        isContextMenuOpen = false;
        base.OnContextMenuClosing(e);
    }

    /// <summary>
    /// Handle mouse clicks when the context menu is open.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (isContextMenuOpen)
        {
            e.Handled = true;

            if (Settings.Default.QuickLaunch)
            {
                Menu menu = Settings.Default.Menu;
                if (menu?.Entries is { Count: > 0 } menuEntries)
                {
                    if (menuEntries[0] is Program p)
                    {
                        p.TryRun();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Catch left mouse button and send it to the underlying window.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        WinHelper.SendMouseButtonUp(MouseButton.Left);
        switch (this.Dock)
        {
            case Dock.Left:
                WinHelper.SendMouseMoveRelative(1, 0);
                break;
            case Dock.Top:
                WinHelper.SendMouseMoveRelative(0, 1);
                break;
            case Dock.Right:
                WinHelper.SendMouseMoveRelative(-1, 0);
                break;
            case Dock.Bottom:
                WinHelper.SendMouseMoveRelative(0, -1);
                break;
        }
        WinHelper.SendMouseButtonDown(MouseButton.Left);
    }

    /// <summary>
    /// Takes over the ownership of the context menu.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        this.ContextMenu ??= App.RequestContextMenu();
        base.OnMouseRightButtonDown(e);
    }

    /// <summary>
    /// Prevent this object from closing unless the <see cref="Close"/> method has been called.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnClosing(CancelEventArgs e)
    {
        if (!this.canClose)
        {
            e.Cancel = true;
        }
        base.OnClosing(e);
    }

    /// <summary>
    /// Close this window.
    /// </summary>
    public new void Close()
    {
        this.canClose = true;
        base.Close();
    }
}