using AutoMapper;
using Swap_Shop.Domain.Entities;
using SwapShop.Application.Commands;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Application.Queries.Auth;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Request.ListingItem;


namespace ProjectX.Api.MappingProfile
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {

            
            CreateMap<RegisterCommand, SignUp>().ReverseMap();
            CreateMap<LoginQuery, SignInModel>().ReverseMap();
            CreateMap<ListItemCommand, ListItemReq>().ReverseMap();
            CreateMap<UpdateUserDto, UpdateUserInfoCommand>();
            CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();
            CreateMap<UpdateUserDto
, UpdateUserInfoCommand>().ReverseMap();
               

            

        }
    }
}