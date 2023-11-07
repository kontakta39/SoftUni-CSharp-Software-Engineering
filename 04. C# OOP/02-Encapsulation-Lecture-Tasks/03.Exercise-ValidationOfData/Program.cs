//3 Exercise - Validation of Data
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
            try
            {
                Person person = new(personInfo[0], personInfo[1], int.Parse(personInfo[2]), decimal.Parse(personInfo[3]));
                people.Add(person);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
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