using System.Windows;

namespace Launchbar;

public static class MenuItemExtensions
{
    public static void SetIconTemplate(DependencyObject d, DataTemplate? value)
    {
        d.SetValue(IconTemplateProperty, value);
    }

    [MustUseReturnValue]
    public static DataTemplate? GetIconTemplate(DependencyObject d)
    {
        return (DataTemplate)d.GetValue(IconTemplateProperty);
    }

    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.RegisterAttached(
        "IconTemplate", typeof(DataTemplate), typeof(MenuItemExtensions), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
}