//1 Exercise - Diagonal Difference
int dimensions = int.Parse(Console.ReadLine());
int sumFirstDiagonalCells = 0;
int sumSecondDiagonalCells = 0;
int[,] matrix = new int[dimensions, dimensions];

for (int row = 0; row < matrix.GetLength(0); row++)
{
    int[] colElements = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = colElements[col];
    }
}

for (int i = 0; i < dimensions; i++)
{
    sumFirstDiagonalCells += matrix[i, i];
    sumSecondDiagonalCells += matrix[i, dimensions - i - 1];
}

int difference = (sumFirstDiagonalCells - sumSecondDiagonalCells);
Console.WriteLine(Math.Abs(difference));