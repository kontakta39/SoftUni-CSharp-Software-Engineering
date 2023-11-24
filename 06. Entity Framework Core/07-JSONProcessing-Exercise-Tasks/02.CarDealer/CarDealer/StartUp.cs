using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        //string inputJson = File.ReadAllText("../../../Datasets/sales.json");

        //Console.WriteLine(ImportSuppliers(context, inputJson));
        //Console.WriteLine(ImportParts(context, inputJson));
        //Console.WriteLine(ImportCars(context, inputJson));
        //Console.WriteLine(ImportCustomers(context, inputJson));
        //Console.WriteLine(ImportSales(context, inputJson));
        //Console.WriteLine(GetOrderedCustomers(context));
        //Console.WriteLine(GetCarsFromMakeToyota(context));
        //Console.WriteLine(GetLocalSuppliers(context));
        //Console.WriteLine(GetCarsWithTheirListOfParts(context));
        Console.WriteLine(GetTotalSalesByCustomer(context));
        //Console.WriteLine(GetSalesWithAppliedDiscount(context));
    }

    //9 Exercise - Import Suppliers
    public static string ImportSuppliers(CarDealerContext context, string inputJson)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);
        SupplierDTO[] suppliersDTO = JsonConvert.DeserializeObject<SupplierDTO[]>(inputJson);

        Supplier[] suppliers = mapper.Map<Supplier[]>(suppliersDTO);
        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        return $"Successfully imported {suppliers.Count()}.";
    }

    //10 Exercise - Import Parts
    public static string ImportParts(CarDealerContext context, string inputJson)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);
        PartDTO[] partsDTO = JsonConvert.DeserializeObject<PartDTO[]>(inputJson);
        Part[] parts = mapper.Map<Part[]>(partsDTO);

        List<int> suppliersId = new();

        foreach (var item in context.Suppliers)
        {
            suppliersId.Add(item.Id);
        }

        List<int> recordsToSkip = new();

        foreach (Part part in parts)
        {
            if (!suppliersId.Contains(part.SupplierId))
            {
                recordsToSkip.Add(part.SupplierId);
            }
        }

        int count = 0;

        foreach (var item in parts)
        {
            if (recordsToSkip.Contains(item.SupplierId))
            {
                continue;
            }

            count++;
            context.Parts.Add(item);
        }

        context.SaveChanges();
        return $"Successfully imported {count}.";
    }

    //11 Exercise - Import Cars
    public static string ImportCars(CarDealerContext context, string inputJson)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);
        CarDTO[] carsDTO = JsonConvert.DeserializeObject<CarDTO[]>(inputJson);
        List<Car> cars = new();

        foreach (var carDTO in carsDTO)
        {
            Car car = mapper.Map<Car>(carDTO);

            int[] carPartIds = carDTO.PartsId
                .Distinct()
                .ToArray();

            var carParts = new List<PartCar>();

            foreach (var id in carPartIds)
            {
                carParts.Add(new PartCar
                {
                    Car = car,
                    PartId = id
                });
            }

            car.PartsCars = carParts;

            foreach (var item in carParts)
            {
                context.PartsCars.Add(item);
            }

            cars.Add(car);
        }

        context.Cars.AddRange(cars);
        context.SaveChanges();

        return $"Successfully imported {cars.Count()}.";
    }

    //12 Exercise - Import Customers
    public static string ImportCustomers(CarDealerContext context, string inputJson)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);
        CustomerDTO[] customersDTO = JsonConvert.DeserializeObject<CustomerDTO[]>(inputJson);
        Customer[] customers = mapper.Map<Customer[]>(customersDTO);

        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Count()}.";
    }

    //13 Exercise - Import Sales
    public static string ImportSales(CarDealerContext context, string inputJson)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);
        SaleDTO[] salesDTO = JsonConvert.DeserializeObject<SaleDTO[]>(inputJson);
        Sale[] sales = mapper.Map<Sale[]>(salesDTO);

        context.Sales.AddRange(sales);
        context.SaveChanges();

        return $"Successfully imported {sales.Count()}.";
    }

    //14 Exercise - Export Ordered Customers
    public static string GetOrderedCustomers(CarDealerContext context)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);

        string directoryPath = @"C:\Users\User\source\repos\Entity Framework Core\07-JSONProcessing-Exercise-Tasks\02.CarDealer\CarDealer\Results";
        string filePath = Path.Combine(directoryPath, "export-ordered-customers.json");

        //var customers = context.Customers
        //    .OrderBy(c => c.BirthDate)
        //    .ThenBy(c => c.IsYoungDriver)
        //    .Select(c => new
        //    {
        //        Name = c.Name,
        //        BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        IsYoungDriver = c.IsYoungDriver
        //    });

        var customers = context.Customers
            .OrderBy(c => c.BirthDate)
            .ThenBy(c => c.IsYoungDriver)
            .Select(c => mapper.Map<ExportCustomerDTO>(c))
            .ToList();

        var formattedDates = customers
            .Select(c => new
            {
                Name = c.Name,
                BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsYoungDriver = c.IsYoungDriver
            })
            .ToList();

        // Convert the sorted list of customers to JSON format with custom date format
        string jsonResult = JsonConvert.SerializeObject(formattedDates, Formatting.Indented);

        // Check if the directory exists, if not, create it
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Save JSON data to the specified file path
        File.WriteAllText(filePath, jsonResult);

        return jsonResult;
    }

    //15 Exercise - Export Cars from Make Toyota
    public static string GetCarsFromMakeToyota(CarDealerContext context)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);

        string directoryPath = @"C:\Users\User\source\repos\Entity Framework Core\07-JSONProcessing-Exercise-Tasks\02.CarDealer\CarDealer\Results";
        string filePath = Path.Combine(directoryPath, "export-cars-toyota.json");

        //var cars = context.Cars
        //    .OrderBy(c => c.Model)
        //    .ThenByDescending(c => c.TraveledDistance)
        //    .Where(c => c.Make == "Toyota")
        //    .Select(c => new
        //    {
        //        Id = c.Id,
        //        Make = c.Make,
        //        Model = c.Model,
        //        TraveledDistance = c.TraveledDistance
        //    });

        var carsDTO = context.Cars
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .Where(c => c.Make == "Toyota")
            .Select(c => mapper.Map<ExportCarDTO>(c))
            .ToList();

        var cars = carsDTO
            .Select(c => new
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance
            })
            .ToList();

        string jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, jsonResult);
        return jsonResult;
    }

    //16 Exercise - Export Local Suppliers
    public static string GetLocalSuppliers(CarDealerContext context)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);

        string directoryPath = @"C:\Users\User\source\repos\Entity Framework Core\07-JSONProcessing-Exercise-Tasks\02.CarDealer\CarDealer\Results";
        string filePath = Path.Combine(directoryPath, "export-local-suppliers.json");

        //var suppliers = context.Suppliers
        //.Where(s => s.IsImporter == false)
        //.Select(s => new
        //{
        //    Id = s.Id,
        //    Name = s.Name,
        //    PartsCount = s.Parts.Count()
        //});

        var suppliers = context.Suppliers
        .Include(s => s.Parts)
        .Where(s => s.IsImporter == false)
        .Select(c => mapper.Map<ExportSupplierDTO>(c))
        .ToList();

        string jsonResult = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, jsonResult);
        return jsonResult;
    }

    //17 Exercise - Export Local Suppliers
    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);

        string directoryPath = @"C:\Users\User\source\repos\Entity Framework Core\07-JSONProcessing-Exercise-Tasks\02.CarDealer\CarDealer\Results";
        string filePath = Path.Combine(directoryPath, "cars-with-their-list-of-parts.json");

        //var cars = context.Cars
        //    .Select(c => new
        //    {
        //        Make = c.Make,
        //        Model = c.Model,
        //        TraveledDistance = c.TraveledDistance,
        //        parts = c.PartsCars.Select(pc => new
        //        {
        //            Name = pc.Part.Name,
        //            Price = $"{pc.Part.Price:f2}"
        //        })
        //    });

        var carsDTO = context.Cars
        .Include(c => c.PartsCars)
        .ThenInclude(c => c.Part)
        .Select(c => mapper.Map<ExportCarWithParts>(c))
        .ToList();

        var cars = carsDTO.Select(c => new
        {
            car = new
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance,
            },
            parts = c.Parts.Where(pc => pc.Part != null).Select(pc => new
            {
                Name = pc.Part.Name,
                Price = $"{pc.Part.Price:f2}"
            })
        })
        .ToList();

        string jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, jsonResult);
        return jsonResult;
    }

    //18 Exercise - Export Total Sales by Customer
    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);

        string directoryPath = @"C:\Users\User\source\repos\Entity Framework Core\07-JSONProcessing-Exercise-Tasks\02.CarDealer\CarDealer\Results";
        string filePath = Path.Combine(directoryPath, "customers-total-sales.json");

        //var customers = context.Customers
        //        .Where(c => c.Sales.Count() > 0)
        //        .Select(c => new
        //        {
        //            fullName = c.Name,
        //            boughtCars = c.Sales.Count(),
        //            spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(p => p.Part.Price))
        //        })
        //        .OrderByDescending(x => x.spentMoney)
        //        .ThenByDescending(x => x.boughtCars)
        //        .ToList();

        var customersDTO = context.Customers
        .Include(c => c.Sales)
        .ThenInclude(c => c.Car)
        .ThenInclude(c => c.PartsCars)
        .ThenInclude(c => c.Part)
        .Select(c => mapper.Map<ExportCustomerTotalSales>(c))
        .ToList();

        var customers = customersDTO
            .Where(c => c.Sales.Count() > 0)
            .Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count(),
                spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(p => p.Part.Price))
            })
            .OrderByDescending(c => c.spentMoney)
            .ThenByDescending(c => c.boughtCars)
            .ToList();

        string jsonResult = JsonConvert.SerializeObject(customers, Formatting.Indented);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, jsonResult);
        return jsonResult;
    }

    //19 Exercise - Export Sales with Applied Discount
    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {
        var configuration = new MapperConfiguration(mc => mc.AddProfile<CarDealerProfile>());
        IMapper mapper = new Mapper(configuration);

        string directoryPath = @"C:\Users\User\source\repos\Entity Framework Core\07-JSONProcessing-Exercise-Tasks\02.CarDealer\CarDealer\Results";
        string filePath = Path.Combine(directoryPath, "sales-discounts.json");

        //var sales = context.Sales
        //    .Take(10)
        //    .Select(s => new
        //    {
        //        Car = new
        //        {
        //            Make = s.Car.Make,
        //            Model = s.Car.Model,
        //            TraveledDistance = s.Car.TraveledDistance
        //        },
        //        CustomerName = s.Customer.Name,
        //        Discount = $"{s.Discount:f2}",
        //        Price = $"{s.Car.PartsCars.Sum(p => p.Part.Price):f2}",
        //        PriceWithDiscount = $"{(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - s.Discount / 100)):f2}"
        //    })
        //    .ToList();

        var salesDTO = context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Car)
            .ThenInclude(s => s.PartsCars)
            .ThenInclude(s => s.Part)
            .Select(c => mapper.Map<ExportSalesDiscountsDTO>(c))
            .ToList();

        var sales = salesDTO
            .Take(10)
            .Select(s => new
            {
                car = new
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance
                },
                customerName = s.Customer.Name,
                discount = $"{s.Discount:f2}",
                price = $"{s.Car.PartsCars.Sum(p => p.Part.Price):f2}",
                priceWithDiscount = $"{(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - s.Discount / 100)):f2}"
            })
            .ToList();

        string jsonResult = JsonConvert.SerializeObject(sales, Formatting.Indented);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, jsonResult);
        return jsonResult;
    }
}