//2 Exercise - The Squirrel
int size = int.Parse(Console.ReadLine());
string[] directions = Console.ReadLine()
    .Split(", ", StringSplitOptions.RemoveEmptyEntries);
char[,] matrix = new char[size, size];
int squirrelRow = 0;
int squirrelCol = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    string input = Console.ReadLine();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = input[col];

        if (matrix[row, col] == 's')
        {
            squirrelRow = row;
            squirrelCol = col;
        }
    }
}

int hazelnutsCount = 0;
int currentSquirrelRow = 0;
int currentSquirrelCol = 0;

foreach (string direction in directions)
{
    if (direction == "left")
    {
        currentSquirrelCol = squirrelCol - 1;

        if (squirrelRow >= 0 && squirrelRow < matrix.GetLength(0) &&
             currentSquirrelCol >= 0 && currentSquirrelCol < matrix.GetLength(1))
        {
            if (matrix[squirrelRow, currentSquirrelCol] == '*')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelCol = currentSquirrelCol;
                matrix[squirrelRow, currentSquirrelCol] = 's';
            }

            else if (matrix[squirrelRow, currentSquirrelCol] == 't')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                Console.WriteLine("Unfortunately, the squirrel stepped on a trap...");
                Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
                Environment.Exit(0);
            }

            else if (matrix[squirrelRow, currentSquirrelCol] == 'h')
            {
                hazelnutsCount++;
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelCol = currentSquirrelCol;
                matrix[squirrelRow, currentSquirrelCol] = 's';
            }
        }

        else
        {
            matrix[squirrelRow, squirrelCol] = '*';
            Console.WriteLine("The squirrel is out of the field.");
            Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
            Environment.Exit(0);
        }
    }

    else if (direction == "right")
    {
        currentSquirrelCol = squirrelCol + 1;

        if (squirrelRow >= 0 && squirrelRow < matrix.GetLength(0) &&
             currentSquirrelCol >= 0 && currentSquirrelCol < matrix.GetLength(1))
        {
            if (matrix[squirrelRow, currentSquirrelCol] == '*')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelCol = currentSquirrelCol;
                matrix[squirrelRow, currentSquirrelCol] = 's';
            }

            else if (matrix[squirrelRow, currentSquirrelCol] == 't')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                Console.WriteLine("Unfortunately, the squirrel stepped on a trap...");
                Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
                Environment.Exit(0);
            }

            else if (matrix[squirrelRow, currentSquirrelCol] == 'h')
            {
                hazelnutsCount++;
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelCol = currentSquirrelCol;
                matrix[squirrelRow, currentSquirrelCol] = 's';
            }
        }

        else
        {
            matrix[squirrelRow, squirrelCol] = '*';
            Console.WriteLine("The squirrel is out of the field.");
            Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
            Environment.Exit(0);
        }
    }

    else if (direction == "up")
    {
        currentSquirrelRow = squirrelRow - 1;

        if (currentSquirrelRow >= 0 && currentSquirrelRow < matrix.GetLength(0) &&
             squirrelCol >= 0 && squirrelCol < matrix.GetLength(1))
        {
            if (matrix[currentSquirrelRow, squirrelCol] == '*')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelRow = currentSquirrelRow;
                matrix[currentSquirrelRow, squirrelCol] = 's';
            }

            else if (matrix[currentSquirrelRow, squirrelCol] == 't')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                Console.WriteLine("Unfortunately, the squirrel stepped on a trap...");
                Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
                Environment.Exit(0);
            }

            else if (matrix[currentSquirrelRow, squirrelCol] == 'h')
            {
                hazelnutsCount++;
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelRow = currentSquirrelRow;
                matrix[currentSquirrelRow, squirrelCol] = 's';
            }
        }

        else
        {
            matrix[squirrelRow, squirrelCol] = '*';
            Console.WriteLine("The squirrel is out of the field.");
            Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
            Environment.Exit(0);
        }
    }

    else if (direction == "down")
    {
        currentSquirrelRow = squirrelRow + 1;

        if (currentSquirrelRow >= 0 && currentSquirrelRow < matrix.GetLength(0) &&
             squirrelCol >= 0 && squirrelCol < matrix.GetLength(1))
        {
            if (matrix[currentSquirrelRow, squirrelCol] == '*')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelRow = currentSquirrelRow;
                matrix[currentSquirrelRow, squirrelCol] = 's';
            }

            else if (matrix[currentSquirrelRow, squirrelCol] == 't')
            {
                matrix[squirrelRow, squirrelCol] = '*';
                Console.WriteLine("Unfortunately, the squirrel stepped on a trap...");
                Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
                Environment.Exit(0);
            }

            else if (matrix[currentSquirrelRow, squirrelCol] == 'h')
            {
                hazelnutsCount++;
                matrix[squirrelRow, squirrelCol] = '*';
                squirrelRow = currentSquirrelRow;
                matrix[currentSquirrelRow, squirrelCol] = 's';
            }
        }

        else
        {
            matrix[squirrelRow, squirrelCol] = '*';
            Console.WriteLine("The squirrel is out of the field.");
            Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
            Environment.Exit(0);
        }
    }
}

if (hazelnutsCount == 3)
{
    Console.WriteLine("Good job! You have collected all hazelnuts!");
    Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
}

else if (hazelnutsCount < 3)
{
    Console.WriteLine("There are more hazelnuts to collect.");
    Console.WriteLine($"Hazelnuts collected: {hazelnutsCount}");
}