//4 Exercise - First and Reserve Team
using PersonsInfo;

public class StartUp
{
    static void Main()
    {
        Team team = new("SoftUni");
        int peopleCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < peopleCount; i++)
        {
            string[] personInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                Person person = new(personInfo[0], personInfo[1], int.Parse(personInfo[2]), decimal.Parse(personInfo[3]));
                team.AddPlayer(person);
        }

        Console.WriteLine($"First team has {team.FirstTeam.Count} players.");
        Console.WriteLine($"Reserve team has {team.ReserveTeam.Count} players.");
    }
}