namespace Vehicles.Models.Interfaces
{
    public interface IVehicle
    {
        double FuelQuantity { get; }
        double FuelConsumption { get; }
        int TankCapacity { get; }

        public string Drive(string command, double distance);
        public double Refuel(double amount);
    }
}