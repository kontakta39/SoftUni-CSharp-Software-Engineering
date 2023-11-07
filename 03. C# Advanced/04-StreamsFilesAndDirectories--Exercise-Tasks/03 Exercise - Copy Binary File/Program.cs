//3 Exercise - Copy Binary File
namespace CopyBinaryFile;

public class CopyBinaryFile
{
    static void Main()
    {
        string inputFilePath = @"..\..\..\copyMe.png";
        string outputFilePath = @"..\..\..\copyMe-copy.png";

        CopyFile(inputFilePath, outputFilePath);
    }

    public static void CopyFile(string inputFilePath, string outputFilePath)
    {
        using FileStream reader = new(inputFilePath, FileMode.Open);
        using FileStream writer = new(outputFilePath, FileMode.Create);
        byte[] buffer = new byte[1024];
        int size = 0;

        while (true)
        {
            size = reader.Read(buffer, 0, buffer.Length);

            if (size != 0)
            {
                writer.Write(buffer, 0, buffer.Length);
            }

            else
            {
                break;
            }
        }
    }
}
