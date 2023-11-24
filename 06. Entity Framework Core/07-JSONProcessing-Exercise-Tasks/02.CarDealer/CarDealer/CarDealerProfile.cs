using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierDTO, Supplier>();
            CreateMap<PartDTO, Part>();
            CreateMap<CarDTO, Car>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<SaleDTO, Sale>();

            CreateMap<Customer, ExportCustomerDTO>();
            CreateMap<Car, ExportCarDTO>();
            CreateMap<Supplier, ExportSupplierDTO>();
            CreateMap<Car, ExportCarWithParts>()
             .ForMember(dest => dest.Parts, opt => opt.MapFrom(src => src.PartsCars));
            CreateMap<Customer, ExportCustomerTotalSales>()
            .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src.Sales));
            CreateMap<Sale, ExportSalesDiscountsDTO>()
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
               .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car))
               .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));
        }
    }
}