//1 Exercise - Sort People by Name and Age
using PersonsInfo;

public class StartUp
{
    static void Main()
    {
        int peopleCount = int.Parse(Console.ReadLine());
        List<Person> people = new();

        for (int i = 0; i < peopleCount; i++)
        {
            string[] personInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Person person = new(personInfo[0], personInfo[1], int.Parse(personInfo[2]));
            people.Add(person);
        }

        foreach (Person person in people.OrderBy(x => x.FirstName).ThenBy(x => x.Age))
        {
            Console.WriteLine(person);
        }
    }
}