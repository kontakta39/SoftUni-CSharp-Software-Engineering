//6 Exercise - Generic Count Method Doubles
using GenericCount;

public class StartUp
{
    static void Main()
    {
        Box<double> currentElement = new();
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            double addElement = double.Parse(Console.ReadLine());
            currentElement.AddName(addElement);
        }

        double elementToCompare = double.Parse(Console.ReadLine());

        Console.WriteLine(currentElement.Compare(elementToCompare));
    }
}
