namespace SpeedRacing;

public class Car
{
    public string Model { get; set; }
    public double FuelAmount { get; set; }
    public double FuelConsumptionPerKilometer { get; set; }
    public double TravelledDistance { get; set; }

    public List<Car> Drive(List<Car> cars, double kmAmount)
    {
        double neededLitres = kmAmount * FuelConsumptionPerKilometer;

        if (FuelAmount >= neededLitres)
        {
            FuelAmount -= neededLitres;
            TravelledDistance += kmAmount;
        }

        else
        {
            Console.WriteLine("Insufficient fuel for the drive");
        }

        return cars;
    }
}
