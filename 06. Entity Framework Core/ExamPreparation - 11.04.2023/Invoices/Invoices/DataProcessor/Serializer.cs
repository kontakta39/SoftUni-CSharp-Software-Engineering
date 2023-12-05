namespace Invoices.DataProcessor;

using AutoMapper.Execution;
using Invoices.Data;
using Invoices.DataProcessor.ExportDto;
using Newtonsoft.Json;
using Invoices.Utilities;
using System.Globalization;
using System.Linq;

public class Serializer
{
    public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
    {
        var xmlParser = new XmlParser();

        var currentClients = context.Clients
            .Where(c => c.Invoices.Any(i => i.IssueDate > date))
            .Select(c => new ExportClientsWithInvoicesDto()
            {
                InvoicesCount = c.Invoices.Count,
                Name = c.Name,
                NumberVat = c.NumberVat,
                Invoices = c.Invoices
                .OrderBy(i => i.IssueDate)
                .ThenByDescending(i => i.DueDate)
                .Select(i => new InvoicesDto()
                {
                    Number = i.Number,
                    Amount = decimal.Parse(i.Amount.ToString().TrimEnd('0').TrimEnd('.')),
                    DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                    CurrencyType = i.CurrencyType
                })
                .ToArray()
            })
            .OrderByDescending(c => c.Invoices.Count())
            .ThenBy(c => c.Name)
            .ToArray();

        var result = xmlParser.Serialize<ExportClientsWithInvoicesDto>(currentClients, "Clients");
            return result;
    }

    public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
    {
        var products = context.Products
                       .Where(p => p.ProductsClients.Any(p => p.Client.Name.Length >= nameLength))
                        .Select(p => new
                        {
                            Name = p.Name,
                            Price = decimal.Parse(p.Price.ToString().TrimEnd('0').TrimEnd('.')),
                            Category = p.CategoryType.ToString(),
                            Clients = p.ProductsClients
                                    .Where(pc => pc.Client.Name.Length >= nameLength)
                                    .Select(pc => new
                                    {
                                        Name = pc.Client.Name,
                                        NumberVat = pc.Client.NumberVat
                                    })
                                    .OrderBy(c => c.Name)
                                    .ToArray()
                        })
                        .OrderByDescending(p => p.Clients.Length)
                        .ThenBy(p => p.Name)
                        .Take(5)
                        .ToArray();

        return JsonConvert.SerializeObject(products, Formatting.Indented);
    }
}