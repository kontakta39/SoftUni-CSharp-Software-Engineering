//2 Exercise - Cars
using Cars.Core.Interfaces;
using Cars.Core;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}