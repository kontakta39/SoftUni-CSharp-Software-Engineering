namespace Boardgames.DataProcessor;

using Boardgames.Data;
using Boardgames.Data.Models;
using Boardgames.DataProcessor.ExportDto;
using Boardgames.Utilities;
using Newtonsoft.Json;

public class Serializer
{
    public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
    {
        var xmlParser = new XmlParser();

        var creators = context.Creators
            .AsEnumerable()
            .Select(c => new ExportCreatorDto()
            {
                BoardgamesCount = c.Boardgames.Count(),
                CreatorName = $"{c.FirstName} {c.LastName}",
                Boardgames = c.Boardgames
                .OrderBy(c => c.Name)
                .Select(b => new ExportBoardgameDto() 
                {
                BoardgameName = b.Name,
                BoardgameYearPublished = b.YearPublished
                })
                .ToArray()
            })
            .OrderByDescending(c => c.BoardgamesCount)
            .ThenBy(c => c.CreatorName)
            .ToArray();

        var result = xmlParser.Serialize<ExportCreatorDto>(creators, "Creators");
        return result;
    }

    public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
    {
        var sellers = context.Sellers
            .AsEnumerable()
            .Where(s => s.BoardgamesSellers.Any(s => s.Boardgame.YearPublished >= year && s.Boardgame.Rating <= rating))
            .Select(s => new ExportSellerDto()
            {
                Name = s.Name,
                Website = s.Website,
                Boardgames = s.BoardgamesSellers
                .Where(s => s.Boardgame.YearPublished >= year && s.Boardgame.Rating <= rating)
                .OrderByDescending(p => p.Boardgame.Rating)
                .ThenBy(p => p.Boardgame.Name)
                .Select(bs => new ExportBoardgamesDto()
                { 
                Name = bs.Boardgame.Name,
                Rating = bs.Boardgame.Rating,
                Mechanics = bs.Boardgame.Mechanics,
                Category = bs.Boardgame.CategoryType.ToString()
                })
                .ToArray()
            })
            .OrderByDescending(b => b.Boardgames.Count())
            .ThenBy(b => b.Name)
            .Take(5)
            .ToArray();

        return JsonConvert.SerializeObject(sellers, Formatting.Indented);
    }
}