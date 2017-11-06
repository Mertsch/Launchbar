using System;
using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes
{
    /// <summary>
    /// Interaction logic for ShapeFolder.xaml
    /// </summary>
    public sealed partial class ShapeFolder : UserControl
    {
        /// <summary>
        /// Gets or sets whether the folder is open.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)this.GetValue(IsOpenProperty); }
            set { this.SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(ShapeFolder));

        public ShapeFolder()
        {
            this.InitializeComponent();
            this.shape.DataContext = this;
        }

        private void sizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.scaler.ScaleX = this.ActualWidth / this.shape.Width;
            this.scaler.ScaleY = this.ActualHeight / this.shape.Height;
        }
    }
}