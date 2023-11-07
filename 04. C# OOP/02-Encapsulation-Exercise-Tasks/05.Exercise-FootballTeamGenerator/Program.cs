//5 Exercise - Football Team Generator
using FootballTeamGenerator;

public class StartUp
{
    static void Main()
    {
        List<Team> teams = new();
        string input = Console.ReadLine();

        while (input != "END")
        {
            string[] inputInfo = input
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (inputInfo[0] == "Team")
            {
                try
                {
                    Team team = new(inputInfo[1]);
                    teams.Add(team);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            else if (inputInfo[0] == "Add")
            {
                string currentTeam = inputInfo[1];
                Player player = new(inputInfo[2]);
                List<int> currentStats = new();

                for (int i = 3; i < inputInfo.Length; i++)
                {
                    currentStats.Add(int.Parse(inputInfo[i]));
                }

                try
                {
                    player.AddStats(currentStats);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    input = Console.ReadLine();
                    continue;
                }

                try
                {
                    bool ifExists = false;

                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (teams[i].Name == currentTeam)
                        {
                            teams[i].AddPlayer(player);
                            ifExists = true;
                            break;
                        }
                    }

                    if (ifExists == false)
                    {
                        throw new ArgumentException($"Team {currentTeam} does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            else if (inputInfo[0] == "Remove")
            {
                string currentTeam = inputInfo[1];
                string playerName = inputInfo[2];

                try
                {
                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (teams[i].Name == currentTeam)
                        {
                            teams[i].RemovePlayer(playerName);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            else if (inputInfo[0] == "Rating")
            {
                string currentTeam = inputInfo[1];

                try
                {
                    bool ifExists = false;

                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (teams[i].Name == currentTeam)
                        {
                            Console.WriteLine($"{currentTeam} - {teams[i].Rating()}");
                            ifExists = true;
                            break;
                        }
                    }

                    if (ifExists == false)
                    {
                        throw new ArgumentException($"Team {currentTeam} does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            input = Console.ReadLine();
        }
    }
}