using Vehicles.Models.Interfaces;

namespace Vehicles.Models
{
    public abstract class Vehicle : IVehicle
    {
        private double increasedConsumption;

        public Vehicle(double fuelQuantity, double fuelConsumption, double increasedConsumption)
        {
            FuelQuantity = fuelQuantity;
            FuelConsumption = fuelConsumption;
            this.increasedConsumption = increasedConsumption;
        }

        public double FuelQuantity { get; private set; }
        public double FuelConsumption { get; private set; }

        public string Drive(double distance)
        {
            if (FuelQuantity < distance * (FuelConsumption + increasedConsumption))
            {
                throw new ArgumentException($"{GetType().Name} needs refueling");
            }

            FuelQuantity -= distance * (FuelConsumption + increasedConsumption);
            return $"{GetType().Name} travelled {distance} km";
        }

        public virtual double Refuel(double amount) => FuelQuantity += amount;
    }
}