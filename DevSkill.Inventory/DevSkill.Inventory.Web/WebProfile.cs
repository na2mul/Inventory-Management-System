using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;

namespace DevSkill.Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ProductAddCommand, Product>().ReverseMap();
            CreateMap<ProductUpdateCommand, Product>().ReverseMap();
            CreateMap<ProductSearchModel, ProductSearchDto>().ReverseMap();
        }
    }
}
