//5 Exercise - Special Cars
using SpecialCars;

public class StartUp
{
    static void Main()
    {
        string tireInput = Console.ReadLine();
        List<List<Tire>> tiresCount = new();

        while (tireInput != "No more tires")
        {
            List<Tire> tires = new();

            string[] tyresInfo = tireInput
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int tyre1Year = int.Parse(tyresInfo[0]);
            double tyre1Pressure = double.Parse(tyresInfo[1]);
            int tyre2Year = int.Parse(tyresInfo[2]);
            double tyre2Pressure = double.Parse(tyresInfo[3]);
            int tyre3Year = int.Parse(tyresInfo[4]);
            double tyre3Pressure = double.Parse(tyresInfo[5]);
            int tyre4Year = int.Parse(tyresInfo[6]);
            double tyre4Pressure = double.Parse(tyresInfo[7]);

            Tire[] addTyre = new Tire[4]
                {
                new Tire(tyre1Year, tyre1Pressure),
                new Tire(tyre2Year, tyre2Pressure),
                new Tire(tyre3Year, tyre3Pressure),
                new Tire(tyre4Year, tyre4Pressure)
                };

            for (int i = 0; i < addTyre.Length; i++)
            {
                tires.Add(addTyre[i]);
            }

            tiresCount.Add(tires);
            tireInput = Console.ReadLine();
        }

        string engineInput = Console.ReadLine();
        List<Engine> engines = new();

        while (engineInput != "Engines done")
        {
            string[] enginesInfo = engineInput
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int engineHorsePower = int.Parse(enginesInfo[0]);
            double engineCubicCapacity = double.Parse(enginesInfo[1]);


            Engine[] addEngine = new Engine[1]
               {
                new Engine(engineHorsePower, engineCubicCapacity)
               };

            for (int i = 0; i < addEngine.Length; i++)
            {
                engines.Add(addEngine[i]);
            }

            engineInput = Console.ReadLine();
        }

        string carInput = Console.ReadLine();
        List<Car> cars = new();

        while (carInput != "Show special")
        {
            string[] carInfo = carInput
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string carName = carInfo[0];
            string carModel = carInfo[1];
            int year = int.Parse(carInfo[2]);
            double fuelQuantity = int.Parse(carInfo[3]);
            double fuelConsumption = double.Parse(carInfo[4]);
            int engineIndex = int.Parse(carInfo[5]);
            int tiresIndex = int.Parse(carInfo[6]);

            int horsePower = engines[engineIndex].HorsePower;
            double pressure = Tire.GetSumPressure(tiresCount, tiresIndex);

            Car car = new(carName, carModel, year, fuelQuantity, fuelConsumption, engineIndex, tiresIndex, horsePower, pressure);
            cars.Add(car);
            carInput = Console.ReadLine();
        }

        foreach (var car in cars)
        {
            if (car.Year >= 2017 && car.HorsePower >= 330
                 && car.Pressure > 9 && car.Pressure < 10)
            {
                car.FuelQuantity = car.Drive20Kilometers(car.FuelQuantity, car.FuelConsumption);

                Console.WriteLine($"Make: {car.Make}");
                Console.WriteLine($"Model: {car.Model}");
                Console.WriteLine($"Year: {car.Year}");
                Console.WriteLine($"HorsePowers: {car.HorsePower}");
                Console.WriteLine($"FuelQuantity: {car.FuelQuantity}");
            }
        }
    }
}

