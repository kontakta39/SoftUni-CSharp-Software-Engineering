using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System;

namespace ProductShop;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        CreateMap<UserDTO, User>();
        CreateMap<ProductDTO, Product>();
        CreateMap<CategoryDTO, Category>();
        CreateMap<CategoryProductDTO, CategoryProduct>();

        CreateMap<Product, ExportProductsDTO>();
        CreateMap<User, ExportSoldProductsDTO>();
        CreateMap<Category, ExportCategoriesDTO>();
        CreateMap<User, AllUsersCountDTO>();
    }
}