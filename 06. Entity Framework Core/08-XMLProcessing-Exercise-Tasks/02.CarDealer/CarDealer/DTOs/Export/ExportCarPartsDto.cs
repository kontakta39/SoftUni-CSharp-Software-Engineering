﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Export;

[XmlType("car")]
public class ExportCarPartsDto
{
    [XmlAttribute("make")]
    public string Make { get; set; } 

    [XmlAttribute("model")]
    public string Model { get; set; }

    [XmlAttribute("traveled-distance")]
    public long TraveledDistance { get; set; }

    [XmlArray("parts")]
    public PartsDto[] Parts { get; set; }
}

[XmlType("part")]
public class PartsDto
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("price")]
    public decimal Price { get; set; }
}