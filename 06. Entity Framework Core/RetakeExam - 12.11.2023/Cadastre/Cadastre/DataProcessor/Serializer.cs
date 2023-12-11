using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Utilities;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor;

public class Serializer
{
    public static string ExportPropertiesWithOwners(CadastreContext dbContext)
    {
        var xmlParser = new XmlParser();
        string inputDateTime = "01/01/2000";
        DateTime dateTime = DateTime.ParseExact(inputDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        var properties = dbContext.Properties
            .AsEnumerable()
            .Where(p => p.DateOfAcquisition >= dateTime)
            .OrderByDescending(p => p.DateOfAcquisition)
            .ThenBy(p => p.PropertyIdentifier)
            .Select(p => new ExportPropertyDto()
            {
                PropertyIdentifier = p.PropertyIdentifier,
                Area = p.Area,
                Address = p.Address,
                DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Owners = p.PropertiesCitizens
                .OrderBy(p => p.Citizen.LastName)
                .Select(pc => new OwnerDto()
                {
                    LastName = pc.Citizen.LastName,
                    MaritalStatus = pc.Citizen.MaritalStatus.ToString()
                })
                .ToArray()
            })
            .ToArray();

        return JsonConvert.SerializeObject(properties, Formatting.Indented);
    }

    public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
    {
        var xmlParser = new XmlParser();

        int inputArea = 100;

        var properties = dbContext.Properties
            .Where(p => p.Area >= 100)
            .OrderByDescending(p => p.Area)
            .ThenBy(p => p.DateOfAcquisition)
            .Select(p => new ExportPropertyWithDistrictDto()
            {
                PostalCode = p.District.PostalCode,
                PropertyIdentifier = p.PropertyIdentifier,
                Area = p.Area,
                DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            })
            .ToArray();

        var result = xmlParser.Serialize<ExportPropertyWithDistrictDto>(properties, "Properties");
        return result;
    }
}