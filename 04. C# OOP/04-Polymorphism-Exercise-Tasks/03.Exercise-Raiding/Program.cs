//3 Exercise - Raiding
using Raiding.Core.Interfaces;
using Raiding.Core;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}