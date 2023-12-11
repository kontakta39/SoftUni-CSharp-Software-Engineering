namespace Footballers.DataProcessor;

using AutoMapper;
using Footballers.Data;
using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ImportDto;
using Footballers.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Security.AccessControl;
using System.Text;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedCoach
        = "Successfully imported coach - {0} with {1} footballers.";

    private const string SuccessfullyImportedTeam
        = "Successfully imported team - {0} with {1} footballers.";

    private static IMapper GetMapper()
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<FootballersProfile>());
        return new Mapper(mapper);
    }

    public static string ImportCoaches(FootballersContext context, string xmlString)
    {
        IMapper mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportCoachDto[] coachDtos = xmlParser.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");
        List<Coach> coaches = new();
        StringBuilder sb = new();

        foreach (var coachDTO in coachDtos)
        {
            if (!IsValid(coachDTO) || string.IsNullOrEmpty(coachDTO.Nationality))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Coach coach = new Coach()
            {
                Name = coachDTO.Name,
                Nationality = coachDTO.Nationality
            };

            foreach (var currentFootballer in coachDTO.Footballers)
            {
                if (!IsValid(currentFootballer) || string.IsNullOrEmpty(currentFootballer.ContractStartDate) || string.IsNullOrEmpty(currentFootballer.ContractEndDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime contractStartDate = DateTime.ParseExact(currentFootballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime contractEndDate = DateTime.ParseExact(currentFootballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (contractStartDate > contractEndDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (currentFootballer.BestSkillType < 0 || currentFootballer.BestSkillType >= 5)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (currentFootballer.PositionType < 0 || currentFootballer.PositionType >= 4)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                coach.Footballers.Add(new Footballer()
                {
                    Name = currentFootballer.Name,
                    ContractStartDate = contractStartDate,
                    ContractEndDate = contractEndDate,
                    BestSkillType = (BestSkillType)currentFootballer.BestSkillType,
                    PositionType = (PositionType)currentFootballer.PositionType
                });
            }

            coaches.Add(coach);
            sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count()));
        };

        context.Coaches.AddRange(coaches);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportTeams(FootballersContext context, string jsonString)
    {
        var inputTeams = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);
        List<Team> teams = new();
        List<int> footballersIds = context.Footballers.Select(f => f.Id).ToList();
        StringBuilder sb = new();

        foreach (var inputTeam in inputTeams)
        {
            if (!IsValid(inputTeam) || string.IsNullOrEmpty(inputTeam.Nationality) || inputTeam.Trophies == 0)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Team team = new()
            {
                Name = inputTeam.Name,
                Nationality = inputTeam.Nationality,
                Trophies = inputTeam.Trophies
            };

            foreach (var currentFootballerId in inputTeam.Footballers.Distinct())
            {
                if (!footballersIds.Contains(currentFootballerId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                else
                {
                    team.TeamsFootballers.Add(new TeamFootballer()
                    {
                        TeamId = team.Id,
                        FootballerId = currentFootballerId
                    });
                }
            }

            sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count()));
            teams.Add(team);
        }

        context.Teams.AddRange(teams);
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