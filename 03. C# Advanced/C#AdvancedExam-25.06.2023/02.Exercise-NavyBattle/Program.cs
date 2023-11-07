//2 Exercise - Navy Battle
int size = int.Parse(Console.ReadLine());
char[,] matrix = new char[size, size];
int hitsCount = 3;
int battlesWinCount = 0;
int submarineRowPosition = 0;
int submarineColumnPosition = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
	string input = Console.ReadLine();

	for (int col = 0; col < matrix.GetLength(1); col++)
	{
		matrix[row, col] = input[col];

		if (matrix[row, col] == 'S')
		{
			submarineRowPosition = row;
			submarineColumnPosition = col;
		}
	}
}

while (true)
{
	string command = Console.ReadLine();

    if (command == "left")
    {
        int currentColumnPositon = submarineColumnPosition - 1;

        if (matrix[submarineRowPosition, currentColumnPositon] == '-')
        {
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineColumnPosition = currentColumnPositon;
            matrix[submarineRowPosition, currentColumnPositon] = 'S';
        }

        else if (matrix[submarineRowPosition, currentColumnPositon] == '*')
        {
            hitsCount--;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineColumnPosition = currentColumnPositon;
            matrix[submarineRowPosition, currentColumnPositon] = 'S';

            if (hitsCount == 0)
            {
                Console.WriteLine($"Mission failed, U-9 disappeared! Last known coordinates [{submarineRowPosition}, {currentColumnPositon}]!");
                break;
            }
        }

        else if (matrix[submarineRowPosition, currentColumnPositon] == 'C')
        {
            battlesWinCount++;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineColumnPosition = currentColumnPositon;
            matrix[submarineRowPosition, currentColumnPositon] = 'S';

            if (battlesWinCount == 3)
            {
                Console.WriteLine("Mission accomplished, U-9 has destroyed all battle cruisers of the enemy!");
                break;
            }
        }
    }

    if (command == "right")
    {
        int currentColumnPositon = submarineColumnPosition + 1;

        if (matrix[submarineRowPosition, currentColumnPositon] == '-')
        {
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineColumnPosition = currentColumnPositon;
            matrix[submarineRowPosition, currentColumnPositon] = 'S';
        }

        else if (matrix[submarineRowPosition, currentColumnPositon] == '*')
        {
            hitsCount--;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineColumnPosition = currentColumnPositon;
            matrix[submarineRowPosition, currentColumnPositon] = 'S';

            if (hitsCount == 0)
            {
                Console.WriteLine($"Mission failed, U-9 disappeared! Last known coordinates [{submarineRowPosition}, {currentColumnPositon}]!");
                break;
            }
        }

        else if (matrix[submarineRowPosition, currentColumnPositon] == 'C')
        {
            battlesWinCount++;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineColumnPosition = currentColumnPositon;
            matrix[submarineRowPosition, currentColumnPositon] = 'S';

            if (battlesWinCount == 3)
            {
                Console.WriteLine("Mission accomplished, U-9 has destroyed all battle cruisers of the enemy!");
                break;
            }
        }
    }

    else if (command == "up")
	{
		int currentRowPositon = submarineRowPosition - 1;

		if (matrix[currentRowPositon, submarineColumnPosition] == '-')
		{
			matrix[submarineRowPosition, submarineColumnPosition] = '-';
			submarineRowPosition = currentRowPositon;
			matrix[currentRowPositon, submarineColumnPosition] = 'S';
		}

		else if (matrix[currentRowPositon, submarineColumnPosition] == '*')
		{
			hitsCount--;
			matrix[submarineRowPosition, submarineColumnPosition] = '-';
			submarineRowPosition = currentRowPositon;
			matrix[currentRowPositon, submarineColumnPosition] = 'S';

			if (hitsCount == 0)
			{
				Console.WriteLine($"Mission failed, U-9 disappeared! Last known coordinates [{currentRowPositon}, {submarineColumnPosition}]!");
				break;
			}
		}

		else if (matrix[currentRowPositon, submarineColumnPosition] == 'C')
		{
			battlesWinCount++;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineRowPosition = currentRowPositon;
            matrix[currentRowPositon, submarineColumnPosition] = 'S';

			if (battlesWinCount == 3) 
			{
				Console.WriteLine("Mission accomplished, U-9 has destroyed all battle cruisers of the enemy!");
				break;
			}
        }
	}

    else if (command == "down")
    {
        int currentRowPositon = submarineRowPosition + 1;

        if (matrix[currentRowPositon, submarineColumnPosition] == '-')
        {
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineRowPosition = currentRowPositon;
            matrix[currentRowPositon, submarineColumnPosition] = 'S';
        }

        else if (matrix[currentRowPositon, submarineColumnPosition] == '*')
        {
            hitsCount--;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineRowPosition = currentRowPositon;
            matrix[currentRowPositon, submarineColumnPosition] = 'S';

            if (hitsCount == 0)
            {
                Console.WriteLine($"Mission failed, U-9 disappeared! Last known coordinates [{currentRowPositon}, {submarineColumnPosition}]!");
                break;
            }
        }

        else if (matrix[currentRowPositon, submarineColumnPosition] == 'C')
        {
            battlesWinCount++;
            matrix[submarineRowPosition, submarineColumnPosition] = '-';
            submarineRowPosition = currentRowPositon;
            matrix[currentRowPositon, submarineColumnPosition] = 'S';

            if (battlesWinCount == 3)
            {
                Console.WriteLine("Mission accomplished, U-9 has destroyed all battle cruisers of the enemy!");
                break;
            }
        }
    }
}

for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        Console.Write(matrix[row, col]);
    }

    Console.WriteLine();
}