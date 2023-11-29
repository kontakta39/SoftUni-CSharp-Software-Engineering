using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();

        //1 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/users.xml");
        //Console.WriteLine(ImportUsers(context, inputXml));

        //2 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/products.xml");
        //Console.WriteLine(ImportProducts(context, inputXml));

        //3 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/categories.xml");
        //Console.WriteLine(ImportCategories(context, inputXml));

        //4 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
        //Console.WriteLine(ImportCategoryProducts(context, inputXml));

        //5 Exercise
        //Console.WriteLine(GetProductsInRange(context));

        //6 Exercise
        //Console.WriteLine(GetSoldProducts(context));

        //7 Exercise
        //Console.WriteLine(GetCategoriesByProductsCount(context));

        //8 Exercise
        Console.WriteLine(GetUsersWithProducts(context));
    }

    private static IMapper GetMapper()
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<ProductShopProfile>());
        return new Mapper(mapper);
    }

    //1 Exercise - Import Users
    public static string ImportUsers(ProductShopContext context, string inputXml)
    {
        var serializer = new XmlSerializer(typeof(UserDTO[]), new XmlRootAttribute("Users"));
        using var reader = new StringReader(inputXml);
        var userDTOs = serializer.Deserialize(reader);
        var mapper = GetMapper();

        var users = mapper.Map<User[]>(userDTOs);

        context.Users.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {users.Length}";
    }

    //2 Exercise - Import Products
    public static string ImportProducts(ProductShopContext context, string inputXml)
    {
        var serializer = new XmlSerializer(typeof(ProductDTO[]), new XmlRootAttribute("Products"));
        using var reader = new StringReader(inputXml);
        var productDTOs = serializer.Deserialize(reader);
        var mapper = GetMapper();

        var products = mapper.Map<Product[]>(productDTOs);

        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    //3 Exercise - Import Categories
    public static string ImportCategories(ProductShopContext context, string inputXml)
    {
        var serializer = new XmlSerializer(typeof(CategoryDTO[]), new XmlRootAttribute("Categories"));
        using var reader = new StringReader(inputXml);
        var categoryDTOs = serializer.Deserialize(reader);
        var mapper = GetMapper();

        var categories = mapper.Map<Category[]>(categoryDTOs);

        foreach (var item in categories)
        {
            if (item != null)
            {
                context.Categories.Add(item);
            }
        }

        context.SaveChanges();
        return $"Successfully imported {categories.Length}";
    }

    //4 Exercise - Import Categories and Products
    public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
    {
        var serializer = new XmlSerializer(typeof(CategoryProductDTO[]), new XmlRootAttribute("CategoryProducts"));
        using var reader = new StringReader(inputXml);
        var categoryProductDTOs = serializer.Deserialize(reader);
        var mapper = GetMapper();

        var categoriesProducts = mapper.Map<CategoryProduct[]>(categoryProductDTOs);

        foreach (var item in categoriesProducts)
        {
            if (item.CategoryId != null && item.ProductId != null)
            {
                context.CategoryProducts.Add(item);
            }
        }

        context.SaveChanges();
        return $"Successfully imported {categoriesProducts.Length}";
    }

    //5 Exercise - Export Products In Range
    public static string GetProductsInRange(ProductShopContext context)
    {
        var xmlParser = new XmlParser();

        var productsInRange = context.Products
        .Where(p => p.Price >= 500 && p.Price <= 1000)
        .OrderBy(p => p.Price)
        .Take(10)
         .Select(p => new ExportProductsDTO()
         {
             Price = p.Price,
             Name = p.Name,
             BuyerName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
         })
        .ToArray();

        var products = xmlParser.Serialize<ExportProductsDTO[]>(productsInRange, "Products");

        return products.ToString().TrimEnd();
    }

    //6 Exercise - Export Sold Products
    public static string GetSoldProducts(ProductShopContext context)
    {
        var xmlParser = new XmlParser();

        var soldProducts = context.Users
        .Where(u => u.ProductsSold.Any())
        .OrderBy(u => u.LastName)
        .ThenBy(u => u.FirstName)
        .Take(5)
         .Select(u => new ExportSoldProductsDTO()
         {
             FirstName = u.FirstName,
             LastName = u.LastName,
             SoldProducts = u.ProductsSold.Select(p => new UserProductsDTO()
             {
                 Name = p.Name,
                 Price = p.Price,
             })
             .ToArray()
         })
        .ToArray();

        var products = xmlParser.Serialize<ExportSoldProductsDTO[]>(soldProducts, "Products");

        return products.ToString().TrimEnd();
    }

    //7 Exercise - Export Categories By Products Count
    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var xmlParser = new XmlParser();

        var currentCategories = context.Categories
            .Select(c => new ExportCategoriesDTO()
            {
                Name = c.Name,
                Count = c.CategoryProducts.Count(),
                AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price), // Calculate average price if products exist
                TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price) // Calculate total revenue
            })
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.TotalRevenue)
            .ToArray();

        var products = xmlParser.Serialize<ExportCategoriesDTO[]>(currentCategories, "Categories");

        return products.ToString().TrimEnd();
    }

    //8 Exercise - Export Users and Products
    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var xmlParser = new XmlParser();

        var currentUsers = context.Users
            .Where(u => u.ProductsSold.Any())
            .OrderByDescending(u => u.ProductsSold.Count())
            .Select(u => new ExportUsersWithProducts()
            {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsDTO()
                    {
                        Count = u.ProductsSold.Count(),
                        CurrentProducts = u.ProductsSold.Select(ps => new CurrentProductsDTO()
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                    }
            })
            .Take(10)
            .ToArray();

        var usersWithCount = new AllUsersCountDTO()
        {
            Count = context.Users.Count(u => u.ProductsSold.Any()),
            ExportUsersWithProducts = currentUsers
        };

        var users = xmlParser.Serialize<AllUsersCountDTO>(usersWithCount, "Users");
        return users.ToString().TrimEnd();
    }
}