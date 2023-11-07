using System.Text;

namespace AutomotiveRepairShop;

public class RepairShop
{
    public RepairShop(int capacity)
    {
        Capacity = capacity;
        Vehicles = new();
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (Vehicles.Count < Capacity)
        {
            Vehicles.Add(vehicle);
        }
    }

    public bool RemoveVehicle(string vin)
    {
        Vehicle vehicle = Vehicles.Where(x => x.VIN == vin).FirstOrDefault();
        bool isContained = Vehicles.Remove(vehicle);
        return isContained;
    }

    public int GetCount()
    {
        return Vehicles.Count;
    }

    public Vehicle GetLowestMileage()
    {
        Vehicle vehicle = Vehicles.OrderBy(x => x.Mileage).FirstOrDefault();
        return vehicle;
    }

    public string Report()
    {
        StringBuilder sb = new();

        sb.AppendLine("Vehicles in the preparatory:");

        foreach (Vehicle vehicle in Vehicles)
        {
            sb.AppendLine(vehicle.ToString());
        }

        return sb.ToString().TrimEnd();
    }

    public int Capacity { get; set; }
    public List<Vehicle> Vehicles { get; set; }
}
