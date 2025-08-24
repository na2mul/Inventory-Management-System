using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.MeasurementUnits.Commands;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Sales.Commands;
using DevSkill.Inventory.Application.Features.TransferAccounts.Commands;
using DevSkill.Inventory.Domain.Dtos.ProductDtos;
using DevSkill.Inventory.Domain.Dtos.SaleDtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models.Customers;
using DevSkill.Inventory.Web.Areas.Admin.Models.Products;
using DevSkill.Inventory.Web.Areas.Admin.Models.Sales;
using DevSkill.Inventory.Web.Areas.Admin.Models.TransferAccounts;

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
            CreateMap<CustomerUpdateCommand, Customer>().ReverseMap();
            CreateMap<CustomerUpdateCommand, UpdateCustomerModel>().ReverseMap();
            CreateMap<UpdateCustomerModel, Customer>().ReverseMap();
            CreateMap<TransferAccountAddCommand, TransferAccount>().ReverseMap();
            CreateMap<TransferAccountAddCommand, AddTransferAccountModel>().ReverseMap();

            CreateMap<Sale, SaleAddCommand>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SalesDetails));
            CreateMap<SaleAddCommand, Sale>()

                .ForMember(dest => dest.SalesDetails, opt => opt.MapFrom(src => src.Items));
            CreateMap<AddSaleModel, SaleAddCommand>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));
            CreateMap<SaleAddCommand, AddSaleModel>()

                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
            CreateMap<SaleItemDto, SalesDetail>().ReverseMap();
            CreateMap<SaleProductModel, SaleItemDto>().ReverseMap();
        }
    }
}
