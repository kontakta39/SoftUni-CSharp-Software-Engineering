//3 Exercise - Oldest Family Member
using DefiningClasses;

public class StartUp
{
    static void Main()
    {
        Family family = new();

        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            string[] addPerson = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            Person person = new Person(addPerson[0], int.Parse(addPerson[1]));

            family.AddMember(person);
        }


        Person adult = family.GetMember();
        Console.WriteLine($"{adult.Name} {adult.Age}");
    }
}
