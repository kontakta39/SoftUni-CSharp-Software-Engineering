//3 Exercise - Car Constructors
using CarManufacturer;

public class StartUp
{
    static void Main()
    {
        Car firstCar = new Car();
        Car secondCar = new Car("VW", "Golf", 2025);
        Car thirdCar = new Car("VW", "Golf", 2025, 200, 10);
    }
}
