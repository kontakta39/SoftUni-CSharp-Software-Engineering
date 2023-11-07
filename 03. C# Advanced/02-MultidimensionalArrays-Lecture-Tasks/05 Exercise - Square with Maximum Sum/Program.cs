//5 Exercise - Square with Maximum Sum
int[] diminesionsSize = Console.ReadLine()
	.Split(", ", StringSplitOptions.RemoveEmptyEntries)
	.Select(int.Parse)
	.ToArray();
int rowsSize = diminesionsSize[0];
int columnsSize = diminesionsSize[1];
int[,] matrix = new int[rowsSize, columnsSize];

for (int row = 0; row < matrix.GetLength(0); row++)
{
	int[] rowElements = Console.ReadLine()
		.Split(", ", StringSplitOptions.RemoveEmptyEntries)
		.Select(int.Parse)
		.ToArray();

 	for (int col = 0; col < matrix.GetLength(1); col++)
	{
		matrix[row, col] = rowElements[col];
	}
}

int maxSum = int.MinValue;
int currentSum = 0;
int firstElementRow = 0;
int firstElementCol = 0;

for (int row = 0; row < matrix.GetLength(0) - 1; row++)
{
	for (int col = 0; col < matrix.GetLength(1) - 1; col++)
	{
		currentSum = matrix[row, col] + matrix[row, col + 1] +
			 matrix[row + 1, col] + matrix[row + 1, col + 1];

		if (maxSum < currentSum)
		{
			maxSum = currentSum;
			firstElementRow = row;
			firstElementCol = col;
		}
    }

}

bool isFound = false;

for (int row = 0; row < matrix.GetLength(0) - 1; row++)
{
    for (int col = 0; col < matrix.GetLength(1) - 1; col++)
    {
        currentSum = matrix[row, col] + matrix[row, col + 1] +
             matrix[row + 1, col] + matrix[row + 1, col + 1];

        if (row == firstElementRow && col == firstElementCol)
        {
			Console.WriteLine($"{matrix[row, col]} {matrix[row, col + 1]}");
            Console.WriteLine($"{matrix[row + 1, col]} {matrix[row + 1, col + 1]}");
			isFound = true;
			break;
        }
    }

	if (isFound)
	{
		break;
	}
}

Console.WriteLine(maxSum);
