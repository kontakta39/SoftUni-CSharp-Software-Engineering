//7 Exercise - Raw Data
using RawData;

public class StartUp
{
    static void Main()
    {
        int count = int.Parse(Console.ReadLine());
        List<Car> cars = new();

        for (int i = 0; i < count; i++)
        {
            string[] carInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string model = carInfo[0];
            int engineSpeed = int.Parse(carInfo[1]);
            int enginePower = int.Parse(carInfo[2]);
            int cargoWeight = int.Parse(carInfo[3]);
            string cargoType = carInfo[4];
            float tire1Pressure = float.Parse(carInfo[5]);
            int tire1Age = int.Parse(carInfo[6]);
            float tire2Pressure = float.Parse(carInfo[7]);
            int tire2Age = int.Parse(carInfo[8]);
            float tire3Pressure = float.Parse(carInfo[9]);
            int tire3Age = int.Parse(carInfo[10]);
            float tire4Pressure = float.Parse(carInfo[11]);
            int tire4Age = int.Parse(carInfo[12]);

            Car car = new(model, engineSpeed, enginePower, cargoWeight,
                         cargoType, tire1Pressure, tire1Age, tire2Pressure,
                         tire2Age, tire3Pressure, tire3Age,
                         tire4Pressure, tire4Age);

            cars.Add(car);
        }

        string command = Console.ReadLine();

        foreach (var car in cars)
        {
            if (car.Cargo.Type == command && command == "fragile")
            {
                foreach (var tire in car.Tires)
                {
                    if (tire.Pressure < 1)
                    {
                        Console.WriteLine(car.Model);
                        break;
                    }
                }
            }

            else if (car.Cargo.Type == command && command == "flammable")
            {
                if (car.Engine.Power > 250)
                {
                    Console.WriteLine(car.Model);
                }
            }
        }
    }
}
