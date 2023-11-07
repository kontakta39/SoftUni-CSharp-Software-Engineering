using Cars.Core.Interfaces;
using Cars.Models.Interfaces;
using Cars.Models;

namespace Cars.Core;

public class Engine : IEngine
{
    public void Run()
    {
        ICar seat = new Seat("Leon", "Grey");
        ICar tesla = new Tesla("Model 3", "Red", 2);

        Console.WriteLine(seat.ToString());
        Console.WriteLine(seat.Start());
        Console.WriteLine(seat.Stop());
        Console.WriteLine(tesla.ToString());
        Console.WriteLine(tesla.Start());
        Console.WriteLine(tesla.Stop());
    }
}