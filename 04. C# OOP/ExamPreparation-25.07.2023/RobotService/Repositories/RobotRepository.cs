using RobotService.Models;
using RobotService.Models.Contracts;
using RobotService.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace RobotService.Repositories;

public class RobotRepository : IRepository<IRobot>
{
    //private List<IRobot> robotRepository;

    //public RobotRepository()
    //{
    //    robotRepository = new();
    //}

    //public IReadOnlyCollection<IRobot> Models()
    //   => robotRepository.AsReadOnly();

    //public void AddNew(IRobot robot)
    //{
    //    robotRepository.Add(robot);
    //}

    //public bool RemoveByName(string robotModel)
    //{
    //    IRobot robot = robotRepository.FirstOrDefault(x => x.Model == robotModel);
    //    return robotRepository.Remove(robot);
    //}

    ////public IRobot FindByStandard(int interfaceStandard)
    ////{
    ////    IRobot robot = null;

    ////    foreach (var item in robotRepository)
    ////    {
    ////        foreach (var standard in item.InterfaceStandards)
    ////        {
    ////            if (standard == interfaceStandard)
    ////            {
    ////                robot = item;
    ////                return robot;
    ////            }
    ////        }
    ////    }

    ////    return null;
    ////}

    //public IRobot FindByStandard(int interfaceStandard)
    //    => robotRepository.FirstOrDefault(r => r.InterfaceStandards.Contains(interfaceStandard));

    List<IRobot> robots;

    public RobotRepository()
    {
        robots = new List<IRobot>();
    }

    public IReadOnlyCollection<IRobot> Models()
        => robots.AsReadOnly();

    public void AddNew(IRobot model)
        => robots.Add(model);

    public bool RemoveByName(string typeName)
        => robots.Remove(robots.FirstOrDefault(s => s.GetType().Name == typeName));

    public IRobot FindByStandard(int interfaceStandard)
        => robots.FirstOrDefault(r => r.InterfaceStandards.Contains(interfaceStandard));
}