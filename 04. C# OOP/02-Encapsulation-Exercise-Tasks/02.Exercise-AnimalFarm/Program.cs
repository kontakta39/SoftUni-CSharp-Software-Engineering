//2 Exercise - Animal Farm
namespace AnimalFarm;
using AnimalFarm.Models;

class Program
{
    static void Main()
    {
        string name = Console.ReadLine();
        int age = int.Parse(Console.ReadLine());

        try
        {
            Chicken chicken = new Chicken(name, age);
            Console.WriteLine($"Chicken {chicken.Name} (age {chicken.Age}) can produce {chicken.ProductPerDay} eggs per day.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}