using System;

namespace Launchbar
{
    public sealed class Area
    {
        public int Left { get; private set; }

        public int Top { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public bool IsActive { get; private set; }

        public event EventHandler Updated = delegate { };

        public void Update(int left, int top, int width, int height)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;

            this.IsActive = width != 0 && height != 0;

            this.Updated(this, EventArgs.Empty);
        }
    }
}