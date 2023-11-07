//1 Exercise - Sum Matrix Elements
int[] dimensionsInfo = Console.ReadLine()
    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int sumAllCells = 0;
int rowsNumber = dimensionsInfo[0];
int colsNumber = dimensionsInfo[1];
int[,] matrix = new int[rowsNumber, colsNumber];

for (int row = 0; row < rowsNumber; row++)
{
    int[] colElements = Console.ReadLine()
        .Split(", ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    for (int col = 0; col < colsNumber; col++)
    {
        matrix[row, col] = colElements[col];
    }
}

for (int row = 0; row < rowsNumber; row++)
{
    for (int col = 0; col < colsNumber; col++)
    {
        sumAllCells += matrix[row, col];
    }
}

Console.WriteLine(matrix.GetLength(0));
Console.WriteLine(matrix.GetLength(1));
Console.WriteLine(sumAllCells);