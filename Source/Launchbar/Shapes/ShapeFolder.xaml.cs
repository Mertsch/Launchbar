using fm;
using fm.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes;

public sealed partial class ShapeFolder : UserControl
{
    /// <summary>
    /// Gets or sets whether the folder is open.
    /// </summary>
    public bool IsOpen
    {
        get { return this.GetValue<bool>(IsOpenProperty); }
        set { this.SetValueBox(IsOpenProperty, value); }
    }

    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        nameof(IsOpen), typeof(bool), typeof(ShapeFolder), new PropertyMetadata(Boxes.False));

    public ShapeFolder()
    {
        this.InitializeComponent();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        this.scaler.ScaleX = this.ActualWidth / this.shape.Width;
        this.scaler.ScaleY = this.ActualHeight / this.shape.Height;
    }
}