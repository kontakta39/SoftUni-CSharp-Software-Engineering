using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ExportDtos;

public class ExportPropertyDto
{
    public string PropertyIdentifier { get; set; }

    public int Area { get; set; }

    public string Address { get; set; }

    public string DateOfAcquisition { get; set; }

    public OwnerDto[] Owners { get; set; }
}

public class OwnerDto
{
    public string LastName { get; set; }

    public string MaritalStatus { get; set; }
}