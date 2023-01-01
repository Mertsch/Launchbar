using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes;

/// <summary>
/// Interaction logic for ShapeNext.xaml
/// </summary>
public sealed partial class ShapeNext : UserControl
{
    public ShapeNext()
    {
        this.InitializeComponent();
    }

    private void sizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.scaler.ScaleX = this.ActualWidth / this.shape.Width;
        this.scaler.ScaleY = this.ActualHeight / this.shape.Height;
    }
}