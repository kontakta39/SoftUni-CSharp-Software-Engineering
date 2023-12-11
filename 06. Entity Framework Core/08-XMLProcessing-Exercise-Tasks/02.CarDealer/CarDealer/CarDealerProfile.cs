using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        //09.Import Suppliers
        CreateMap<ImportSupplierDto, Supplier>();

        //10.Import Suppliers
        CreateMap<ImportPartDto, Part>();

        //11.Import Cars
        CreateMap<ImportCarsDto, Car>();

        //12.Import Customers
        CreateMap<ImportCustomerDto, Customer>();

        //13.Import Sales
        CreateMap<ImportSaleDto, Sale>();

        //16.Export Local Suppliers
        CreateMap<Supplier, ExportLocalSupplierDto>();

        //17.Export Cars With Their List Of Parts
        CreateMap<Car, ExportCarPartsDto>();

        //18.Export Total Sales by Customer
        CreateMap<Customer, TotalSalesByCustomerDto>();

        //19.Export Total Sales by Customer
        CreateMap<Sale, SalesWithAppliedDiscountDto>();
    }
}