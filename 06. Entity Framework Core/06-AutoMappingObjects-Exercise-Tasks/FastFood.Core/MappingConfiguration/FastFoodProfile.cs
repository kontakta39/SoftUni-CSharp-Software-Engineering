namespace FastFood.Core.MappingConfiguration;

using AutoMapper;
using FastFood.Core.ViewModels.Categories;
using FastFood.Core.ViewModels.Employees;
using FastFood.Core.ViewModels.Items;
using FastFood.Core.ViewModels.Orders;
using FastFood.Models;
using System.Security.Cryptography.Xml;
using ViewModels.Positions;

public class FastFoodProfile : Profile
{
    public FastFoodProfile()
    {
        //Positions
        CreateMap<CreatePositionInputModel, Position>()
            .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

        CreateMap<Position, PositionsAllViewModel>()
            .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

        //Employees
        CreateMap<Position, RegisterEmployeeViewModel>()
            .ForMember(x => x.PositionId, y => y.MapFrom(s => s.Id));

        CreateMap<RegisterEmployeeInputModel, Employee>();

        CreateMap<Employee, EmployeesAllViewModel>()
            .ForMember(x => x.Position, y => y.MapFrom(z => z.Position.Name));

        //Categories
        CreateMap<CreateCategoryInputModel, Category>()
            .ForMember(x => x.Name, y => y.MapFrom(z => z.CategoryName));
        CreateMap<Category, CategoryAllViewModel>();

        //Items
        CreateMap<Category, CreateItemViewModel>()
            .ForMember(x => x.CategoryId, y => y.MapFrom(z => z.Id));

        CreateMap<CreateItemInputModel, Item>();

        CreateMap<Item, ItemsAllViewModels>()
            .ForMember(x => x.Category, y => y.MapFrom(z => z.Category.Name));

        //Orders
        CreateMap<Item, CreateOrderViewModel>()
            .ForMember(x => x.Items, y => y.MapFrom(z => z.OrderItems));

        CreateMap<Employee, CreateOrderViewModel>()
            .ForMember(x => x.Employees, y => y.MapFrom(z => z.Id));

        CreateMap<CreateOrderInputModel, Order>();

        CreateMap<Order, OrderAllViewModel>()
           .ForMember(x => x.Item, y => y.MapFrom(z => z.OrderItems.Select(c => c.Item.Id)))
           .ForMember(x => x.Employee, y => y.MapFrom(z => z.Employee.Id))
           .ForMember(x => x.OrderId, y => y.MapFrom(z => z.Id));
    }
}