using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes;

/// <summary>
/// Interaction logic for ShapeDelete.xaml
/// </summary>
public sealed partial class ShapeDelete : UserControl
{
    /// <summary>
    /// Create a new instance of this class.
    /// </summary>
    public ShapeDelete()
    {
        this.InitializeComponent();
    }

    private void sizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.scaler.ScaleX = this.ActualWidth / this.shape.Width;
        this.scaler.ScaleY = this.ActualHeight / this.shape.Height;
    }
}