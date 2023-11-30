using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;

[XmlType("Car")]
public class ImportCarDTO
{
    [XmlElement("make")]
    public string Make { get; set; }

    [XmlElement("model")]
    public string Model { get; set; }

    [XmlElement("traveledDistance")]
    public long TraveledDistance { get; set; }

    [XmlArray("parts")]
    public PartDTO[] PartsIds { get; set; }
}

[XmlType("partId")]
public class PartDTO
{
    [XmlAttribute("id")]
    public int PartId { get; set; }
}