﻿//1 Exercise - Car
using CarManufacturer;

public class StartUp
{
    static void Main()
    {
        Car car = new Car()
        {
            Make = "VW",
            Model = "MK3",
            Year = 1992
        };

        Console.WriteLine($"Make: {car.Make}\nModel: {car.Model}\nYear: {car.Year}");
    }
}
