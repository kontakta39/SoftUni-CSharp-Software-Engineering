using Shapes.Core.Interfaces;
using Shapes.Models.Interfaces;
using Shapes.Models;

namespace Shapes.Core;

public class Engine : IEngine
{
    public void Run()
    {
        var radius = int.Parse(Console.ReadLine());
        IDrawable circle = new Circle(radius);

        var width = int.Parse(Console.ReadLine());
        var height = int.Parse(Console.ReadLine());
        IDrawable rect = new Models.Rectangle(width, height);

        circle.Draw();
        rect.Draw();
    }
}