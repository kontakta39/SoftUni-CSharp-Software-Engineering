using System;
using System.Collections.Generic;
using System.Text;
using RobotService.Models.Contracts;

namespace RobotService.Models;

public abstract class Robot : IRobot
{
    private string model;
    private int batteryCapacity;
    private readonly List<int> interfaceStandards;

    public Robot(string model, int batteryCapacity, int convertionCapacityIndex)
    {
        Model = model;
        BatteryCapacity = batteryCapacity;
        BatteryLevel = batteryCapacity;
        ConvertionCapacityIndex = convertionCapacityIndex;
        interfaceStandards = new();
    }

    public string Model
    {
        get => model;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Model cannot be null or empty.");
            }

            model = value;
        }
    }

    public int BatteryCapacity
    {
        get => batteryCapacity;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Battery capacity cannot drop below zero.");
            }

            batteryCapacity = value;
        }
    }

    public int BatteryLevel { get; private set; }

    public int ConvertionCapacityIndex { get; private set; }

    public IReadOnlyCollection<int> InterfaceStandards => interfaceStandards.AsReadOnly();

    public void Eating(int minutes)
    {
        int energy = ConvertionCapacityIndex * minutes;

        if (energy > BatteryCapacity - BatteryLevel)
        {
            BatteryLevel = BatteryCapacity;
        }

        else
        {
            BatteryLevel += energy;
        }
    }

    public void InstallSupplement(ISupplement supplement)
    {
        interfaceStandards.Add(supplement.InterfaceStandard);
        BatteryCapacity -= supplement.BatteryUsage;
        BatteryLevel -= supplement.BatteryUsage;
    }

    public bool ExecuteService(int consumedEnergy)
    {
        if (BatteryLevel >= consumedEnergy)
        {
            BatteryLevel -= consumedEnergy;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.AppendLine($"{GetType().Name} {Model}:");
        sb.AppendLine($"--Maximum battery capacity: {BatteryCapacity}");
        sb.AppendLine($"--Current battery level: {BatteryLevel}");

        if (InterfaceStandards.Count == 0)
        {
            sb.AppendLine($"--Supplements installed: none");
        }

        else
        {
            sb.AppendLine($"--Supplements installed: {string.Join(" ", InterfaceStandards)}");
        }

        return sb.ToString().TrimEnd();
    }
}