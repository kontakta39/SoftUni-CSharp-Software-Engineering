namespace SpecialCars;

public class Car
{
    public Car(string make, string model, int year, double fuelQuantity, double fuelConsumption, int engineIndex, int tiresIndex, int horsePower, double pressure)
    {
        Make = make;
        Model = model;
        Year = year;
        FuelQuantity = fuelQuantity;
        FuelConsumption = fuelConsumption;
        EngineIndex = engineIndex;
        TiresIndex = tiresIndex;
        HorsePower = horsePower;
        Pressure = pressure;
    }

    private string make;
    private string model;
    private int year;
    private double fuelQuantity;
    private double fuelConsumption;
    private int engineIndex;
    private int tiresIndex;
    private int horsePower;
    private double pressure;

    public string Make
    {
        get { return make; }
        set { make = value; }
    }

    public string Model
    {
        get { return model; }
        set { model = value; }
    }

    public int Year
    {
        get { return year; }
        set { year = value; }
    }

    public double FuelQuantity
    {
        get { return fuelQuantity; }
        set { fuelQuantity = value; }
    }

    public double FuelConsumption
    {
        get { return fuelConsumption; }
        set { fuelConsumption = value; }
    }

    public int EngineIndex
    {
        get { return engineIndex; }
        set { engineIndex = value; }
    }

    public int TiresIndex
    {
        get { return tiresIndex; }
        set { tiresIndex = value; }
    }

    public int HorsePower
    {
        get { return horsePower; }
        set { horsePower = value; }
    }


    public double Pressure
    {
        get { return pressure; }
        set { pressure = value; }
    }

    public double Drive20Kilometers(double fuelQuantity, double fuelConsumption)
    {
        fuelQuantity -= (fuelConsumption / 100) * 20;

        return fuelQuantity;
    }
}
