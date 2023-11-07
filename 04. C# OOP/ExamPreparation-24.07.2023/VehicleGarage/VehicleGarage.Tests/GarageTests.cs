using NUnit.Framework;
using System.Collections.Generic;

namespace VehicleGarage.Tests;

public class GarageTests
{
    [Test]
    public void ConstructorCheck()
    {
        Garage garage = new(10);
        Assert.AreEqual(10, garage.Capacity);
        Assert.NotNull(garage.Vehicles);
    }

    [Test]
    public void CheckCapacitySetter()
    {
        int expectedCapacity = 10;

        Garage garage = new(10);
        Assert.AreEqual(expectedCapacity, garage.Capacity);
    }

    [Test]
    public void CheckVehiclesListSetter()
    {
        List<Vehicle> expectedVehicles = new();

        Garage garage = new(10);
        Assert.AreEqual(expectedVehicles.Count, garage.Vehicles.Count);
    }

    [Test]
    public void AddVehicleCorrecty()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS3525");
        bool result = garage.AddVehicle(vehicleOne);

        Assert.IsTrue(result);
        Assert.AreEqual(2, garage.Vehicles.Count);
    }

    [Test]
    public void CannotAddVehicleBecauseGarageIsFull()
    {
        Garage garage = new(1);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS3525");
        bool result = garage.AddVehicle(vehicleOne);

        Assert.IsFalse(result);
        Assert.AreEqual(1, garage.Vehicles.Count);
    }

    [Test]
    public void CannotAddVehicleBecauseVehicleWithTheSameNumberHaveAlreadyBeenAdded()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS1111");
        bool result = garage.AddVehicle(vehicleOne);

        Assert.IsFalse(result);
        Assert.AreEqual(1, garage.Vehicles.Count);
    }

    [Test]
    public void ChargeVehiclesCorrectly()
    {
        int expectedChargedVehiclesCount = 2;

        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 50;
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS1111");
        vehicleOne.BatteryLevel = 75;
        garage.Vehicles.Add(vehicleOne);

        int currentVehiclesChargedCount = garage.ChargeVehicles(90);

        Assert.AreEqual(expectedChargedVehiclesCount, currentVehiclesChargedCount);
    }

    [Test]
    public void CannotChargeVehiclesBecauseTheyMoreBatteryLevelThanTheNeededBatteryLevel()
    {
        int expectedChargedVehiclesCount = 0;

        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 50;
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS1111");
        vehicleOne.BatteryLevel = 75;
        garage.Vehicles.Add(vehicleOne);

        int currentVehiclesChargedCount = garage.ChargeVehicles(20);

        Assert.AreEqual(expectedChargedVehiclesCount, currentVehiclesChargedCount);
    }

    [Test]
    public void CannotChargeVehiclesBecauseGarageIsEmpty()
    {
        int expectedChargedVehiclesCount = 0;

        Garage garage = new(10);

        int currentVehiclesChargedCount = garage.ChargeVehicles(80);

        Assert.AreEqual(expectedChargedVehiclesCount, currentVehiclesChargedCount);
    }

    [Test]
    public void DriveVehicleAndLowerTheBatteryLevelCorrectly()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 90;
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS3525");
        vehicleOne.BatteryLevel = 95;
        garage.Vehicles.Add(vehicleOne);

        garage.DriveVehicle("SS1111", 20, false);
        garage.DriveVehicle("SS3525", 40, false);

        int expectedAudiBatteryLevel = 70;
        int expectedLadaBatteryLevel = 55;

        Assert.AreEqual(expectedAudiBatteryLevel, vehicle.BatteryLevel);
        Assert.AreEqual(expectedLadaBatteryLevel, vehicleOne.BatteryLevel);
        Assert.IsFalse(vehicle.IsDamaged);
        Assert.IsFalse(vehicleOne.IsDamaged);
    }

    [Test]
    public void DriveVehicleAndLowerTheBatteryLevelCorrectlyButHasAnAccident()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 90;
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS3525");
        vehicleOne.BatteryLevel = 95;
        garage.Vehicles.Add(vehicleOne);

        garage.DriveVehicle("SS1111", 20, true);
        garage.DriveVehicle("SS3525", 40, true);

        int expectedAudiBatteryLevel = 70;
        int expectedLadaBatteryLevel = 55;

        Assert.AreEqual(expectedAudiBatteryLevel, vehicle.BatteryLevel);
        Assert.AreEqual(expectedLadaBatteryLevel, vehicleOne.BatteryLevel);
        Assert.IsTrue(vehicle.IsDamaged);
        Assert.IsTrue(vehicleOne.IsDamaged);
    }

    [Test]
    public void DriveVehicleButItIsDamaged()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 90;
        vehicle.IsDamaged = true;
        garage.Vehicles.Add(vehicle);

        garage.DriveVehicle("SS1111", 20, false);

        int expectedAudiBatteryLevel = 90;

        Assert.AreEqual(expectedAudiBatteryLevel, vehicle.BatteryLevel);
        Assert.IsTrue(vehicle.IsDamaged);
    }

    [Test]
    public void DriveVehicleButBatteryDrainageIsBiggerThan100()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 120;
        garage.Vehicles.Add(vehicle);

        garage.DriveVehicle("SS1111", 110, false);

        int expectedAudiBatteryLevel = 120;

        Assert.AreEqual(expectedAudiBatteryLevel, vehicle.BatteryLevel);
    }

    [Test]
    public void DriveVehicleButHasMoreBatteryDrainageThanTheBatteryLevel()
    {
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.BatteryLevel = 60;
        garage.Vehicles.Add(vehicle);

        garage.DriveVehicle("SS1111", 75, false);

        int expectedAudiBatteryLevel = 60;

        Assert.AreEqual(expectedAudiBatteryLevel, vehicle.BatteryLevel);
    }

    [Test]
    public void RepairVehiclesCorrectly()
    {
        string expectedVehiclesRepaired = $"Vehicles repaired: {2}";
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        vehicle.IsDamaged = true;
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS3525");
        vehicleOne.IsDamaged = true;
        garage.Vehicles.Add(vehicleOne);

        string currentVehiclesRepaired = garage.RepairVehicles();

        Assert.AreEqual(expectedVehiclesRepaired, currentVehiclesRepaired);
        Assert.IsFalse(vehicle.IsDamaged);
        Assert.IsFalse(vehicleOne.IsDamaged);
    }

    [Test]
    public void RepairVehiclesButHasNoDamagedVehicles()
    {
        string expectedVehiclesRepaired = $"Vehicles repaired: {0}";
        Garage garage = new(10);
        Vehicle vehicle = new("Audi", "X6", "SS1111");
        garage.Vehicles.Add(vehicle);

        Vehicle vehicleOne = new("Lada", "Niva", "SS3525");
        garage.Vehicles.Add(vehicleOne);

        string currentVehiclesRepaired = garage.RepairVehicles();

        Assert.AreEqual(expectedVehiclesRepaired, currentVehiclesRepaired);
        Assert.IsFalse(vehicle.IsDamaged);
        Assert.IsFalse(vehicleOne.IsDamaged);
    }

    [Test]
    public void RepairVehiclesButGarageIsEmpty()
    {
        string expectedVehiclesRepaired = $"Vehicles repaired: {0}";
        Garage garage = new(10);

        string currentVehiclesRepaired = garage.RepairVehicles();

        Assert.AreEqual(expectedVehiclesRepaired, currentVehiclesRepaired);
    }
}