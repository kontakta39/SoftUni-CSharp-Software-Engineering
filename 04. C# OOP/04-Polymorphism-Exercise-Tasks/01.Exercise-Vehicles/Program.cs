//1 Exercise - Vehicles
using Vehicles.Core.Interfaces;
using Vehicles.Core;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}