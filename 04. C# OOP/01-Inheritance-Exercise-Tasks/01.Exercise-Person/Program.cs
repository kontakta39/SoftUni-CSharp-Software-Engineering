//1 Exercise - Person
using People;

public class StartUp
{
    static void Main()
    {
        string name = Console.ReadLine();
        int age = int.Parse(Console.ReadLine());

        if (age > 15)
        {
            Person person = new(name, age);
            Console.WriteLine(person);
        }

        else if (age >= 0 && age <= 15)
        {
            Child child = new Child(name, age);
            Console.WriteLine(child);
        }
    }
}