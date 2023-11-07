//6 Exercise - Food Shortage
using FoodShortage.Core;
using FoodShortage.Core.Interfaces;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}