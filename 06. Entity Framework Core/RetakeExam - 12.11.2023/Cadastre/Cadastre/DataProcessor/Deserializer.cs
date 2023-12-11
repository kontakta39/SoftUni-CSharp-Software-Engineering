namespace Cadastre.DataProcessor;

using AutoMapper;
using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Cadastre.DataProcessor.ImportDtos;
using Cadastre.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

public class Deserializer
{
    private const string ErrorMessage =
        "Invalid Data!";
    private const string SuccessfullyImportedDistrict =
        "Successfully imported district - {0} with {1} properties.";
    private const string SuccessfullyImportedCitizen =
        "Succefully imported citizen - {0} {1} with {2} properties.";

    private static IMapper GetMapper()
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<CadastreProfile>());
        return new Mapper(mapper);
    }

    public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
    {
        IMapper mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportDistrictDto[] districtDTOs = xmlParser.Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");
        List<District> districts = new();
        List<string> districtsNames = new();
        List<Property> currentProperties = new();
        StringBuilder sb = new();
        bool ifContains = false;

        foreach (var districtDTO in districtDTOs)
        {
            if (districtsNames.Contains(districtDTO.Name))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            if (!IsValid(districtDTO))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            District district = new District()
            {
                Name = districtDTO.Name,
                PostalCode = districtDTO.PostalCode,
                Region = (Region)Enum.Parse(typeof(Region), districtDTO.Region)
            };

            foreach (var currentProperty in districtDTO.Properties)
            {
                foreach (var item in currentProperties)
                {
                    if (item.PropertyIdentifier == currentProperty.PropertyIdentifier
                        || item.Address == currentProperty.Address)
                    {
                        ifContains = true;
                        break;
                    }
                }

                if (ifContains == true)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (district.Properties.Any(p => p.PropertyIdentifier == currentProperty.PropertyIdentifier))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (district.Properties.Any(p => p.Address == currentProperty.Address))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(currentProperty))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dateOfAcquisition = DateTime.ParseExact(currentProperty.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Property property = new Property()
                {
                    PropertyIdentifier = currentProperty.PropertyIdentifier,
                    Area = currentProperty.Area,
                    Details = currentProperty.Details,
                    Address = currentProperty.Address,
                    DateOfAcquisition = dateOfAcquisition
                };

                district.Properties.Add(property);
                currentProperties.Add(property);
            }

            ifContains = false;
            districts.Add(district);
            districtsNames.Add(district.Name);
            sb.AppendLine(string.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count()));
        };

        dbContext.Districts.AddRange(districts);
        dbContext.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
    {
        var inputCitizens = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);
        List<Citizen> citizens = new();
        StringBuilder sb = new();

        foreach (var inputCitizen in inputCitizens)
        {
            if (!IsValid(inputCitizen))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            if (inputCitizen.MaritalStatus != "Unmarried" && inputCitizen.MaritalStatus != "Married"
                && inputCitizen.MaritalStatus != "Divorced" && inputCitizen.MaritalStatus != "Widowed")
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            DateTime birthDate = DateTime.ParseExact(inputCitizen.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Citizen citizen = new()
            {
                FirstName = inputCitizen.FirstName,
                LastName = inputCitizen.LastName,
                BirthDate = birthDate,
                MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), inputCitizen.MaritalStatus)
            };

            foreach (var currentPropertyId in inputCitizen.Properties)
            {
                citizen.PropertiesCitizens.Add(new PropertyCitizen()
                {
                    PropertyId = currentPropertyId
                });
            }

            sb.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count()));
            citizens.Add(citizen);
        }

        dbContext.Citizens.AddRange(citizens);
        dbContext.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}