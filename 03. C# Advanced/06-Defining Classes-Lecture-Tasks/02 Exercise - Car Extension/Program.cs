﻿//02 Exercise - Car Extension
using CarManufacturer;

public class StartUp
{
    static void Main()
    {
        Car car = new Car()
        {
            Make = "VW",
            Model = "MK3",
            Year = 1992,
            FuelQuantity = 200,
            FuelConsumption = 200
        };

        car.Drive(2000);
        Console.WriteLine(car.WhoAmI());
    }
}
