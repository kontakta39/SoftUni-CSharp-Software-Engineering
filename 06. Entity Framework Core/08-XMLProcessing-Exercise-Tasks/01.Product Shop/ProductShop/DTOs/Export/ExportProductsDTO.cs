﻿using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("Product")]
public class ExportProductsDTO
{
    [XmlElement("name")]
    public string Name { get; set; } = null!;

    [XmlElement("price")]
    public decimal Price { get; set; }

    [XmlElement("buyer")]
    public string BuyerName { get; set; }
}