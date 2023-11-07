//7 Exercise - The V-Logger
string input = Console.ReadLine();
Dictionary<string, Dictionary<string, HashSet<string>>> vloggers = new();

while (input != "Statistics")
{
    string[] currentInput = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string currentVlogger = currentInput[0];
    string followedVlogger = currentInput[2];
    string followed = "Followed";
    string following = "Following";

    if (input == $"{currentVlogger} joined The V-Logger")
    {
        if (!vloggers.ContainsKey(currentVlogger))
        {
            vloggers.Add(currentVlogger, new Dictionary<string, HashSet<string>>());
            vloggers[currentVlogger].Add(followed, new HashSet<string>());
            vloggers[currentVlogger].Add(following, new HashSet<string>());
        }
    }

    else if (input == $"{currentVlogger} followed {followedVlogger}")
    {
        if (currentVlogger == followedVlogger)
        {
            input = Console.ReadLine();
            continue;
        }

        else if (vloggers.ContainsKey(currentVlogger) && vloggers.ContainsKey(followedVlogger))
        {
            if (!vloggers[followedVlogger].ContainsKey(currentVlogger))
            {
                vloggers[followedVlogger][followed].Add(currentVlogger);
                vloggers[currentVlogger][following].Add(followedVlogger);
            }
        }
    }

    input = Console.ReadLine();
}

int vloggersCount = 0;
string mostFollowedVlogger = string.Empty;
int followedCountOfFollowers = int.MinValue;
int followingCountOfFollowers = int.MaxValue;

foreach (var (name, followers) in vloggers)
{
    vloggersCount++;

    foreach (var (followOption, count) in followers)
    {
        if (followOption == "Followed")
        {
            int currentCount = followers[followOption].Count;

            if (followedCountOfFollowers < currentCount)
            {
                followedCountOfFollowers = currentCount;
                mostFollowedVlogger = name;
            }
        }

        else if (followOption == "Following")
        {
            int currentCount = followers[followOption].Count;

            if (followingCountOfFollowers > currentCount)
            {
                followingCountOfFollowers = currentCount;
                mostFollowedVlogger = name;
            }
        }
    }
}

Console.WriteLine($"The V-Logger has a total of {vloggersCount} vloggers in its logs.");
Dictionary<string, Dictionary<string, HashSet<string>>> orderedVloggers = vloggers
    .OrderByDescending(x => x.Value["Followed"].Count)
    .ThenBy(x => x.Value["Following"].Count)
    .ToDictionary(x => x.Key, x => x.Value);
int numbering = 1;

foreach (var (name, followers) in orderedVloggers)
{
    int followedCount = followers["Followed"].Count;
    int followingCount = followers["Following"].Count;

    Console.WriteLine($"{numbering}. {name} : {followedCount} followers, {followingCount} following");
    numbering++;

    if (name == mostFollowedVlogger)
    {
        foreach (var (followOption, names) in followers)
        {
            HashSet<string> orderedFollowers = names.OrderBy(x => x).ToHashSet();

            foreach (var currentFollower in orderedFollowers)
            {
                Console.WriteLine($"*  {currentFollower}");
            }
        }
    }
}

//Dictionary<string, Dictionary<string, HashSet<string>>> vloggers = new();

//string input = string.Empty;
//while ((input = Console.ReadLine()) != "Statistics")
//{
//    string[] tokens = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

//    string command = tokens[1];

//    if (command == "joined")
//    {
//        string vloggerName = tokens[0];

//        if (!vloggers.ContainsKey(vloggerName))
//        {
//            vloggers.Add(vloggerName, new Dictionary<string, HashSet<string>>());
//            vloggers[vloggerName].Add("followers", new HashSet<string>());
//            vloggers[vloggerName].Add("following", new HashSet<string>());
//        }
//    }
//    else if (command == "followed")
//    {
//        string vlogger = tokens[0];
//        string vloggerToFollow = tokens[2];

//        if (vloggers.ContainsKey(vlogger) &&
//            vloggers.ContainsKey(vloggerToFollow) &&
//            vlogger != vloggerToFollow)
//        {
//            vloggers[vlogger]["following"].Add(vloggerToFollow);
//            vloggers[vloggerToFollow]["followers"].Add(vlogger);
//        }
//    }
//}

//int count = 1;

//Console.WriteLine($"The V-Logger has a total of {vloggers.Count} vloggers in its logs.");

//Dictionary<string, Dictionary<string, HashSet<string>>> orderedVloggers = vloggers
//    .OrderByDescending(v => v.Value["followers"].Count)
//    .ThenBy(v => v.Value["following"].Count)
//    .ToDictionary(v => v.Key, v => v.Value);

//foreach (var vlogger in orderedVloggers)
//{
//    Console.WriteLine($"{count}. {vlogger.Key} : {vlogger.Value["followers"].Count} followers, {vlogger.Value["following"].Count} following");

//    if (count == 1)
//    {
//        //Try SortedSet for vloggers 
//        List<string> orderedFollowers = vlogger.Value["followers"]
//            .OrderBy(f => f)
//            .ToList();

//        foreach (var follower in orderedFollowers)
//        {
//            Console.WriteLine($"*  {follower}");
//        }
//    }

//    count++;
//}