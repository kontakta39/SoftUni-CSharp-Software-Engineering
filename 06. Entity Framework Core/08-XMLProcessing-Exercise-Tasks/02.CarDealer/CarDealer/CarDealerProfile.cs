using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer;

public class CarDealerProfile : Profile
{
    public CarDealerProfile()
    {
        CreateMap<ImportSupplierDTO, Supplier>();
        CreateMap<ImportPartDTO, Part>();
        CreateMap<ImportCarDTO, Car>();
        CreateMap<ImportCustomerDTO, Customer>();
        CreateMap<ImportSaleDTO, Sale>();

        //CreateMap<Car, ExportCarsWithDistanceDTO>();
        //CreateMap<Car, ExportBMWCarsDTO>();
    }
}