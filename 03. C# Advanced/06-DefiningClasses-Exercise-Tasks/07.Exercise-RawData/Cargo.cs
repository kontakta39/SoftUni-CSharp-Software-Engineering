namespace RawData;

public class Cargo
{
    public Cargo(int cargoWeight, string cargoType)
    {
        Weight = cargoWeight;
        Type = cargoType;
    }

    public int Weight { get; set; }
    public string Type { get; set; }
}
