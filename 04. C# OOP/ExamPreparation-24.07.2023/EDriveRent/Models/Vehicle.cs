﻿using EDriveRent.Models.Contracts;
using EDriveRent.Utilities.Messages;
using System;

namespace EDriveRent.Models;

public abstract class Vehicle : IVehicle
{
    private string brand;
    private string model;
    private string licensePlateNumber;

    public Vehicle(string brand, string model, double maxMileage, string licensePlateNumber)
    {
        Brand = brand;
        Model = model;
        MaxMileage = maxMileage;
        LicensePlateNumber = licensePlateNumber;
        BatteryLevel = 100;
        IsDamaged = false;
    }

    public string Brand
    {
        get => brand;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ExceptionMessages.BrandNull);
            }

            brand = value;
        }
    }

    public string Model
    {
        get => model;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ExceptionMessages.ModelNull);
            }

            model = value;
        }
    }

    public double MaxMileage { get; private set; }

    public string LicensePlateNumber
    {
        get => licensePlateNumber;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ExceptionMessages.LicenceNumberRequired);
            }

            licensePlateNumber = value;
        }
    }

    public int BatteryLevel { get; private set; }

    public bool IsDamaged { get; private set; }

    public void Drive(double mileage)
    {
        double difference = Math.Round((mileage / MaxMileage) * 100);
        BatteryLevel -= (int)difference;

        if (GetType().Name == nameof(CargoVan))
        {
            BatteryLevel -= 5;
        }
    }

    public void Recharge()
    {
        BatteryLevel = 100;
    }

    public void ChangeStatus()
    {
        if (IsDamaged)
        {
            IsDamaged = false;
        }

        else
        {
            IsDamaged = true;
        }
    }

    public override string ToString()
    {
        string damageStatus = string.Empty;

        if (IsDamaged)
        {
            damageStatus = "damaged";
        }

        else
        {
            damageStatus = "OK";
        }

        return $"{Brand} {Model} License plate: {LicensePlateNumber} Battery: {BatteryLevel}% Status: {damageStatus}".TrimEnd();
    }
}