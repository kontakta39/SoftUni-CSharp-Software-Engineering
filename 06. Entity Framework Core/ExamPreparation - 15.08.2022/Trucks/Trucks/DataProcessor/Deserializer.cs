namespace Trucks.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using AutoMapper;
using Data;
using Newtonsoft.Json;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ImportDto;
using Trucks.Utilities;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedDespatcher
        = "Successfully imported despatcher - {0} with {1} trucks.";

    private const string SuccessfullyImportedClient
        = "Successfully imported client - {0} with {1} trucks.";

    private static IMapper GetMapper()
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<TrucksProfile>());
        return new Mapper(mapper);
    }

    public static string ImportDespatcher(TrucksContext context, string xmlString)
    {
        IMapper mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportDespatcherDto[] despatcherDTOs = xmlParser.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");
        List<Despatcher> despatchers = new();
        StringBuilder sb = new();

        foreach (var despatcherDTO in despatcherDTOs)
        {
            if (!IsValid(despatcherDTO) || string.IsNullOrEmpty(despatcherDTO.Position))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Despatcher despatcher = new Despatcher()
            {
                Name = despatcherDTO.Name,
                Position = despatcherDTO.Position,
            };

            foreach (var currentTruck in despatcherDTO.Trucks)
            {
                if (!IsValid(currentTruck) || string.IsNullOrEmpty(currentTruck.VinNumber) || string.IsNullOrEmpty(currentTruck.RegistrationNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (currentTruck.CategoryType < 0 || currentTruck.CategoryType >= 4)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (currentTruck.MakeType < 0 || currentTruck.MakeType >= 5)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                despatcher.Trucks.Add(new Truck()
                {
                    RegistrationNumber = currentTruck.RegistrationNumber,
                    VinNumber = currentTruck.VinNumber,
                    TankCapacity = currentTruck.TankCapacity,
                    CargoCapacity = currentTruck.CargoCapacity,
                    CategoryType = (CategoryType)currentTruck.CategoryType,
                    MakeType = (MakeType)currentTruck.MakeType
                });
            }

            despatchers.Add(despatcher);
            sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count()));
        };

        context.Despatchers.AddRange(despatchers);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportClient(TrucksContext context, string jsonString)
    {
        var inputClients = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
        List<Client> clients = new();
        List<int> trucksIds = context.Trucks.Select(t => t.Id).ToList();
        StringBuilder sb = new();

        foreach (var inputClient in inputClients)
        {
            if (!IsValid(inputClient) || string.IsNullOrEmpty(inputClient.Nationality) || inputClient.Type == "usual")
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Client client = new()
            {
                Name = inputClient.Name,
                Nationality = inputClient.Nationality,
                Type = inputClient.Type,
            };

            foreach (var currentTruckId in inputClient.Trucks.Distinct())
            {
                if (!trucksIds.Contains(currentTruckId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                else
                {
                    client.ClientsTrucks.Add(new ClientTruck()
                    {
                        TruckId = currentTruckId
                    });
                }
            }

            sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count()));
            clients.Add(client);
        }

        context.Clients.AddRange(clients);
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