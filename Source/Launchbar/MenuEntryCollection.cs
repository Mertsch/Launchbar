using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace Launchbar
{
    public class MenuEntryCollection : ObservableCollection<MenuEntry>
    {
        public Submenu Parent { get; set; }

        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void InsertItem(int index, [NotNull] MenuEntry item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
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
                this[^1].IsSelected = true;
            }
            else
            {
                this[index].IsSelected = true;
            }
        }

        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void SetItem(int index, [NotNull] MenuEntry item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            this[index].Parent = null;
            item.Parent = this;
            base.SetItem(index, item);
        }
    }
}