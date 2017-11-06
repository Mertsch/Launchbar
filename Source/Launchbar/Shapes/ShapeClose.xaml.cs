using System;
using System.Windows;
using System.Windows.Controls;

namespace Launchbar.Shapes
{
    /// <summary>
    /// Interaction logic for ShapeClose.xaml
    /// </summary>
    public sealed partial class ShapeClose : UserControl
    {
        /// <summary>
        /// Gets or sets whether this shape is hovered.
        /// </summary>
        public bool IsHover
        {
            get { return (bool)this.GetValue(IsHoverProperty); }
            set { this.SetValue(IsHoverProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsHover"/> property.
        /// </summary>
        public static readonly DependencyProperty IsHoverProperty =
            DependencyProperty.Register("IsHover", typeof(bool), typeof(ShapeClose));

        public ShapeClose()
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