using AutoMapper;
using Core.Persistence.Paging;
using Komut.Captech.ProductService.Application.Features.Products.Commands.ProductCreate;
using Komut.Captech.ProductService.Application.Features.Products.Dtos;
using Komut.Captech.ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komut.Captech.ProductService.Application.Features.Products.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, CreatedProductDto>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            //CreateMap<IPaginate<Brand>, BrandListModel>().ReverseMap();
            //CreateMap<Brand, BrandListDto>().ReverseMap();
            //CreateMap<Brand, BrandGetByIdDto>().ReverseMap();
        }
    }
}
