namespace Boardgames.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Text;
using Boardgames.Data;
using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Boardgames.DataProcessor.ImportDto;
using Boardgames.Utilities;
using Newtonsoft.Json;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedCreator
        = "Successfully imported creator – {0} {1} with {2} boardgames.";

    private const string SuccessfullyImportedSeller
        = "Successfully imported seller - {0} with {1} boardgames.";

    //private static IMapper GetMapper()
    //{
    //    var mapper = new MapperConfiguration(c => c.AddProfile<BoardgamesProfile>());
    //    return new Mapper(mapper);
    //}

    public static string ImportCreators(BoardgamesContext context, string xmlString)
    {
        XmlParser xmlParser = new XmlParser();

        ImportCreatorDto[] creatorDTOs = xmlParser.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");
        List<Creator> creators = new();
        StringBuilder sb = new();

        foreach (var currentCreator in creatorDTOs)
        {
            if (!IsValid(currentCreator))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Creator creator = new()
            {
                FirstName = currentCreator.FirstName,
                LastName = currentCreator.LastName
            };

            foreach (var currentBoardgame in currentCreator.Boardgames)
            {
                if (string.IsNullOrEmpty(currentBoardgame.Name) || currentBoardgame.CategoryType >= 5 || currentBoardgame.CategoryType < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (IsValid(currentBoardgame))
                {
                    creator.Boardgames.Add(new Boardgame()
                    {
                        Name = currentBoardgame.Name,
                        Rating = currentBoardgame.Rating,
                        YearPublished = currentBoardgame.YearPublished,
                        CategoryType = (CategoryType)currentBoardgame.CategoryType,
                        Mechanics = currentBoardgame.Mechanics
                    });
                }

                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            creators.Add(creator);
            sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count()));
        }

        context.Creators.AddRange(creators);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportSellers(BoardgamesContext context, string jsonString)
    {
        var inputSellers = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);
        List<Seller> sellers = new();
        StringBuilder sb = new();
        List<int> boardgamesIds = context.Boardgames.Select(b => b.Id).ToList();

        foreach (var item in inputSellers)
        {
            if (!IsValid(item))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Seller currentSeller = new()
            {
               Name = item.Name,
               Address = item.Address,
               Country = item.Country,
               Website = item.Website,
            };

            foreach (var currentBoardgame in item.Boardgames.Distinct())
            {
                if (boardgamesIds.Contains(currentBoardgame))
                {
                    currentSeller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        BoardgameId = currentBoardgame
                    });
                }

                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            sb.AppendLine(string.Format(SuccessfullyImportedSeller, currentSeller.Name, currentSeller.BoardgamesSellers.Count()));
            sellers.Add(currentSeller);
        }

        context.Sellers.AddRange(sellers);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}