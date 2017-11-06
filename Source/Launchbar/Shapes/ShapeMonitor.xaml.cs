using System;
using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes
{
    /// <summary>
    /// Interaction logic for ShapeMonitor.xaml
    /// </summary>
    public sealed partial class ShapeMonitor : UserControl
    {
        public ShapeMonitor()
        {
            this.InitializeComponent();
        }

        private void sizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.scaler.ScaleX = this.ActualWidth / this.shape.Width;
            this.scaler.ScaleY = this.ActualHeight / this.shape.Height;
        }
    }
}