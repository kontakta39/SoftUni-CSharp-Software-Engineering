using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("User")]
public class ExportSoldProductsDTO
{
    [XmlElement("firstName")]
    public string FirstName { get; set; } = null!;

    [XmlElement("lastName")]
    public string LastName { get; set; } = null!;

    [XmlArray("soldProducts")]
    public UserProductsDTO[] SoldProducts { get; set; }
}

[XmlType("Product")]
public class UserProductsDTO
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("price")]
    public decimal Price { get; set; }
}