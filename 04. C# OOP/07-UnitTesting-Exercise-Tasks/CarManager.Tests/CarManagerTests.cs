namespace CarManager.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CarManagerTests
    {
        private Car car;

        [SetUp]
        public void SetUp()
        {
            car = new Car("Audi", "X6", 10.5, 70.0);
        }

        [Test]
        public void ConstructorCheck()
        {
            string make = "Audi";
            string model = "X6";
            double fuelConsumption = 10.5;
            double fuelCapacity = 70.0;

            Assert.AreEqual(make, car.Make);
            Assert.AreEqual(model, car.Model);
            Assert.AreEqual(fuelConsumption, car.FuelConsumption);
            Assert.AreEqual(fuelCapacity, car.FuelCapacity);
        }

        [Test]
        public void CarShouldBeCreatedWithZeroFuelAmount()
        {
            Assert.AreEqual(0, car.FuelAmount);
        }

        [TestCase(null)]
        [TestCase("")]
        public void CarMakeThrowsAnExceptionIfSetIsNull(string make)
        {
            Assert.Throws<ArgumentException>
                (() => car = new Car(make, "X6", 10.5, 70.0), "Make cannot be null or empty!");
        }

        [TestCase(null)]
        [TestCase("")]
        public void CarModelThrowsAnExceptionIfSetIsNull(string model)
        {
            Assert.Throws<ArgumentException>
                (() => car = new Car("Audi", model, 10.5, 70.0), "Model cannot be null or empty!");
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void CarFuelConsumptionThrowsAnExceptionIfSetIsNegative(double fuelConsumption)
        {
            Assert.Throws<ArgumentException>
                (() => car = new Car("Audi", "X6", fuelConsumption, 70.0), "Fuel consumption cannot be zero or negative!");
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void CarFuelCapacityThrowsAnExceptionIfSetIsNegative(double fuelCapacity)
        {
            Assert.Throws<ArgumentException>
                (() => car = new Car("Audi", "X6", 10.5, fuelCapacity), "Fuel capacity cannot be zero or negative!");
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void CarFuelAmountThrowsAnExceptionIfSetIsNegative(double fuelAmount)
        {
            Assert.Throws<InvalidOperationException>(()
            => car.Drive(10), "Fuel amount cannot be negative!");
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void RefuelThrowsAnExceptionIfItIsNegative(double fuelToRefuel)
        {
            Assert.Throws<ArgumentException>(()
               => car.Refuel(fuelToRefuel), "Fuel amount cannot be negative!");
        }

        [TestCase(10)]
        public void RefuelTheCar(double fuelToRefuel)
        {
            car.Refuel(fuelToRefuel);

            Assert.AreEqual(fuelToRefuel, car.FuelAmount);
        }

        [TestCase(100)]
        public void RefuelTheCarWhichIsBiggerThanItsCapacity(double fuelToRefuel)
        {
            car.Refuel(fuelToRefuel);
            double expectedResult = car.FuelCapacity;

            Assert.AreEqual(expectedResult, car.FuelAmount);
        }

        [TestCase(20)]
        public void DriveDecreasesFuelAmount(double distance)
        {
            car.Refuel(100);
            double fuelNeeded = (distance / 100) * car.FuelConsumption;
            double currentFuelAmount = car.FuelAmount;
            currentFuelAmount -= fuelNeeded;
            car.Drive(distance);

            Assert.AreEqual(currentFuelAmount, car.FuelAmount);
        }

        [TestCase(20)]
        public void DriveThrowsAnExceptionIfFuelAmountIsLessThanNeeded(double distance)
        {
            Assert.Throws<InvalidOperationException>(()
               => car.Drive(distance), "You don't have enough fuel to drive!");
        }
    }
}