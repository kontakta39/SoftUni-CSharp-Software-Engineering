namespace SpecialCars;

public class Tire
{
    public Tire(int year, double pressure)
    {
        Year = year;
        Pressure = pressure;
    }

    private int year;
    private double pressure;

    public int Year
    {
        get
        {
            return year;
        }
        set
        {
            year = value;
        }
    }

    public double Pressure
    {
        get
        {
            return pressure;
        }
        set
        {
            pressure = value;
        }
    }

    public static double GetSumPressure(List<List<Tire>> tiresCount,
    int tiresIndex)
    {
        double sumPressure = 0;

        foreach (var item in tiresCount[tiresIndex])
        {
            sumPressure += item.Pressure;
        }

        return sumPressure;
    }
}
