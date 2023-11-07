namespace SpecialCars;

public class Engine
{
    public Engine(int horsePower, double cubicCapacity)
    {
        HorsePower = horsePower;
        CubicCapacity = cubicCapacity;
    }

    private int horsePower;
    private double cubicCapacity;

    public int HorsePower
    {
        get
        {
            return horsePower;
        }
        set
        {
            horsePower = value;
        }
    }

    public double CubicCapacity
    {
        get
        {
            return cubicCapacity;
        }
        set
        {
            cubicCapacity = value;
        }
    }
}
