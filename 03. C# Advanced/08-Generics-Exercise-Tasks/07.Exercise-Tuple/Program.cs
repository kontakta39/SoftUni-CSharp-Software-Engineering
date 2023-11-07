//7 Exercise - Tuple
using Tuple;

public class StartUp
{
    static void Main()
    {
        string[] personInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        personInfo[0] += " " + personInfo[1];
        string[] beerInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] numberInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);

        TupleTypes<string, string> personAddress = new(personInfo[0], personInfo[2]);
        TupleTypes<string, int> personBeer = new(beerInfo[0], int.Parse(beerInfo[1]));
        TupleTypes<int, double> numbers = new(int.Parse(numberInfo[0]), double.Parse(numberInfo[1]));

        Console.WriteLine(personAddress.ToString());
        Console.WriteLine(personBeer.ToString());
        Console.WriteLine(numbers.ToString());
    }
}
