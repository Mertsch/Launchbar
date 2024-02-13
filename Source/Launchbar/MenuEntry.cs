using System.Xml.Serialization;

namespace Launchbar;

/// <summary>
/// The base for all different menu entries.
/// </summary>
[XmlInclude(typeof(MenuEntryAdvanced))]
[XmlInclude(typeof(Separator))]
[XmlInclude(typeof(MenuEntrySettings))]
[XmlInclude(typeof(MenuEntryExit))]
public class MenuEntry : NotifyBase
{
    public const string IsSelectedProperty = "IsSelected";

    private bool isSelected;

    /// <summary>
    /// Gets or sets whether this object is selected.
    /// </summary>
    [XmlIgnore]
    public bool IsSelected
    {
        get { return this.isSelected; }
        set
        {
            if (this.isSelected == value)
            {
                return;
            }
            this.isSelected = value;
            this.OnPropertyChanged(IsSelectedProperty);
        }
    }

    /// <summary>
    /// Gets or sets the parent of this object.
    /// </summary>
    [XmlIgnore]
    public MenuEntryCollection? Parent { get; set; }

    /// <summary>
    /// Move this item upwards in the parent collection.
    /// </summary>
    public void MoveUp()
    {
        this.move(true);
    }

    /// <summary>
    /// Move this item downwards in the parent collection.
    /// </summary>
    public void MoveDown()
    {
        this.move(false);
    }

    private void move(bool up)
    {
        MenuEntryCollection? parent = this.Parent;
        if (parent == null)
        {
            return; // Unable to do anything
        }

        int index = parent.IndexOf(this);
        // The item is at the beginning of the collection
        if ((index == 0 && up) || (index == parent.Count - 1 && !up))
        {
            if (parent.Parent == null)
            {
                return; // We can't move the element any further.
            }
            MenuEntryCollection? parentParent = parent.Parent.Parent;
            if (parentParent == null)
            {
                return; // There is nothing we can do.
            }
            int parentIndex = parentParent.IndexOf(parent.Parent);
            this.IsSelected = false;
            parent.Remove(this);
            if (up)
            {
                parentParent.Insert(parentIndex, this);
            }
            else
            {
                parentParent.Insert(parentIndex + 1, this);
            }
            this.IsSelected = true;
        }
        else // The element is somewhere in the parent collection and can be moved.
        {
            int delta = 1; // Increase the index by one.
            if (up)
            {
                delta = -1; // Decrease the index by one.
            }

            Submenu? moveInto = parent[index + delta] as Submenu;
            if (moveInto == null)
            {
                parent.Move(index, index + delta);
            }
            else
            {
                this.IsSelected = false;
                parent.Remove(this);
                moveInto.IsExpanded = true;
                if (up)
                {
                    moveInto.MenuEntries.Add(this);
                }
                else
                {
                    moveInto.MenuEntries.Insert(0, this);
                }
                this.IsSelected = true;
            }
        }
    }
}