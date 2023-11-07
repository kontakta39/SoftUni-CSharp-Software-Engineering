namespace Vehicles.Models.Interfaces
{
    public interface IVehicle
    {
        double FuelQuantity { get; }
        double FuelConsumption { get; }

        public string Drive(double distance);
        public double Refuel(double amount);
    }
}