using Vehicles.Core.Interfaces;
using Vehicles.Models;
using Vehicles.Models.Interfaces;

namespace Vehicles.Core;

public class Engine : IEngine
{
    public void Run()
    {
        List<IVehicle> vehicles = new();
        string[] input = Console.ReadLine().Split(" ");

        while (input.Length > 1)
        {
            if (input[0] == "Car")
            {
                IVehicle vehicle = new Car(double.Parse(input[1]), double.Parse(input[2]));
                vehicles.Add(vehicle);
            }

            else if (input[0] == "Truck")
            {
                IVehicle vehicle = new Truck(double.Parse(input[1]), double.Parse(input[2]));
                vehicles.Add(vehicle);
            }

            input = Console.ReadLine().Split(" ");
        }

        int operationsCount = int.Parse(input[0]);

        for (int i = 0; i < operationsCount; i++)
        {
            string[] operationInfo = Console.ReadLine().Split(" ");

            try
            {
                if (operationInfo[0] == "Drive")
                {
                    foreach (var item in vehicles)
                    {
                        if (item.GetType().Name == operationInfo[1])
                        {
                            Console.WriteLine(item.Drive(double.Parse(operationInfo[2])));
                            break;
                        }
                    }
                }

                else if (operationInfo[0] == "Refuel")
                {
                    foreach (var item in vehicles)
                    {
                        if (item.GetType().Name == operationInfo[1])
                        {
                            item.Refuel(double.Parse(operationInfo[2]));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        foreach (var item in vehicles)
        {
            Console.WriteLine($"{item.GetType().Name}: {item.FuelQuantity:f2}");
        }
    }
}