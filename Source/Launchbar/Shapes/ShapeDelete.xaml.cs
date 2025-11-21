using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes;

public sealed partial class ShapeDelete : UserControl
{
    public ShapeDelete()
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