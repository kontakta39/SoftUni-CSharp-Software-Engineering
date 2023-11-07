//2 Exercise - Multiple Implementation
using PersonInfo.Core;
using PersonInfo.Core.Interfaces;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}