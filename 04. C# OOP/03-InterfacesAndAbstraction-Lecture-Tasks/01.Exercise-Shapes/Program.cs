//1 Exercise - Shapes
using Shapes.Core.Interfaces;
using Shapes.Core;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}