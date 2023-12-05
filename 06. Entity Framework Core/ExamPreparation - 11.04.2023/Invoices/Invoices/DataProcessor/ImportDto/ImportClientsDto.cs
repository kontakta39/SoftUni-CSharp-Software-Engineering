using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto;

[XmlType("Client")]
public class ImportClientsDto
{
    [XmlElement("Name")]
    [StringLength(25, MinimumLength = 10)]
    public string Name { get; set; }

    [XmlElement("NumberVat")]
    [StringLength(15, MinimumLength = 10)]
    public string NumberVat { get; set; }

    [XmlArray("Addresses")]
    public AddressDto[] Addresses { get; set; }
}

[XmlType("Address")]
public class AddressDto
{
    [XmlElement("StreetName")]
    [StringLength(20, MinimumLength = 10)]
    public string StreetName { get; set; }

    [XmlElement("StreetNumber")]
    public int StreetNumber { get; set; }

    [XmlElement("PostCode")]
    public string PostCode { get; set; }

    [XmlElement("City")]
    [StringLength(15, MinimumLength = 5)]
    public string City { get; set; }

    [XmlElement("Country")]
    [StringLength(15, MinimumLength = 5)]
    public string Country { get; set; }
}