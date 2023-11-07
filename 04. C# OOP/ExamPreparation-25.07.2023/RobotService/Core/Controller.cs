using RobotService.Core.Contracts;
using RobotService.Models;
using RobotService.Models.Contracts;
using RobotService.Repositories;
using RobotService.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotService.Core;

public class Controller : IController
{
    private IRepository<ISupplement> supplements;
    private IRepository<IRobot> robots;

    public Controller()
    {
        supplements = new SupplementRepository();
        robots = new RobotRepository();
    }

    public string CreateRobot(string model, string typeName)
    {
        IRobot robot = null;

        if (typeName == nameof(DomesticAssistant))
        {
            robot = new DomesticAssistant(model);
        }

        else if (typeName == nameof(IndustrialAssistant))
        {
            robot = new IndustrialAssistant(model);
        }

        else
        {
            return $"Robot type {typeName} cannot be created.";
        }

        robots.AddNew(robot);
        return $"{typeName} {model} is created and added to the RobotRepository.";
    }

    public string CreateSupplement(string typeName)
    {
        ISupplement supplement = null;

        if (typeName == nameof(SpecializedArm))
        {
            supplement = new SpecializedArm();
        }

        else if (typeName == nameof(LaserRadar))
        {
            supplement = new LaserRadar();
        }

        else
        {
            return $"{typeName} is not compatible with our robots.";
        }

        supplements.AddNew(supplement);
        return $"{typeName} is created and added to the SupplementRepository.";
    }

    public string UpgradeRobot(string model, string supplementTypeName)
    {
        ISupplement supplement = supplements.Models().FirstOrDefault(x => x.GetType().Name == supplementTypeName);
        int interfaceValue = supplement.InterfaceStandard;

        IReadOnlyCollection<IRobot> currentRobots = robots.Models().Where(x => !x.InterfaceStandards.Contains(interfaceValue)).ToArray();
        currentRobots = currentRobots.Where(x => x.Model == model).ToArray();

        if (currentRobots.Count == 0)
        {
            return $"All {model} are already upgraded!";
        }

        IRobot robot = currentRobots.FirstOrDefault();
        robot.InstallSupplement(supplement);
        supplements.RemoveByName(supplementTypeName);
        return $"{model} is upgraded with {supplementTypeName}.";
    }

    public string PerformService(string serviceName, int intefaceStandard, int totalPowerNeeded)
    {
        IReadOnlyCollection<IRobot> currentRobots = robots.Models().Where(x => x.InterfaceStandards.Contains(intefaceStandard)).OrderByDescending(x => x.BatteryLevel).ToArray();

        if (currentRobots.Count == 0)
        {
            return $"Unable to perform service, {intefaceStandard} not supported!";
        }

        int availablePower = currentRobots.Sum(r => r.BatteryLevel);

        if (availablePower < totalPowerNeeded)
        {
            return $"{serviceName} cannot be executed! {totalPowerNeeded - availablePower} more power needed.";
        }

        int counter = 0;

        foreach (var robot in currentRobots)
        {
            counter++;

            if (robot.BatteryLevel >= totalPowerNeeded)
            {
                robot.ExecuteService(totalPowerNeeded);
                break;
            }

            totalPowerNeeded -= robot.BatteryLevel;
            robot.ExecuteService(robot.BatteryLevel);
        }

        return $"{serviceName} is performed successfully with {counter} robots.";
    }

    public string RobotRecovery(string model, int minutes)
    {
        int fedCount = 0;

        IReadOnlyCollection<IRobot> robotsToFeed = robots.Models().Where(x => x.Model == model && x.BatteryCapacity / 2 > x.BatteryLevel).ToArray();

        foreach (var robot in robotsToFeed)
        {
            fedCount++;
            robot.Eating(minutes);
        }

        return $"Robots fed: {fedCount}";
    }

    public string Report()
    {
        StringBuilder sb = new();

        IReadOnlyCollection<IRobot> currentRobots = robots.Models().OrderByDescending(x => x.BatteryLevel).ThenBy(x => x.BatteryCapacity).ToArray();

        foreach (var robot in currentRobots)
        {
            sb.AppendLine(robot.ToString());
        }

        return sb.ToString().TrimEnd();
    }
}