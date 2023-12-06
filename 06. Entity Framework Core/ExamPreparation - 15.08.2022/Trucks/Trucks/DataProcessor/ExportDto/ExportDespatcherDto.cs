using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto;

[XmlType("Despatcher")]
public class ExportDespatcherDto
{
    [XmlAttribute("TrucksCount")]
    public int TrucksCount { get; set; }

    [XmlElement("DespatcherName")]
    public string DespatcherName { get; set; }

    [XmlArray("Trucks")]
    public TruckInfoDto[] Trucks { get; set; }
}

[XmlType("Truck")]
public class TruckInfoDto
{
    [XmlElement("RegistrationNumber")]
    public string RegistrationNumber { get; set; }

    [XmlElement("Make")]
    public string Make { get; set; }
}