using Vehicles.Core.Interfaces;
using Vehicles.Models;
using Vehicles.Models.Interfaces;

namespace Vehicles.Core;

public class Engine : IEngine
{
    public void Run()
    {
        List<IVehicle> vehicles = new();
        string[] input = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);

        while (input.Length > 1)
        {
            if (input[0] == "Car")
            {
                IVehicle vehicle = new Car(double.Parse(input[1]), double.Parse(input[2]), int.Parse(input[3]));
                vehicles.Add(vehicle);
            }

            else if (input[0] == "Truck")
            {
                IVehicle vehicle = new Truck(double.Parse(input[1]), double.Parse(input[2]), int.Parse(input[3]));
                vehicles.Add(vehicle);
            }

            else if (input[0] == "Bus")
            {
                IVehicle vehicle = new Bus(double.Parse(input[1]), double.Parse(input[2]), int.Parse(input[3]));
                vehicles.Add(vehicle);
            }

            input = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }

        int operationsCount = int.Parse(input[0]);

        for (int i = 0; i < operationsCount; i++)
        {
            string[] operationInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            try
            {
                if (operationInfo[0] == "Drive" || operationInfo[0] == "DriveEmpty")
                {
                    foreach (var item in vehicles)
                    {
                        if (item.GetType().Name == operationInfo[1])
                        {
                            Console.WriteLine(item.Drive(operationInfo[0], double.Parse(operationInfo[2])));
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