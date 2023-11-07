//2 Exercise - Blind Man’s Buff
int[] size = Console.ReadLine()
    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

char[,] matrix = new char[size[0], size[1]];
int playerRow = 0;
int playerCol = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    char[] chars = Console.ReadLine()
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(char.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = chars[col];

        if (matrix[row, col] == 'B')
        {
            playerRow = row;
            playerCol = col;
        }
    }
}

int madeMoves = 0;
int touchedOponents = 0;
int currentPlayerRow = 0;
int currentPlayerCol = 0;
string direction = Console.ReadLine();

while (direction != "Finish")
{
    if (direction == "left")
    {
        currentPlayerCol = playerCol - 1;

        if (playerRow >= 0 && playerRow < matrix.GetLength(0) &&
             currentPlayerCol >= 0 && currentPlayerCol < matrix.GetLength(1))
        {
            if (matrix[playerRow, currentPlayerCol] == 'O')
            {
                direction = Console.ReadLine();
                continue;
            }

            else if (matrix[playerRow, currentPlayerCol] == '-')
            {
                madeMoves++;
                matrix[playerRow, playerCol] = '-';
                playerCol = currentPlayerCol;
                matrix[playerRow, currentPlayerCol] = 'B';
            }

            else if (matrix[playerRow, currentPlayerCol] == 'P')
            {
                madeMoves++;
                touchedOponents++;
                matrix[playerRow, playerCol] = '-';
                playerCol = currentPlayerCol;
                matrix[playerRow, currentPlayerCol] = 'B';
            }
        }

        else
        {
            direction = Console.ReadLine();
            continue;
        }
    }

    else if (direction == "right")
    {
        currentPlayerCol = playerCol + 1;

        if (playerRow >= 0 && playerRow < matrix.GetLength(0) &&
             currentPlayerCol >= 0 && currentPlayerCol < matrix.GetLength(1))
        {
            if (matrix[playerRow, currentPlayerCol] == 'O')
            {
                direction = Console.ReadLine();
                continue;
            }

            else if (matrix[playerRow, currentPlayerCol] == '-')
            {
                madeMoves++;
                matrix[playerRow, playerCol] = '-';
                playerCol = currentPlayerCol;
                matrix[playerRow, currentPlayerCol] = 'B';
            }

            else if (matrix[playerRow, currentPlayerCol] == 'P')
            {
                madeMoves++;
                touchedOponents++;
                matrix[playerRow, playerCol] = '-';
                playerCol = currentPlayerCol;
                matrix[playerRow, currentPlayerCol] = 'B';
            }
        }

        else
        {
            direction = Console.ReadLine();
            continue;
        }
    }

    else if (direction == "up")
    {
        currentPlayerRow = playerRow - 1;

        if (currentPlayerRow >= 0 && currentPlayerRow < matrix.GetLength(0) &&
             playerCol >= 0 && playerCol < matrix.GetLength(1))
        {
            if (matrix[currentPlayerRow, playerCol] == 'O')
            {
                direction = Console.ReadLine();
                continue;
            }

            else if (matrix[currentPlayerRow, playerCol] == '-')
            {
                madeMoves++;
                matrix[playerRow, playerCol] = '-';
                playerRow = currentPlayerRow;
                matrix[currentPlayerRow, playerCol] = 'B';
            }

            else if (matrix[currentPlayerRow, playerCol] == 'P')
            {
                madeMoves++;
                touchedOponents++;
                matrix[playerRow, playerCol] = '-';
                playerRow = currentPlayerRow;
                matrix[currentPlayerRow, playerCol] = 'B';
            }
        }

        else
        {
            direction = Console.ReadLine();
            continue;
        }
    }

    else if (direction == "down")
    {
        currentPlayerRow = playerRow + 1;

        if (currentPlayerRow >= 0 && currentPlayerRow < matrix.GetLength(0) &&
             playerCol >= 0 && playerCol < matrix.GetLength(1))
        {
            if (matrix[currentPlayerRow, playerCol] == 'O')
            {
                direction = Console.ReadLine();
                continue;
            }

            else if (matrix[currentPlayerRow, playerCol] == '-')
            {
                madeMoves++;
                matrix[playerRow, playerCol] = '-';
                playerRow = currentPlayerRow;
                matrix[currentPlayerRow, playerCol] = 'B';
            }

            else if (matrix[currentPlayerRow, playerCol] == 'P')
            {
                madeMoves++;
                touchedOponents++;
                matrix[playerRow, playerCol] = '-';
                playerRow = currentPlayerRow;
                matrix[currentPlayerRow, playerCol] = 'B';
            }
        }

        else
        {
            direction = Console.ReadLine();
            continue;
        }
    }

    if (touchedOponents == 3)
    {
        break;
    }

    direction = Console.ReadLine();
}

Console.WriteLine("Game over!");
Console.WriteLine($"Touched opponents: {touchedOponents} Moves made: {madeMoves}");