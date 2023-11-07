namespace SoftUniParking;

public class Parking
{
    private List<Car> parkedCars;
    private int capacity;

    public Parking(int capacity)
    {
        parkedCars = new List<Car>();
        this.capacity = capacity;
    }

    public int Count { get { return parkedCars.Count; } }

    public string AddCar(Car car)
    {
        if (parkedCars.Any(x => x.RegistrationNumber == car.RegistrationNumber))
        {
            return $"Car with that registration number, already exists!";
        }

        else if (parkedCars.Count == capacity)
        {
            return "Parking is full!";
        }

        else
        {
            parkedCars.Add(car);
            return $"Successfully added new car {car.Make} {car.RegistrationNumber}";
        }
    }

    public string RemoveCar(string registrationNumber)
    {
        bool ifExists = false;

        for (int i = 0; i < parkedCars.Count; i++)
        {
            if (parkedCars[i].RegistrationNumber == registrationNumber)
            {
                parkedCars.Remove(parkedCars[i]);
                ifExists = true;
                break;
            }
        }

        if (ifExists == true)
        {
            return $"Successfully removed {registrationNumber}";
        }

        else
        {
            return "Car with that registration number, doesn't exist!";
        }
    }

    public Car GetCar(string registrationNumber)
    {
        Car car = new();

        foreach (var item in parkedCars)
        {
            if (item.RegistrationNumber == registrationNumber)
            {
                car = item;
                break;
            }
        }

        return car;
    }

    public void RemoveSetOfRegistrationNumber(List<string> regNumbers)
    {
        foreach (var currentRegNumber in regNumbers)
        {
            RemoveCar(currentRegNumber);
        }
    }
}
