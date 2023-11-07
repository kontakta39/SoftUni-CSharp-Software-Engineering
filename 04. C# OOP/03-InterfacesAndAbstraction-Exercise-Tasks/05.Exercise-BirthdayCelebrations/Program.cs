//5 Exercise - Birthday Celebrations
using BirthdayCelebrations.Core;
using BirthdayCelebrations.Core.Interfaces;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}