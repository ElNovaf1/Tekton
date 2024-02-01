using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.Service.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<Product, NewProductDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ReverseMap();

            CreateMap<Product, EditProductDTO>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
           .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
           .ReverseMap();

            //CreateMap<Cliente, ReporteMovimientosClienteDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            //.ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad))
            //.ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
            //.ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
            //.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));

            //CreateMap<Cliente, CreateClienteDTO>()
            //.ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombre))
            //.ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad))
            //.ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
            //.ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
            //.ForMember(dest => dest.Contraseña, opt => opt.MapFrom(src => src.Contraseña))
            //.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
            //.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
            //.ReverseMap();

            //CreateMap<Cliente, UpdateClienteDTO>()
            //.ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombre))
            //.ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Edad))
            //.ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
            //.ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
            //.ForMember(dest => dest.Contraseña, opt => opt.MapFrom(src => src.Contraseña))
            //.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
            //.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
            //.ReverseMap();
        }
    }
}