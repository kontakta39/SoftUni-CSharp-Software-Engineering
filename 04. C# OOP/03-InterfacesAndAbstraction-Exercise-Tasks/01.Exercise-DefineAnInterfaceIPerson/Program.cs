//1 Exercise - Define an Interface IPerson
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