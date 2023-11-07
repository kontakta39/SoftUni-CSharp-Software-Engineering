using BirthdayCelebrations.Core.Interfaces;
using BirthdayCelebrations.Models;
using BirthdayCelebrations.Models.Interfaces;
using System.Numerics;

namespace BirthdayCelebrations.Core;

public class Engine : IEngine
{
    public void Run()
    {
        List<string> birthdates = new();
        string input = Console.ReadLine();

        while (input != "End")
        {
            string[] inputInfo = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (inputInfo[0] == "Citizen")
            {
                ICitizen citizen = new Citizen(inputInfo[1], int.Parse(inputInfo[2]), BigInteger.Parse(inputInfo[3]), inputInfo[4]);
                birthdates.Add(citizen.Birthdate);
            }

            else if (inputInfo[0] == "Pet")
            {
                IPet pet = new Pet(inputInfo[1], inputInfo[2]);
                birthdates.Add(pet.Birthdate);
            }

            else
            {
                IRobot robot = new Robot(inputInfo[1], BigInteger.Parse(inputInfo[2]));
            }

            input = Console.ReadLine();
        }

        int specificYear = int.Parse(Console.ReadLine());

        foreach (var item in birthdates)
        {
            if (item.EndsWith(specificYear.ToString()))
            {
                Console.WriteLine(item);
            }
        }
    }
}