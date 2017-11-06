using System;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Launchbar.Win32;

namespace Launchbar
{
    /// <summary>
    /// An extended menu entry. It has all the properties every active element has (name, icon).
    /// </summary>
    [XmlInclude(typeof(Submenu))]
    [XmlInclude(typeof(Program))]
    public class MenuEntryAdvanced : MenuEntry
    {
        #region Fields

        private string text;

        private string iconPath;

        private int iconIndex;

        private ImageSource icon;

        private IconType iconType = IconType.Default;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the text to be shown for this entry.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                if (this.text == value)
                {
                    return;
                }
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));
            }
        }

        /// <summary>
        /// Path to a file that contains an icon at the specified icon index
        /// </summary>
        public string IconPath
        {
            get { return this.iconPath; }
            set
            {
                if (value == string.Empty)
                {
                    value = null;
                }
                if (this.iconPath == value)
                {
                    return;
                }
                this.iconPath = value;
                this.OnPropertyChanged(nameof(this.IconPath));
                this.UpdateIcon();
            }
        }

        /// <summary>
        /// Index of the icon inside the specified file
        /// </summary>
        public int IconIndex
        {
            get { return this.iconIndex; }
            set
            {
                if (this.iconIndex == value)
                {
                    return;
                }
                this.iconIndex = value;
                this.OnPropertyChanged(nameof(this.IconIndex));
                this.UpdateIcon();
            }
        }

        /// <summary>
        /// Gets the type of icon to display.
        /// </summary>
        [XmlIgnore]
        public IconType IconType
        {
            get { return this.iconType; }
            private set
            {
                if (this.iconType == value)
                {
                    return;
                }
                this.iconType = value;
                this.OnPropertyChanged(nameof(this.IconType));
            }
        }

        /// <summary>
        /// Gets the icon to display when <see cref="IconType"/> is set to <see cref="Launchbar.IconType.Custom"/>.
        /// </summary>
        [XmlIgnore]
        public ImageSource Icon
        {
            get { return this.icon; }
            private set
            {
                if (this.icon == value)
                {
                    return;
                }
                this.icon = value;
                this.OnPropertyChanged(nameof(this.Icon));
            }
        }

        #endregion

        /// <summary>
        /// Extract the icon based on the current data.
        /// </summary>
        public void UpdateIcon()
        {
            ImageSource newIcon = null;
            IconType newType = IconType.Warning;

            if (!string.IsNullOrEmpty(this.iconPath))
            {
                newIcon = WinHelper.ExtractIcon(this.iconPath, this.iconIndex);
                if (newIcon != null)
                {
                    newType = IconType.Custom;
                }
            }
            else
            {
                Program p = this as Program;
                if (p == null) // Not a program - use the default icon.
                {
                    newType = IconType.Default;
                }
                else
                {
                    if (p.IsValidFile) // Valid file?
                    {
                        newIcon = WinHelper.ExtractAssociatedIcon(p.PathAbsolute);
                        newType = IconType.Custom;
                    }
                    else if (p.IsValidPath) // Valid dictionary?
                    {
                        newType = IconType.Default;
                    }
                }
            }

            this.Icon = newIcon;
            this.IconType = newType;
        }

        /// <summary>
        /// Displays the pick icon dialog with path based on the object data and sets the new icon.
        /// </summary>
        public void ChooseIcon()
        {
            string path = @"%SystemRoot%\system32\shell32.dll"; // Default icon path
            int index = 0;
            if (!string.IsNullOrEmpty(this.iconPath)) // Did the user already choose an icon?
            {
                // Preset path and index to the previous values.
                path = this.iconPath;
                index = this.iconIndex;
            }
            else
            {
                Program p = this as Program;
                if (p != null && p.IsValidFile) // When the program path is valid, use that path as default.
                {
                    path = p.Path;
                }
            }

            // We need to have a string that is long enough to handle a more complex path than
            // the default one (more characters in length).
            StringBuilder sb = new StringBuilder(path, 4096); // 4095 + null-char should be enough

            // Methods returns one when pressing OK in the dialog.
            if (SafeNativeMethods.PickIconDlg(IntPtr.Zero, sb, (uint)sb.Capacity, ref index) == 1)
            {
                this.IconPath = sb.ToString(); //save the information
                this.IconIndex = index;
            }
            this.UpdateIcon();
        }

        /// <summary>
        /// Remove any user-set icon.
        /// </summary>
        public void ClearIcon()
        {
            this.IconPath = null;
            this.IconIndex = 0;
        }
    }
}