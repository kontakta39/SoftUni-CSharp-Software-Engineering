namespace CopyDirectory;

public class CopyDirectory
{
    static void Main()
    {
        string inputPath = @$"{Console.ReadLine()}";
        string outputPath = @$"{Console.ReadLine()}";

        CopyAllFiles(inputPath, outputPath);
    }

    public static void CopyAllFiles(string inputPath, string outputPath)
    {
        var dir = new DirectoryInfo(inputPath);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }

        Directory.CreateDirectory(outputPath);

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(outputPath, file.Name);
            file.CopyTo(targetFilePath);
        }
    }
}
