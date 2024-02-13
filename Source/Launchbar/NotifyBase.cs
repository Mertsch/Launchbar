using System.ComponentModel;

namespace Launchbar;

public class NotifyBase : INotifyPropertyChanged
{
    /// <summary>
    /// Indicates the change of a property.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">Name of the property which changed.</param>
    [NotifyPropertyChangedInvocator]
    protected void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged!.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}