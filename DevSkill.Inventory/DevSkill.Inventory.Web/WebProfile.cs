using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.MeasurementUnits.Commands;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Dtos.ProductDtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.Customers;
using DevSkill.Inventory.Web.Areas.Admin.Models.Products;

namespace DevSkill.Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ProductAddCommand, Product>().ReverseMap();
            CreateMap<ProductAddCommand, AddProductModel>().ReverseMap();
            CreateMap<ProductSearchModel, ProductSearchDto>().ReverseMap();
            CreateMap<ProductUpdateCommand, Product>().ReverseMap();
            CreateMap<ProductUpdateCommand, UpdateProductModel>().ReverseMap();
            CreateMap<UpdateProductModel, Product>().ReverseMap();
            CreateMap<ProductGetListQuery, ProductListModel>().ReverseMap();
            CreateMap<ProductStoreCommand, Product>().ReverseMap();
            CreateMap<ProductStoreCommand, StoreProductModel>().ReverseMap();
            CreateMap<ProductDamageCommand, Product>().ReverseMap();
            CreateMap<ProductDamageCommand, DamageProductModel>().ReverseMap();
            CreateMap<CategoryAddCommand, Category>().ReverseMap();
            CreateMap<MeasurementUnitAddCommand, MeasurementUnit>().ReverseMap();
            CreateMap<CustomerAddCommand, Customer>().ReverseMap();
            CreateMap<CustomerAddCommand, AddCustomerModel>().ReverseMap();

        }
    }
}
