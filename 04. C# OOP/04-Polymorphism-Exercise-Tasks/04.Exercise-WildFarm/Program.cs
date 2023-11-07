//4 Exercise - Wild Farm
using WildFarm.Core.Interfaces;
using WildFarm.Core;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}