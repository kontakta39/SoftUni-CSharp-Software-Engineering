//4 Exercise - Border Control
using BorderControl.Core;
using BorderControl.Core.Interfaces;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}