using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //9 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
        //Console.WriteLine(ImportSuppliers(context, inputXml));

        //10 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/parts.xml");
        //Console.WriteLine(ImportParts(context, inputXml));

        //11 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/cars.xml");
        //Console.WriteLine(ImportCars(context, inputXml));

        //12 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/customers.xml");
        //Console.WriteLine(ImportCustomers(context, inputXml));

        //13 Exercise
        //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
        //Console.WriteLine(ImportSales(context, inputXml));

        //14 Exercise
        //Console.WriteLine(GetCarsWithDistance(context));

        //15 Exercise
        Console.WriteLine(GetCarsFromMakeBmw(context));
    }

    private static IMapper GetMapper()
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<CarDealerProfile>());
        return new Mapper(mapper);
    }

    //9 Exercise - Import Suppliers
    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        var mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        //Deserializing the Xml to Supplier DTOs
        ImportSupplierDTO[] supplierDTOs = xmlParser.Deserialize<ImportSupplierDTO[]>(inputXml, "Suppliers");

        //Mapping the Supplier DTOs to Suppliers
        Supplier[] suppliers = mapper.Map<Supplier[]>(supplierDTOs);

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        return $"Successfully imported {suppliers.Length}";
    }

    //10 Exercise - Import Parts
    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        var mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportPartDTO[] partDTOs = xmlParser.Deserialize<ImportPartDTO[]>(inputXml, "Parts");
        Part[] parts = mapper.Map<Part[]>(partDTOs);
        List<int> supplierIds = context.Suppliers.Select(x => x.Id).ToList();
        int count = 0;

        foreach (var id in supplierIds) 
        {
            foreach (var item in parts)
            {
                if (item.SupplierId == id)
                {
                    count++;
                    context.Parts.Add(item);
                }
            }
        }

        context.SaveChanges();

        return $"Successfully imported {count}";
    }

    //11 Exercise - Import Cars
    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        var mapper = GetMapper();
        var xmlParser = new XmlParser();

        //Deserializing the Xml to Part DTOs
        ImportCarDTO[] carsDTOs = xmlParser.Deserialize<ImportCarDTO[]>(inputXml, "Cars");

        //Mapping the Car DTOs to Cars only if their parts are unique
        List<Car> cars = new List<Car>();

        foreach (var carDTO in carsDTOs)
        {
            Car car = mapper.Map<Car>(carDTO);

            int[] carPartIds = carDTO.PartsIds
                .Select(x => x.PartId)
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
            cars.Add(car);
        }

        context.AddRange(cars);
        context.SaveChanges();

        return $"Successfully imported {cars.Count}";
    }

    //12 Exercise - Import Customers
    public static string ImportCustomers(CarDealerContext context, string inputXml)
    {
        var mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportCustomerDTO[] customerDTOs = xmlParser.Deserialize<ImportCustomerDTO[]>(inputXml, "Customers");
        Customer[] customers = mapper.Map<Customer[]>(customerDTOs);

        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Length}";
    }

    //13 Exercise - Import Sales
    public static string ImportSales(CarDealerContext context, string inputXml)
    {
        var mapper = GetMapper();
        XmlParser xmlParser = new XmlParser();

        ImportSaleDTO[] saleDTOs = xmlParser.Deserialize<ImportSaleDTO[]>(inputXml, "Sales");
        Sale[] sales = mapper.Map<Sale[]>(saleDTOs);
        List<int> carIds = context.Cars.Select(x => x.Id).ToList();
        int count = 0;

        foreach (var id in carIds)
        {
            foreach (var item in sales)
            {
                if (item.CarId == id)
                {
                    count++;
                    context.Sales.Add(item);
                }
            }
        }

        context.SaveChanges();

        return $"Successfully imported {count}";
    }

    //14 Exercise - Export Cars With Distance
    public static string GetCarsWithDistance(CarDealerContext context)
    {
        XmlParser xmlParser = new XmlParser();

        var carsWithDistance = context.Cars
        .Where(c => c.TraveledDistance > 2_000_000)
        .OrderBy(c => c.Make)
        .ThenBy(c => c.Model)
        .Take(10)
        .Select(c => new ExportCarsWithDistanceDTO()
        {
            Make = c.Make,
            Model = c.Model,
            TraveledDistance = c.TraveledDistance
        })
        .ToArray();


        var cars = xmlParser.Serialize<ExportCarsWithDistanceDTO[]>(carsWithDistance, "cars");

        return cars.ToString().TrimEnd();
    }

    //15 Exercise - Export Cars from Make BMW
    public static string GetCarsFromMakeBmw(CarDealerContext context)
    {
        XmlParser xmlParser = new XmlParser();

        var BMWCars = context.Cars
        .Where(c => c.Make == "BMW")
        .OrderBy(c => c.Model)
        .ThenByDescending(c => c.TraveledDistance)
        .Select(c => new ExportBMWCarsDTO()
        {
            Id = c.Id,
            Model = c.Model,
            TraveledDistance = c.TraveledDistance
        })
        .ToArray();

        var cars = xmlParser.Serialize<ExportBMWCarsDTO[]>(BMWCars, "cars");

        return cars;
    }
}