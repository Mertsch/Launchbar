using System;
using System.Collections.ObjectModel;

namespace Launchbar
{
    public class MenuEntryCollection : ObservableCollection<MenuEntry>
    {
        public Submenu Parent { get; set; }

        protected override void InsertItem(int index, MenuEntry item)
        {
            item.Parent = this;
            base.InsertItem(index, item);
            item.IsSelected = true;
        }

        protected override void RemoveItem(int index)
        {
            this[index].IsSelected = false;
            this[index].Parent = null;
            base.RemoveItem(index);
            if (index == 0)
            {
                if (this.Parent == null)
                {
                    return;
                }
                this.Parent.IsSelected = true;
            }
            else if (index >= this.Count)
            {
                this[this.Count - 1].IsSelected = true;
            }
            else
            {
                this[index].IsSelected = true;
            }
        }

        protected override void SetItem(int index, MenuEntry item)
        {
            this[index].Parent = null;
            item.Parent = this;
            base.SetItem(index, item);
        }
    }
}