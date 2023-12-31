﻿namespace VehicleGarage;

public class Vehicle
{
    public Vehicle(string brand, string model, string licensePlateNumber)
    {
        Brand = brand;
        Model = model;
        LicensePlateNumber = licensePlateNumber;
        BatteryLevel = 100;
        IsDamaged = false;
    }

    public string Brand { get; set; }

    public string Model { get; set; }

    public string LicensePlateNumber { get; set; }

    public int BatteryLevel { get; set; }

    public bool IsDamaged { get; set; }
}