//10 Exercise - ForceBook
Dictionary<string, HashSet<string>> forceUsers = new();
string input = Console.ReadLine();

while (input != "Lumpawaroo")
{
    string[] delimiters = { " | ", " -> " };
    string[] forceUserInfo = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    string firstInput = forceUserInfo[0];   
    string secondInput = forceUserInfo[1];

    if (firstInput == "Light" || firstInput == "Lighter" || 
        firstInput == "Dark" || firstInput == "Darker")
    {
        if (!forceUsers.ContainsKey(firstInput))
        {
            forceUsers.Add(firstInput, new HashSet<string>());
            forceUsers[firstInput].Add(secondInput);
        }
    }

    else if (secondInput == "Light" || secondInput == "Lighter" || 
        secondInput == "Dark" || secondInput == "Darker")
    {
        string otherSide = string.Empty;

        switch (secondInput)
        {
            case "Light":
                otherSide = "Dark";
                break;
            case "Dark":
                otherSide = "Light";
                break;
            case "Lighter":
                otherSide = "Darker";
                break;
            case "Darker":
                otherSide = "Lighter";
                break;
        }

        if (!forceUsers[secondInput].Contains(firstInput))
        {
            forceUsers[secondInput].Add(firstInput);
            Console.WriteLine($"{firstInput} joins the {secondInput} side!");
        }

        if (forceUsers[otherSide].Contains(firstInput))
        {
            forceUsers[otherSide].Remove(firstInput);
        }
    }

    input = Console.ReadLine();
}

forceUsers = forceUsers.OrderByDescending(x => x.Value.Count()).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

foreach (var (side, forceUserList) in forceUsers)
{
    if (forceUserList.Count == 0)
    {
        continue;
    }

    int usersCount = forceUserList.Count;
    Console.WriteLine($"Side: {side}, Members: {usersCount}");

    HashSet<string> orderedForceUsers = forceUserList.OrderBy(x => x).ToHashSet();

    foreach (var currentForceUser in orderedForceUsers)
    {
        Console.WriteLine($"! {currentForceUser}");
    }
}
