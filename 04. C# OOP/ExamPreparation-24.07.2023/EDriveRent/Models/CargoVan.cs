namespace EDriveRent.Models;

public class CargoVan : Vehicle
{
    public const double cargoVanMaxMileage = 180;

    public CargoVan(string brand, string model, string licensePlateNumber) : base(brand, model, cargoVanMaxMileage, licensePlateNumber)
    {
    }
}