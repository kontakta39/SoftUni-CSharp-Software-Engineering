//2 Exercise - Salary Increase
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
            Person person = new(personInfo[0], personInfo[1], int.Parse(personInfo[2]), decimal.Parse(personInfo[3]));
            people.Add(person);
        }

        decimal percentage = decimal.Parse(Console.ReadLine());

        foreach (Person person in people)
        {
            person.IncreaseSalary(percentage);
        }

        foreach (Person person in people)
        {
            Console.WriteLine(person);
        }
    }
}