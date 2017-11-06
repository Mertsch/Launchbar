using System;
using System.Xml.Serialization;

namespace Launchbar
{
    /// <summary>
    /// This class introduces lower level entries.
    /// </summary>
    public sealed class Submenu : MenuEntryAdvanced
    {
        #region Fields

        private MenuEntryCollection entries;

        private bool isExpanded;

        #endregion

        /// <summary>
        /// Gets or sets whether this submenu is expanded.
        /// </summary>
        [XmlIgnore]
        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (this.isExpanded == value)
                {
                    return;
                }
                this.isExpanded = value;
                this.OnPropertyChanged(nameof(this.IsExpanded));
            }
        }

        /// <summary>
        /// List of entries inside this sub menu.
        /// </summary>
        public MenuEntryCollection MenuEntries
        {
            get { return this.entries; }
            set
            {
                if (this.entries == value)
                {
                    return;
                }
                if (this.entries != null)
                {
                    this.entries.Parent = null;
                }
                this.entries = value;
                if (this.entries != null)
                {
                    this.entries.Parent = this;
                }

                this.OnPropertyChanged(nameof(this.MenuEntries));
            }
        }

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        public Submenu()
        {
            this.entries = new MenuEntryCollection { Parent = this };
            this.Text = "Submenu";
        }
    }
}