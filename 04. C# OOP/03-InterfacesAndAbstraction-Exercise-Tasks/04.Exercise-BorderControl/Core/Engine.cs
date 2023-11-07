using BorderControl.Core.Interfaces;
using BorderControl.Models;
using BorderControl.Models.Interfaces;
namespace BorderControl.Core;

public class Engine : IEngine
{
    public void Run()
    {
        List<string> ids = new();
        string input = Console.ReadLine();

        while (input != "End")
        {
            string[] inputInfo = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (inputInfo.Length == 3)
            {
                ICitizen citizen = new Citizen(inputInfo[0], int.Parse(inputInfo[1]), inputInfo[2]);
                ids.Add(citizen.Id);
            }

            else
            {
                IRobot robot = new Robot(inputInfo[0], inputInfo[1]);
                ids.Add(robot.Id);
            }

            input = Console.ReadLine();
        }

        string lastDigits = Console.ReadLine();

        foreach (var item in ids)
        {
            if (item.EndsWith(lastDigits))
            {
                Console.WriteLine(item);
            }
        }
    }
}