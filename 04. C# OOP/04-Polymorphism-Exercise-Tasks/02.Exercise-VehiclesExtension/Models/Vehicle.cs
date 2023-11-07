using System.Data;
using Vehicles.Models.Interfaces;

namespace Vehicles.Models
{
    public abstract class Vehicle : IVehicle
    {
        private double increasedConsumption;
        private double fuelQuantity;

        public Vehicle(double fuelQuantity, double fuelConsumption, double increasedConsumption, int tankCapacity)
        {
            TankCapacity = tankCapacity;
            FuelQuantity = fuelQuantity;
            FuelConsumption = fuelConsumption;
            this.increasedConsumption = increasedConsumption;
        }

        public double FuelQuantity
        {
            get => fuelQuantity;
            private set
            {
                if (value > TankCapacity)
                {
                    fuelQuantity = 0;
                }

                else
                {
                    fuelQuantity = value;
                }
            }
        }
        public double FuelConsumption { get; private set; }
        public int TankCapacity { get; private set; }

        public string Drive(string command, double distance)
        {
            if (command == "DriveEmpty")
            {
                increasedConsumption = 0;
            }

            if (FuelQuantity < distance * (FuelConsumption + increasedConsumption))
            {
                throw new ArgumentException($"{GetType().Name} needs refueling");
            }

            FuelQuantity -= distance * (FuelConsumption + increasedConsumption);
            return $"{GetType().Name} travelled {distance} km";
        }

        public virtual double Refuel(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Fuel must be a positive number");
            }

            if (FuelQuantity + amount > TankCapacity)
            {
                throw new ArgumentException($"Cannot fit {amount} fuel in the tank");
            }

            return FuelQuantity += amount;
        }
    }
}