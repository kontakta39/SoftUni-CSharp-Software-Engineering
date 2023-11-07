namespace Vehicles.Models
{
    public class Truck : Vehicle
    {
        private const double IncreasedConsumption = 1.6;

        public Truck(double fuelQuantity, double fuelConsumption, int tankCapacity)
            : base(fuelQuantity, fuelConsumption, IncreasedConsumption, tankCapacity)
        {
        }

        public override double Refuel(double amount)
        {
            if (FuelQuantity + amount > TankCapacity)
            {
                throw new ArgumentException($"Cannot fit {amount} fuel in the tank");
            }

            return base.Refuel(amount * 0.95);
        }
    }
}