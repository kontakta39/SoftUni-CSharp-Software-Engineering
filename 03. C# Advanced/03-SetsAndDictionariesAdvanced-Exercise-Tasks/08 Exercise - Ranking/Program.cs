//8 Exercise - Ranking
Dictionary<string, string> contests = new();
Dictionary<string, Dictionary<string, int>> submissions = new();
string contestsInfo = Console.ReadLine();

while (contestsInfo != "end of contests")
{
    string[] addContest = contestsInfo
        .Split(":", StringSplitOptions.RemoveEmptyEntries);
    string currentContest = addContest[0];
    string password = addContest[1];

    if (!contests.ContainsKey(currentContest))
    {
        contests.Add(currentContest, password);
    }

    contestsInfo = Console.ReadLine();
}

string submissionInfo = Console.ReadLine();

while (submissionInfo != "end of submissions")
{
    string[] addSubmission = submissionInfo
        .Split("=>", StringSplitOptions.RemoveEmptyEntries);
    string currentConstest = addSubmission[0];
    string passwordConstest = addSubmission[1];
    string currentUser = addSubmission[2];
    int points = int.Parse(addSubmission[3]);

    if (contests.ContainsKey(currentConstest))
    {
        if (contests[currentConstest] == passwordConstest)
        {
            if (!submissions.ContainsKey(currentUser))
            {
                submissions.Add(currentUser, new Dictionary<string, int>());
                submissions[currentUser].Add(currentConstest, points);
            }

            else if (!submissions[currentUser].ContainsKey(currentConstest))
            {
                submissions[currentUser].Add(currentConstest, points);
            }

            else
            {
                if (submissions[currentUser][currentConstest] < points)
                {
                    submissions[currentUser][currentConstest] = points;
                }

            }
        }
    }

    submissionInfo = Console.ReadLine();
}

submissions = submissions.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

string bestCandidate = submissions.MaxBy(c => c.Value.Values.Sum()).Key;
int bestCandidateTotalPoints = submissions[bestCandidate].Values.Sum();

Console.WriteLine($"Best candidate is {bestCandidate} with total {bestCandidateTotalPoints} points.");
Console.WriteLine("Ranking: ");

foreach (var (name, allContests) in submissions)
{
    Console.WriteLine(name);
    Dictionary<string, int> orderedContests = allContests
        .OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

    foreach (var (currentContest, points) in orderedContests)
    {
        Console.WriteLine($"#  {currentContest} -> {points}");
    }
}