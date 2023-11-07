//4 Exercise - Directory Traversal
using System.IO.Enumeration;
using System.Text;

namespace DirectoryTraversal;

public class DirectoryTraversal
{
    static void Main()
    {
        string path = @"D:\HP\User\Изтеглени файлове";
        string reportFileName = @"\report.txt";

        string reportContent = TraverseDirectory(path);
        Console.WriteLine(reportContent);

        WriteReportToDesktop(reportContent, reportFileName);
    }

    public static string TraverseDirectory(string inputFolderPath)
    {
        string[] extensions = Directory.GetDirectories(inputFolderPath);
        SortedDictionary<string, List<FileInfo>> extensionFiles = new();

        foreach (var item in extensions)
        {
            FileInfo fileInfo = new(item);

            if (!extensionFiles.ContainsKey(fileInfo.Extension))
            {
                extensionFiles.Add(fileInfo.Extension, new List<FileInfo>());
            }

            extensionFiles[fileInfo.Extension].Add(fileInfo);
        }

        StringBuilder sb = new();

        foreach (var item in extensionFiles.OrderByDescending(x => x.Value.Count))
        {
            sb.AppendLine(item.Key);

            foreach (var file in item.Value.OrderBy(x => x.Length))
            {
                sb.AppendLine($"--{file.Name} - {(double)file.Length / 1024:f3}kb");
            }
        }
;
        return sb.ToString();
    }

    public static void WriteReportToDesktop(string textContent, string reportFileName)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)) + reportFileName;

        File.WriteAllText(filePath, textContent);
    }
}
