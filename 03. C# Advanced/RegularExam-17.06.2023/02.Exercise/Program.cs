//2 Exercise
int[] size = Console.ReadLine()
    .Split(",", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
char[,] matrix = new char[size[0], size[1]];
int mouseRow = 0;
int mouseCol = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    string input = Console.ReadLine();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = input[col];

        if (matrix[row, col] == 'M')
        {
            mouseRow = row;
            mouseCol = col;
        }
    }
}

int cheeseCount = 0;
CheeseCountCheck();
int currentMouseRow = 0;
int currentMouseCol = 0;
string direction = Console.ReadLine();

while (direction != "danger")
{
    if (direction == "left")
    {
        currentMouseCol = mouseCol - 1;

        if (mouseRow >= 0 && mouseRow < matrix.GetLength(0) &&
             currentMouseCol >= 0 && currentMouseCol < matrix.GetLength(1))
        {
            if (matrix[mouseRow, currentMouseCol] == '*')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseCol = currentMouseCol;
                matrix[mouseRow, currentMouseCol] = 'M';
            }

            else if (matrix[mouseRow, currentMouseCol] == 'T')
            {
                matrix[mouseRow, mouseCol] = '*';
                matrix[mouseRow, currentMouseCol] = 'M';
                Console.WriteLine("Mouse is trapped!");
                MatrixPrint();
                Environment.Exit(0);
            }

            else if (matrix[mouseRow, currentMouseCol] == 'C')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseCol = currentMouseCol;
                matrix[mouseRow, currentMouseCol] = 'M';
                cheeseCount--;

                if (cheeseCount == 0)
                {
                    break;
                }
            }

            else if (matrix[mouseRow, currentMouseCol] == '@')
            {
                direction = Console.ReadLine();
                continue;
            }
        }

        else
        {
            Console.WriteLine("No more cheese for tonight!");
            MatrixPrint();
            Environment.Exit(0);
        }
    }

    else if (direction == "right")
    {
        currentMouseCol = mouseCol + 1;

        if (mouseRow >= 0 && mouseRow < matrix.GetLength(0) &&
             currentMouseCol >= 0 && currentMouseCol < matrix.GetLength(1))
        {
            if (matrix[mouseRow, currentMouseCol] == '*')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseCol = currentMouseCol;
                matrix[mouseRow, currentMouseCol] = 'M';
            }

            else if (matrix[mouseRow, currentMouseCol] == 'T')
            {
                matrix[mouseRow, mouseCol] = '*';
                matrix[mouseRow, currentMouseCol] = 'M';
                Console.WriteLine("Mouse is trapped!");
                MatrixPrint();
                Environment.Exit(0);
            }

            else if (matrix[mouseRow, currentMouseCol] == 'C')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseCol = currentMouseCol;
                matrix[mouseRow, currentMouseCol] = 'M';
                cheeseCount--;

                if (cheeseCount == 0) 
                {
                    break;
                }
            }

            else if (matrix[mouseRow, currentMouseCol] == '@')
            {
                direction = Console.ReadLine();
                continue;
            }
        }

        else
        {
            Console.WriteLine("No more cheese for tonight!");
            MatrixPrint();
            Environment.Exit(0);
        }
    }

    else if (direction == "up")
    {
        currentMouseRow = mouseRow - 1;

        if (currentMouseRow >= 0 && currentMouseRow < matrix.GetLength(0) &&
             mouseCol >= 0 && mouseCol < matrix.GetLength(1))
        {
            if (matrix[currentMouseRow, mouseCol] == '*')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseRow = currentMouseRow;
                matrix[currentMouseRow, mouseCol] = 'M';
            }

            else if (matrix[currentMouseRow, mouseCol] == 'T')
            {
                matrix[mouseRow, mouseCol] = '*';
                matrix[currentMouseRow, mouseCol] = 'M';
                Console.WriteLine("Mouse is trapped!");
                MatrixPrint();
                Environment.Exit(0);
            }

            else if (matrix[currentMouseRow, mouseCol] == 'C')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseRow = currentMouseRow;
                matrix[currentMouseRow, mouseCol] = 'M';
                cheeseCount--;

                if (cheeseCount == 0)
                {
                    break;
                }
            }

            else if (matrix[currentMouseRow, mouseCol] == '@')
            {
                direction = Console.ReadLine();
                continue;
            }
        }

        else
        {
            Console.WriteLine("No more cheese for tonight!");
            MatrixPrint();
            Environment.Exit(0);
        }
    }

    else if (direction == "down")
    {
        currentMouseRow = mouseRow + 1;

        if (currentMouseRow >= 0 && currentMouseRow < matrix.GetLength(0) &&
             mouseCol >= 0 && mouseCol < matrix.GetLength(1))
        {
            if (matrix[currentMouseRow, mouseCol] == '*')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseRow = currentMouseRow;
                matrix[currentMouseRow, mouseCol] = 'M';
            }

            else if (matrix[currentMouseRow, mouseCol] == 'T')
            {
                matrix[mouseRow, mouseCol] = '*';
                matrix[currentMouseRow, mouseCol] = 'M';
                Console.WriteLine("Mouse is trapped!");
                MatrixPrint();
                Environment.Exit(0);
            }

            else if (matrix[currentMouseRow, mouseCol] == 'C')
            {
                matrix[mouseRow, mouseCol] = '*';
                mouseRow = currentMouseRow;
                matrix[currentMouseRow, mouseCol] = 'M';
                cheeseCount--;

                if (cheeseCount == 0)
                {
                    break;
                }
            }

            else if (matrix[currentMouseRow, mouseCol] == '@')
            {
                direction = Console.ReadLine();
                continue;
            }
        }

        else
        {
            Console.WriteLine("No more cheese for tonight!");
            MatrixPrint();
            Environment.Exit(0);
        }
    }

    direction = Console.ReadLine();
}

if (cheeseCount > 0)
{
    Console.WriteLine("Mouse will come back later!");
    MatrixPrint();
}

else if (cheeseCount == 0)
{
    Console.WriteLine("Happy mouse! All the cheese is eaten, good night!");
    MatrixPrint();
}

void CheeseCountCheck()
{
    for (int row = 0; row < matrix.GetLength(0); row++)
    {
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            if (matrix[row, col] == 'C')
            {
                cheeseCount++;
            }
        }
    }
}

void MatrixPrint()
{
    for (int row = 0; row < matrix.GetLength(0); row++)
    {
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            Console.Write(matrix[row, col]);
        }

        Console.WriteLine();
    }
}