//9 Exercise - SoftUni Exam Results
Dictionary<string, int> usernames = new();
Dictionary<string, int> submissions = new();
string input = Console.ReadLine();

while (input != "exam finished")
{
    string[] usernameInfo = input.Split("-", StringSplitOptions.RemoveEmptyEntries);
    string currentUser = usernameInfo[0];
    string subject = usernameInfo[1];

    if (subject != "banned")
    {
        int points = int.Parse(usernameInfo[2]);

        if (!usernames.ContainsKey(currentUser))
        {
            usernames.Add(currentUser, points);
        }

        else
        {
            if (usernames[currentUser] < points)
            {
                usernames[currentUser] = points;
            }
        }

        if (!submissions.ContainsKey(subject))
        {
            submissions.Add(subject, 1);
        }

        else
        {
            submissions[subject]++;
        }
    }

    else
    {
        if (subject == "banned")
        {
            usernames.Remove(currentUser);
        }
    }

    input = Console.ReadLine();
}

usernames = usernames.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
submissions = submissions.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

Console.WriteLine("Results:");

foreach (var (username, points) in usernames)
{
    Console.WriteLine($"{username} | {points}");
}

Console.WriteLine("Submissions:");

foreach (var (subject, count) in submissions)
{
    Console.WriteLine($"{subject} - {count}");
}
