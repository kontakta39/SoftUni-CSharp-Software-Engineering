//4 Exercise - Matrix Shuffling
int[] dimensionsInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int rowsNumber = dimensionsInfo[0];
int columnsNumber = dimensionsInfo[1];
string[,] matrix = new string[rowsNumber, columnsNumber];

for (int row = 0; row < matrix.GetLength(0); row++)
{
    string[] colNumbers = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries);

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = colNumbers[col];
    }
}

string input = Console.ReadLine();

while (input != "END")
{
    string[] operationInfo = input
     .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string operation = operationInfo[0];

    if (operation == "swap" && operationInfo.Length == 5)
    {
        int firstRowNumber = int.Parse(operationInfo[1]);
        int firstColumnNumber = int.Parse(operationInfo[2]);
        int secondRowNumber = int.Parse(operationInfo[3]);
        int SecondColumnNumber = int.Parse(operationInfo[4]);

        if (firstRowNumber >= 0 && firstRowNumber < matrix.GetLength(0) &&
            firstColumnNumber >= 0 && firstColumnNumber < matrix.GetLength(1) &&
            secondRowNumber >= 0 && secondRowNumber < matrix.GetLength(0) &&
            SecondColumnNumber >= 0 && SecondColumnNumber < matrix.GetLength(1))
        {
            string temp = string.Empty;
            temp = matrix[firstRowNumber, firstColumnNumber];
            matrix[firstRowNumber, firstColumnNumber] = matrix[secondRowNumber, SecondColumnNumber];
            matrix[secondRowNumber, SecondColumnNumber] = temp;

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write(matrix[row, col] + " ");
                }

                Console.WriteLine();
            }
        }

        else
        {
            Console.WriteLine("Invalid input!");
        }
    }

    else
    {
        Console.WriteLine("Invalid input!");
    }

    input = Console.ReadLine();
}