using System.Text.Json;

namespace BookWebStore.Data;

public static class JsonFileLoader<T>
{
    public static async Task<List<T>> LoadFromFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"JSON file not found: {filePath}");
            return new List<T>();
        }

        string jsonContent = await File.ReadAllTextAsync(filePath);

        List<T>? items = JsonSerializer.Deserialize<List<T>>(jsonContent);

        return items ?? new List<T>();
    }
}
