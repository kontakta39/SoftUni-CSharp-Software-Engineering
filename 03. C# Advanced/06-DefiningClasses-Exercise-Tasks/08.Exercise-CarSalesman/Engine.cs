namespace CarSalesman;

public class Engine
{
    public Engine(string engineModel, 
        int enginePower, int engineDisplacement, string engineEfficiency)
    {
        Model = engineModel;
        Power = enginePower;
        Displacement = engineDisplacement;
        Efficiency = engineEfficiency;
    }

    public string Model { get; set; }
    public int Power { get; set; }
    public int Displacement { get; set; }
    public string Efficiency { get; set; }
}
