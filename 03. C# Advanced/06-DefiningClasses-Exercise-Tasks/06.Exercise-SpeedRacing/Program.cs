//6 Exercise - Speed Racing
using SpeedRacing;

public class StartUp
{
    static void Main()
    {
        List<Car> cars = new();
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            string[] carInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (cars.Any(x => x.Model == carInfo[0]))
            {
                continue;
            }

            Car car = new Car()
            {
                Model = carInfo[0],
                FuelAmount = double.Parse(carInfo[1]),
                FuelConsumptionPerKilometer = double.Parse(carInfo[2])
            };

            cars.Add(car);
        }

        string input = Console.ReadLine();

        while (input != "End")
        {
            string[] kmInfo = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string operation = kmInfo[0];

            if (operation == "Drive")
            {
                string model = kmInfo[1];
                double kmAmount = double.Parse(kmInfo[2]);

                Car carForDriving = cars
                .Where(x => x.Model == model).First();

                cars = carForDriving.Drive(cars, kmAmount);
            }

            input = Console.ReadLine();
        }

        foreach (var currentCar in cars)
        {
            Console.WriteLine($"{currentCar.Model} {currentCar.FuelAmount:f2} {currentCar.TravelledDistance}");
        }
    }
}
