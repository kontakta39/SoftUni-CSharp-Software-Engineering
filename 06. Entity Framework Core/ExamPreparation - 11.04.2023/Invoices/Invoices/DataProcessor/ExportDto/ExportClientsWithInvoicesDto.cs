using Invoices.Data.Models.Enums;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto;

[XmlType("Client")]
public class ExportClientsWithInvoicesDto
{
    [XmlAttribute("InvoicesCount")]
    public int InvoicesCount { get; set; }

    [XmlElement("ClientName")]
    public string Name { get; set; }

    [XmlElement("VatNumber")]
    public string NumberVat { get; set; }

    [XmlArray("Invoices")]
    public InvoicesDto[] Invoices { get; set; }
}

[XmlType("Invoice")]
public class InvoicesDto
{
    [XmlElement("InvoiceNumber")]
    public int Number { get; set; }

    [XmlElement("InvoiceAmount")]
    public decimal Amount { get; set; }

    [XmlElement("DueDate")]
    public string DueDate { get; set; }

    [XmlElement("Currency")]
    public CurrencyType CurrencyType { get; set; }
}