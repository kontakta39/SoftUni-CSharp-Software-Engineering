using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

        //Console.WriteLine(ImportUsers(context, inputJson));
        //Console.WriteLine(ImportProducts(context, inputJson));
        //Console.WriteLine(ImportCategories(context, inputJson));
        //Console.WriteLine(ImportCategoryProducts(context, inputJson));
        //GetProductsInRange(context);
        //GetSoldProducts(context);
        //GetCategoriesByProductsCount(context);
        GetUsersWithProducts(context);
    }

    //1 Exercise - Import Users
    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        var users = JsonConvert.DeserializeObject<User[]>(inputJson);

        context.Users.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {users.Length}";
    }

    //2 Exercise - Import Products
    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

        context.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    //3 Exercise - Import Categories
    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
        int count = 0;

        foreach (var category in categories)
        {
            if (category.Name != null)
            {
                count++;
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        return $"Successfully imported {count}";
    }

    //4 Exercise - Import Categories and Products
    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

        context.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Length}";
    }

    //5 Exercise - Export Products in Range
    public static string GetProductsInRange(ProductShopContext context)
    {
        var currentProducts = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Select(p => new
            {
                name = p.Name,
                price = p.Price,
                seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
            })
            .ToList();

        string products = JsonConvert.SerializeObject(currentProducts, Formatting.Indented);
        return products;
    }

    //6 Exercise - Export Sold Products
    public static string GetSoldProducts(ProductShopContext context)
    {
        var currentUsers = context.Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                soldProducts = u.ProductsSold.Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    buyerFirstName = p.Buyer.FirstName,
                    buyerLastName = p.Buyer.LastName
                })
            })
            .ToList();

        string users = JsonConvert.SerializeObject(currentUsers, Formatting.Indented);
        return users;
    }

    //7 Exercise - Export Categories by Products Count
    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var categories = context.Categories
            .OrderByDescending(c => c.CategoriesProducts.Count)
            .Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoriesProducts.Count(),
                averagePrice = $"{c.CategoriesProducts.Average(p => p.Product.Price):f2}",
                totalRevenue = $"{c.CategoriesProducts.Sum(p => p.Product.Price):f2}"
            })
            .ToList();

        string users = JsonConvert.SerializeObject(categories, Formatting.Indented);
        return users;
    }

    //8 Exercise - Export Users and Products
    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var currentUsers = context.Users
             .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
             .Select(u => new
             {
                 firstName = u.FirstName,
                 lastName = u.LastName,
                 age = u.Age,
                 soldProducts = u.ProductsSold
                 .Where(p => p.BuyerId != null)
                 .Select(p => new
                 {
                     name = p.Name,
                     price = p.Price
                 })
             })
             .OrderByDescending(u => u.soldProducts.Count())
             .ToList();

        var output = new
        {
            usersCount = currentUsers.Count,
            users = currentUsers.Select(u => new
            {
                u.firstName,
                u.lastName,
                u.age,
                soldProducts = new
                {
                    count = u.soldProducts.Count(),
                    products = u.soldProducts
                }
            })
        };

        string users = JsonConvert.SerializeObject(output, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });

        return users;
    }
}