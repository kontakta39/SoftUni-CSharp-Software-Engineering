//6 Exercise - Money Transactions
Dictionary<int, double> bankAccounts = new();
string[] inputInfo = Console.ReadLine().Split(",");

for (int i = 0; i < inputInfo.Length; i++)
{
    string[] currentPair = inputInfo[i].Split("-");
    bankAccounts.Add(int.Parse(currentPair[0]), double.Parse(currentPair[1]));
}

string command =  Console.ReadLine();

while (command != "End")
{
    string[] commandInfo = command.Split();

    try
    {
        if (commandInfo[0] == "Deposit")
        {
            try
            {
                int currentAccount = int.Parse(commandInfo[1]);
                double currentBalance = double.Parse(commandInfo[2]);

                if (!bankAccounts.ContainsKey(currentAccount))
                {
                    throw new FormatException();
                }

                bankAccounts[currentAccount] += currentBalance;
                Console.WriteLine($"Account {currentAccount} has new balance: {bankAccounts[currentAccount]:f2}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid account!");
            }
        }

        else if (commandInfo[0] == "Withdraw")
        {
            try
            {
                int currentAccount = int.Parse(commandInfo[1]);
                double currentBalance = double.Parse(commandInfo[2]);

                if (currentBalance > bankAccounts[currentAccount])
                { 
                throw new ArgumentOutOfRangeException();
                }

                bankAccounts[currentAccount] -= currentBalance;
                Console.WriteLine($"Account {currentAccount} has new balance: {bankAccounts[currentAccount]:f2}");

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid account!");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Insufficient balance!");
            }
        }

        else
        {
            throw new FormatException();
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid command!");
    }
    finally 
    {
        Console.WriteLine("Enter another command");
    }

    command = Console.ReadLine();
}