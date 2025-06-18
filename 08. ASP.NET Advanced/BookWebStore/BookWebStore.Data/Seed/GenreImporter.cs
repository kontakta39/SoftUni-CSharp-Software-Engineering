using System.Text.Json;
using BookWebStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookWebStore.Data;

public static class GenreImporter
{
    public static async Task ImportGenresFromJsonAsync(string filePath, IServiceProvider serviceProvider)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("JSON file not found: " + filePath);
            return;
        }

        string jsonContent = await File.ReadAllTextAsync(filePath);

        List<Genre>? genres = JsonSerializer.Deserialize<List<Genre>>(jsonContent);

        if (genres == null || genres.Count == 0)
        {
            Console.WriteLine("There are no genres for importing.");
            return;
        }

        using IServiceScope? scope = serviceProvider.CreateScope();
        BookStoreDbContext? context = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

        foreach (Genre genre in genres)
        {
            bool genreExists = await context.Genres
                .AnyAsync(g => g.Name.ToLower() == genre.Name.ToLower() && !g.IsDeleted);

            if (!genreExists)
            {
                if (genre.Id == Guid.Empty)
                {
                    genre.Id = Guid.NewGuid();
                }

                context.Genres.Add(genre);
            }
        }

        await context.SaveChangesAsync();
        Console.WriteLine("Genres are imported successfully.");
    }
}
