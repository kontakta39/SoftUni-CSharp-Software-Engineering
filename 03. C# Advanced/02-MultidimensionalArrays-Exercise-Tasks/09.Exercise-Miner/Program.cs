//9 Exercise - Miner
int size = int.Parse(Console.ReadLine());
string[] directions = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries);
char[,] matrix = new char[size, size];
int minerRow = 0;
int minerCol = 0;
int coalsCount = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    char[] input = Console.ReadLine()
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(char.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = input[col];

        if (matrix[row, col] == 's')
        {
            minerRow = row;
            minerCol = col;
        }

        if (matrix[row, col] == 'c')
        {
            coalsCount++;
        }
    }
}

int currentMinerRow = 0;
int currentMinerCol = 0;

foreach (string direction in directions)
{
    if (direction == "left")
    {
        currentMinerCol = minerCol - 1;

        if (minerRow >= 0 && minerRow < matrix.GetLength(0) &&
             currentMinerCol >= 0 && currentMinerCol < matrix.GetLength(1))
        {
            if (matrix[minerRow, currentMinerCol] == '*')
            {
                matrix[minerRow, minerCol] = '*';
                minerCol = currentMinerCol;
                matrix[minerRow, currentMinerCol] = 's';
            }

            else if (matrix[minerRow, currentMinerCol] == 'e')
            {
                matrix[minerRow, minerCol] = '*';
                minerCol = currentMinerCol;
                matrix[minerRow, currentMinerCol] = 's';
                Console.WriteLine($"Game over! ({minerRow}, {currentMinerCol})");
                Environment.Exit(0);
            }

            else if (matrix[minerRow, currentMinerCol] == 'c')
            {
                coalsCount--;
                matrix[minerRow, minerCol] = '*';
                minerCol = currentMinerCol;
                matrix[minerRow, currentMinerCol] = 's';

                if (coalsCount == 0)
                {
                    break;
                }
            }
        }

        else
        {
            continue;
        }
    }

    else if (direction == "right")
    {
        currentMinerCol = minerCol + 1;

        if (minerRow >= 0 && minerRow < matrix.GetLength(0) &&
             currentMinerCol >= 0 && currentMinerCol < matrix.GetLength(1))
        {
            if (matrix[minerRow, currentMinerCol] == '*')
            {
                matrix[minerRow, minerCol] = '*';
                minerCol = currentMinerCol;
                matrix[minerRow, currentMinerCol] = 's';
            }

            else if (matrix[minerRow, currentMinerCol] == 'e')
            {
                matrix[minerRow, minerCol] = '*';
                minerCol = currentMinerCol;
                matrix[minerRow, currentMinerCol] = 's';
                Console.WriteLine($"Game over! ({minerRow}, {currentMinerCol})");
                Environment.Exit(0);
            }

            else if (matrix[minerRow, currentMinerCol] == 'c')
            {
                coalsCount--;
                matrix[minerRow, minerCol] = '*';
                minerCol = currentMinerCol;
                matrix[minerRow, currentMinerCol] = 's';

                if (coalsCount == 0)
                {
                    break;
                }
            }
        }

        else
        {
            continue;
        }
    }

    else if (direction == "up")
    {
        currentMinerRow = minerRow - 1;

        if (currentMinerRow >= 0 && currentMinerRow < matrix.GetLength(0) &&
             minerCol >= 0 && minerCol < matrix.GetLength(1))
        {
            if (matrix[currentMinerRow, minerCol] == '*')
            {
                matrix[minerRow, minerCol] = '*';
                minerRow = currentMinerRow;
                matrix[currentMinerRow, minerCol] = 's';
            }

            else if (matrix[currentMinerRow, minerCol] == 'e')
            {
                matrix[minerRow, minerCol] = '*';
                minerRow = currentMinerRow;
                matrix[currentMinerRow, minerCol] = 's';
                Console.WriteLine($"Game over! ({currentMinerRow}, {minerCol})");
                Environment.Exit(0);
            }

            else if (matrix[currentMinerRow, minerCol] == 'c')
            {
                coalsCount--;
                matrix[minerRow, minerCol] = '*';
                minerRow = currentMinerRow;
                matrix[currentMinerRow, minerCol] = 's';

                if (coalsCount == 0)
                {
                    break;
                }
            }
        }

        else
        {
            continue;
        }
    }

    else if (direction == "down")
    {
        currentMinerRow = minerRow + 1;

        if (currentMinerRow >= 0 && currentMinerRow < matrix.GetLength(0) &&
             minerCol >= 0 && minerCol < matrix.GetLength(1))
        {
            if (matrix[currentMinerRow, minerCol] == '*')
            {
                matrix[minerRow, minerCol] = '*';
                minerRow = currentMinerRow;
                matrix[currentMinerRow, minerCol] = 's';
            }

            else if (matrix[currentMinerRow, minerCol] == 'e')
            {
                matrix[minerRow, minerCol] = '*';
                minerRow = currentMinerRow;
                matrix[currentMinerRow, minerCol] = 's';
                Console.WriteLine($"Game over! ({currentMinerRow}, {minerCol})");
                Environment.Exit(0);
            }

            else if (matrix[currentMinerRow, minerCol] == 'c')
            {
                coalsCount--;
                matrix[minerRow, minerCol] = '*';
                minerRow = currentMinerRow;
                matrix[currentMinerRow, minerCol] = 's';

                if (coalsCount == 0)
                {
                    break;
                }
            }
        }

        else
        {
            continue;
        }
    }
}

if (coalsCount == 0)
{
    Console.WriteLine($"You collected all coals! ({minerRow}, {minerCol})");
}

else if (coalsCount > 0)
{
    Console.WriteLine($"{coalsCount} coals left. ({minerRow}, {minerCol})");
}