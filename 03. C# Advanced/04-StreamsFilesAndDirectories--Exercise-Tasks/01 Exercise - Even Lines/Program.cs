//1 Exercise - Even Lines
using System.Text;

namespace EvenLines;

public class EvenLines
{
    static void Main()
    {
        string inputFilePath = @"..\..\..\text.txt";

        Console.WriteLine(ProcessLines(inputFilePath));
    }

    public static string ProcessLines(string inputFilePath)
    {
        string line = string.Empty;
        using StreamReader streamReader = new(inputFilePath);
        List<string> lines = new();
        int count = 0;
        line = streamReader.ReadLine();

        while (line != null)
        {
            char[] chars = { '-', ',', '.', '!', '?' };

            foreach (var currentChar in chars)
            {
                line = line.Replace(currentChar, '@');
            }

            if (count % 2 == 0)
            {
                lines.Add(line);
            }

            count++;
            line = streamReader.ReadLine();
        }

        for (int i = 0; i < lines.Count; i++)
        {
            string[] stringArray = lines[i]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(stringArray);
            string currentText = string.Empty;

            foreach (var item in stringArray)
            {
                currentText += item;
                currentText += " ";
            }

            lines[i] = currentText;
        }
;
        return string.Join(Environment.NewLine, lines);
    }
}
