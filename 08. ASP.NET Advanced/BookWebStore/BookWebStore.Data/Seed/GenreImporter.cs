using BookWebStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookWebStore.Data;

public static class GenreImporter
{
    public static async Task ImportGenresFromJsonAsync(string filePath, IServiceProvider serviceProvider)
    {
        List<Genre>? genres = await JsonFileLoader<Genre>.LoadFromFileAsync(filePath);

        if (genres.Count == 0)
        {
            Console.WriteLine("There are no genres for importing.");
            return;
        }

        using IServiceScope? scope = serviceProvider.CreateScope();
        BookStoreDbContext? context = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

        foreach (var genre in genres)
        {
            bool genreExists = await context.Genres
                .AnyAsync(g => g.Name.ToLower() == genre.Name.ToLower() && !g.IsDeleted);

            if (!genreExists)
            {
                await context.Genres.AddAsync(genre);
            }
        }

        await context.SaveChangesAsync();
        Console.WriteLine("Genres are imported successfully.");
    }
}