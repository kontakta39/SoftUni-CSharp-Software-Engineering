namespace FootballTeamGenerator;

public class Team
{
    private string name;
    private List<Player> players;

    public Team(string name)
    {
        Name = name;
        players = new();
    }

    public string Name
    {
        get => name;
        private set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Name)} cannot be empty");
            }

            name = value;
        }
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void RemovePlayer(string playerName)
    {
        bool ifExists = false;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Name == playerName)
            {
                ifExists = true;
                players.RemoveAt(i);
                break;
            }
        }

        if (ifExists == false)
        {
            throw new ArgumentException($"Player {playerName} is not in {Name} team.");
        }
    }

    public int Rating()
    {
        double allStatsSum = 0;
        int roundedAllStatsSum = 0;

        foreach (var item in players)
        {
            allStatsSum += item.SkillsLevel;
        }

        if (allStatsSum > 0)
        {
            allStatsSum /= players.Count * 1.0;
            roundedAllStatsSum = (int)Math.Round(allStatsSum, 0);
        }

        else
        {
            roundedAllStatsSum = 0;
        }

        return roundedAllStatsSum;
    }
}