//8 Exercise - Car Salesman
using CarSalesman;

public class StartUp
{
    static void Main()
    {
        int enginesCount = int.Parse(Console.ReadLine());
        List<Engine> engines = new();

        for (int i = 0; i < enginesCount; i++)
        {
            string[] engineInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string engineModel = engineInfo[0];
            int enginePower = int.Parse(engineInfo[1]);
            int engineDisplacement = 0;
            string engineEfficiency = "n/a";

            if (engineInfo.Length == 3)
            {
                bool isNumeric = int.TryParse(engineInfo[2], out engineDisplacement);

                if (isNumeric)
                {
                    engineDisplacement = int.Parse(engineInfo[2]);
                }

                else
                {
                    engineEfficiency = engineInfo[2];
                }

            }

            else if (engineInfo.Length == 4)
            {
                engineDisplacement = int.Parse(engineInfo[2]);
                engineEfficiency = engineInfo[3];
            }

            Engine engine = new(engineModel, enginePower, engineDisplacement, engineEfficiency);
            engines.Add(engine);
        }

        int carsCount = int.Parse(Console.ReadLine());
        List<Car> cars = new();

        for (int i = 0; i < carsCount; i++)
        {
            string[] carInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string carModel = carInfo[0];
            string carEngine = carInfo[1];
            int carWeight = 0;
            string carColor = "n/a";

            if (carInfo.Length == 3)
            {
                bool isNumeric = int.TryParse(carInfo[2], out carWeight);

                if (isNumeric)
                {
                    carWeight = int.Parse(carInfo[2]);
                }

                else
                {
                    carColor = carInfo[2];
                }
            }

            else if (carInfo.Length == 4)
            {
                carWeight = int.Parse(carInfo[2]);
                carColor = carInfo[3];
            }

            Engine currentEngine = engines
            .Where(x => x.Model == carEngine).First();

            Car car = new(carModel, currentEngine, carWeight, carColor);
            cars.Add(car);
        }

        foreach (var car in cars)
        {
            Console.Write(car.ToString());
            Console.WriteLine();
        }
    }
}