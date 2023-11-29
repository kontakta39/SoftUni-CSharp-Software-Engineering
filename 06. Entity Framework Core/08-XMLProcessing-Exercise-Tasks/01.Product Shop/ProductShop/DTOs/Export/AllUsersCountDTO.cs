using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

public class AllUsersCountDTO
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("users")]
    public ExportUsersWithProducts[] ExportUsersWithProducts { get; set; }
}

[XmlType("User")]
public class ExportUsersWithProducts
{
    [XmlElement("firstName")]
    public string FirstName { get; set; } = null!;

    [XmlElement("lastName")]
    public string LastName { get; set; } = null!;

    [XmlElement("age")]
    public int? Age { get; set; }

    [XmlElement("SoldProducts")]
    public SoldProductsDTO SoldProducts { get; set; }
}

[XmlType("SoldProducts")]
public class SoldProductsDTO
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("products")]
    public CurrentProductsDTO[] CurrentProducts { get; set; }
}

[XmlType("Product")]
public class CurrentProductsDTO
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("price")]
    public decimal Price { get; set; }
}