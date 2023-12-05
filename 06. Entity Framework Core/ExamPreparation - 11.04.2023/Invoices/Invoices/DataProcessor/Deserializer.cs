namespace Invoices.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Text;
using AutoMapper;
using Invoices.Data;
using Invoices.Data.Models;
using Invoices.DataProcessor.ImportDto;
using Newtonsoft.Json;
using Invoices.Utilities;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedClients
        = "Successfully imported client {0}.";

    private const string SuccessfullyImportedInvoices
        = "Successfully imported invoice with number {0}.";

    private const string SuccessfullyImportedProducts
        = "Successfully imported product - {0} with {1} clients.";

    private static IMapper GetMapper()
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<InvoicesProfile>());
        return new Mapper(mapper);
    }

    public static string ImportClients(InvoicesContext context, string xmlString)
    {
        IMapper mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportClientsDto[] clientDTOs = xmlParser.Deserialize<ImportClientsDto[]>(xmlString, "Clients");
        List<Client> clients = new();
        StringBuilder sb = new();

        foreach (var clientDTO in clientDTOs)
        {
            if (!IsValid(clientDTO))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Client client = new Client()
            {
                Name = clientDTO.Name,
                NumberVat = clientDTO.NumberVat
            };

            foreach (var item in clientDTO.Addresses)
            {
                if (IsValid(item))
                {
                    client.Addresses.Add(new Address()
                    {
                        StreetName = item.StreetName,
                        StreetNumber = item.StreetNumber,
                        PostCode = item.PostCode,
                        City = item.City,
                        Country = item.Country
                    });
                }

                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            clients.Add(client);
            sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
        }
        ;
        context.Clients.AddRange(clients);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportInvoices(InvoicesContext context, string jsonString)
    {
        var inputInvoices = JsonConvert.DeserializeObject<ImportInvoicesDto[]>(jsonString);
        List<Invoice> invoices = new();
        StringBuilder sb = new();
        int[] clientsIds = context.Clients.Select(c => c.Id).ToArray();

        foreach (var item in inputInvoices)
        {
            if (!IsValid(item) || item.IssueDate > item.DueDate || !clientsIds.Contains(item.ClientId))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Invoice currentInvoice = new Invoice()
            {
                Number = item.Number,
                IssueDate = item.IssueDate,
                DueDate = item.DueDate,
                Amount = item.Amount,
                CurrencyType = item.CurrencyType,
                ClientId = item.ClientId
            };

            sb.AppendLine(string.Format(SuccessfullyImportedInvoices, currentInvoice.Number));
            invoices.Add(currentInvoice);
        }

        context.Invoices.AddRange(invoices);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportProducts(InvoicesContext context, string jsonString)
    {
        var inputProducts = JsonConvert.DeserializeObject<ImportProductsDto[]>(jsonString);
        List<Product> products = new();
        StringBuilder sb = new();
        List<int> existingClientsIds = context.Clients.Select(c => c.Id).ToList();

        foreach (var item in inputProducts)
        {
            if (!IsValid(item))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            List<int> clientIds = new();

            Product product = new Product()
            {
                Name = item.Name,
                Price = item.Price,
                CategoryType = item.CategoryType
            };

            foreach (var currentClientId in item.Clients.Distinct())
            {
                if (existingClientsIds.Contains(currentClientId))
                {
                    if (!clientIds.Contains(currentClientId))
                    {
                        clientIds.Add(currentClientId);
                    }
                }

                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            foreach (var id in clientIds)
            {
                product.ProductsClients.Add(new ProductClient()
                {
                    ClientId = id
                });
            }

            sb.AppendLine(string.Format(SuccessfullyImportedProducts, item.Name, clientIds.Count));
            products.Add(product);
        }

        context.Products.AddRange(products);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}