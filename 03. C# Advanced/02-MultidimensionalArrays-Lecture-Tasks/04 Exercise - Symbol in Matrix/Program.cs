//4 Exercise - Symbol in Matrix
int dimensions = int.Parse(Console.ReadLine());
char[,] matrix = new char[dimensions, dimensions];

for (int row = 0; row < matrix.GetLength(0); row++)
{
    char[] colElements = Console.ReadLine().ToCharArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = colElements[col];
    }
}

char currentSymbol = char.Parse(Console.ReadLine());
bool ifExists = false;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        if (currentSymbol == matrix[row, col])
        {
            Console.WriteLine($"({row}, {col})");
            ifExists = true;
            break;
        }
    }

    if (ifExists)
    {
        break;
    }
}

if (ifExists == false)
{
    Console.WriteLine($"{currentSymbol} does not occur in the matrix");
}