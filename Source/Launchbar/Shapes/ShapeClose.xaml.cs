using fm;
using fm.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes;

public sealed partial class ShapeClose : UserControl
{
    /// <summary>
    /// Gets or sets whether this shape is hovered.
    /// </summary>
    public bool IsHover
    {
        get { return this.GetValue<bool>(IsHoverProperty); }
        set { this.SetValueBox(IsHoverProperty, value); }
    }

    public static readonly DependencyProperty IsHoverProperty = DependencyProperty.Register(
        nameof(IsHover), typeof(bool), typeof(ShapeClose), new PropertyMetadata(Boxes.False));

    public ShapeClose()
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