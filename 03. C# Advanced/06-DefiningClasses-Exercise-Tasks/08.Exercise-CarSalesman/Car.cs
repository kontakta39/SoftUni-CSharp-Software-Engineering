using System.Text;

namespace CarSalesman;

public class Car
{
    public Car(string carModel, Engine currentEngine,
        int carWeight, string carColor)
    {
        Model = carModel;
        Engine = currentEngine;
        Weight = carWeight;
        Color = carColor;
    }

    public string Model { get; set; }
    public Engine Engine { get; set; }
    public int Weight { get; set; }
    public string Color { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{Model}:");
        sb.AppendLine($"  {Engine.Model}:");
        sb.AppendLine($"   Power: {Engine.Power}");

        if (Engine.Displacement == 0)
        {
            sb.AppendLine("   Displacement: n/a");
        }

        else
        {
            sb.AppendLine($"   Displacement: {Engine.Displacement}");
        }

        sb.AppendLine($"   Efficiency: {Engine.Efficiency}");

        if (Weight == 0)
        {
            sb.AppendLine("  Weight: n/a");
        }

        else
        {
            sb.AppendLine($"  Weight: {Weight}");
        }

        sb.AppendLine($"  Color: {Color}");

        return sb.ToString().TrimEnd();
    }
}
