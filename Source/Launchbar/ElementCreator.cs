using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Launchbar
{
    /// <summary>
    /// Creates objects from a given template.
    /// </summary>
    public sealed class ElementCreator : IValueConverter
    {
        /// <summary>
        /// Creates the the objects described by a template.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">A <see cref="ControlTemplate"/>.</param>
        /// <param name="culture"></param>
        /// <returns>Objects described by the template.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ControlTemplate ct = parameter as ControlTemplate;
            if (ct == null)
            {
                throw new ArgumentException("You must specify a ControlTemplate.", nameof(parameter));
            }
            return new Control { Template = ct };
            //return ct.LoadContent();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}