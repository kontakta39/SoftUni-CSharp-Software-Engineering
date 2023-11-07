using FoodShortage.Core.Interfaces;
using FoodShortage.Models;
using FoodShortage.Models.Interfaces;
using System.Numerics;

namespace FoodShortage.Core;

public class Engine : IEngine
{
    public void Run()
    {
        List<IBuyer> citizenBuyers = new();
        List<IBuyer> rebelBuyers = new();
        int buyersCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < buyersCount; i++)
        { 
            string[] inputInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (inputInfo.Length == 4)
            {
                IBuyer citizen = new Citizen(inputInfo[0], int.Parse(inputInfo[1]), BigInteger.Parse(inputInfo[2]), inputInfo[3]);
                citizenBuyers.Add(citizen);
            }

            else
            {
                IBuyer rebel = new Rebel(inputInfo[0], int.Parse(inputInfo[1]), inputInfo[2]);
                rebelBuyers.Add(rebel);
            }
        }

        string input = Console.ReadLine();

        while (input != "End")
        {
            IBuyer currentCitizenBuyer = citizenBuyers.Where(x => x.Name == input).FirstOrDefault();

            if (currentCitizenBuyer != null)
            {
                currentCitizenBuyer.BuyFood();
            }

            else
            {
                IBuyer currentRebelBuyer = rebelBuyers.Where(x => x.Name == input).FirstOrDefault();

                if (currentRebelBuyer != null)
                {
                    currentRebelBuyer.BuyFood();
                }
            }

            input = Console.ReadLine();
        }

        int foodCount = 0;

        foreach (var item in citizenBuyers)
        {
            foodCount += item.Food;
        }

        foreach (var item in rebelBuyers)
        {
            foodCount += item.Food;
        }

        Console.WriteLine(foodCount);
    }
}