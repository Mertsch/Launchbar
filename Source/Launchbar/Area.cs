namespace Launchbar;

public sealed class Area
{
    public double Left { get; private set; }

    public double Top { get; private set; }

    public double Width { get; private set; }

    public double Height { get; private set; }

    public bool IsActive { get; private set; }

    public event EventHandler Updated = delegate { };

    public void Update(double left, double top, double width, double height)
    {
        this.Left = left;
        this.Top = top;
        this.Width = width;
        this.Height = height;

        this.IsActive = Math.Abs(width) > 0.1 && Math.Abs(height) > 0.1; // ~0

        this.Updated(this, EventArgs.Empty);
    }
}