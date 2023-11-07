namespace FootballTeamGenerator;

public class Player
{
    private string name;
    private int roundedSkillsLevel;

    public Player(string name)
    {
        Name = name;
        SkillsLevel = 0;
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

    public int SkillsLevel
    {
        get => roundedSkillsLevel;
        private set => roundedSkillsLevel = value;
    }

    public void AddStats(List<int> currentStats)
    {
        double skillsLevel = 0;

        for (int i = 0; i < currentStats.Count; i++)
        {
            switch (i)
            {
                case 0:
                    string enduranceStat = "Endurance";
                    if (currentStats[i] < 0 || currentStats[i] > 100)
                    {
                        throw new ArgumentException($"{enduranceStat} should be between 0 and 100.");
                    }
                    break;
                case 1:
                    string sprintStat = "Sprint";
                    if (currentStats[i] < 0 || currentStats[i] > 100)
                    {
                        throw new ArgumentException($"{sprintStat} should be between 0 and 100.");
                    }
                    break;
                case 2:
                    string dribbleStat = "Dribble";
                    if (currentStats[i] < 0 || currentStats[i] > 100)
                    {
                        throw new ArgumentException($"{dribbleStat} should be between 0 and 100.");
                    }
                    break;
                case 3:
                    string passingStat = "Passing";
                    if (currentStats[i] < 0 || currentStats[i] > 100)
                    {
                        throw new ArgumentException($"{passingStat} should be between 0 and 100.");
                    }
                    break;
                case 4:
                    string shootingStat = "Shooting";
                    if (currentStats[i] < 0 || currentStats[i] > 100)
                    {
                        throw new ArgumentException($"{shootingStat} should be between 0 and 100.");
                    }
                    break;
            }

            skillsLevel += currentStats[i];
        }

        skillsLevel /= currentStats.Count * 1.0;
        roundedSkillsLevel = (int)Math.Round(skillsLevel, 0);
    }
}