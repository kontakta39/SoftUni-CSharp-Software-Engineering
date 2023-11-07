namespace BoxData;

public class Box
{
    private double length;
    private double width;
    private double height;

    public Box(double length, double width, double height)
    {
        Length = length;
        Width = width;
        Height = height;
    }

    public double Length
    {
        get => length;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{nameof(Length)} cannot be zero or negative.");
            }

            length = value;
        }
    }

    public double Width
    {
        get => width;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{nameof(Width)} cannot be zero or negative.");
            }

            width = value;
        }
    }

    public double Height
    {
        get => height;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{nameof(Height)} cannot be zero or negative.");
            }

            height = value;
        }
    }

    public double SurfaceArea()
    {
        double surfaceArea = 2 * Length * Width + LateralSurfaceArea();
        return surfaceArea;
    }

    public double LateralSurfaceArea()
    {
        double lateralSurfaceArea = 2 * Length * Height + 2 * Width * Height;
        return lateralSurfaceArea;
    }

    public double Volume()
    {
        double volume = Length * Width * Height;
        return volume;
    }
}