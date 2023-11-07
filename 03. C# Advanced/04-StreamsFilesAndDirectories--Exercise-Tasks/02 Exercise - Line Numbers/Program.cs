//2 Exercise - Line Numbers
using System.Text;

namespace LineNumbers;

public class LineNumbers
{
    static void Main()
    {
        string inputFilePath = @"..\..\..\text.txt";
        string outputFilePath = @"..\..\..\output.txt";

        ProcessLines(inputFilePath, outputFilePath);
    }

    public static void ProcessLines(string inputFilePath, string outputFilePath)
    {
        string[] lines = File.ReadAllLines(inputFilePath);
        StringBuilder sb = new();
        int lettersCount = 0;
        int marksCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            lettersCount = lines[i].Count(char.IsLetter);
            marksCount = lines[i].Count(char.IsPunctuation);

            sb.AppendLine($"Line {i + 1}: {lines[i]} ({lettersCount})({marksCount})");
        }

        File.WriteAllText(outputFilePath, sb.ToString());
    }
}