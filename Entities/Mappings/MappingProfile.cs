using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source -> Target
            CreateMap<AppUser, AppUserResponse>()
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.UserName));

            CreateMap<AppUser, AppUserViewModel>()
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.UserName));

            CreateMap<AppUserRequest, AppUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(src => src.Email));


            CreateMap<LoginRequest, LoginRequest>();
            CreateMap<AuthenticationResponse, AuthenticationResponse>();

            CreateMap<LoginRequest, LoginModel>();
            CreateMap<Authentication, AuthenticationResponse>();

            CreateMap<Actor, ActorResponse>().ReverseMap();
            CreateMap<ActorRequest, Actor>().ReverseMap();

            CreateMap<Site, SiteResponse>().ReverseMap();
            CreateMap<SiteRequest, Site>().ReverseMap();

            CreateMap<Transaction, TransactionResponse>().ReverseMap();
            CreateMap<TransactionRequest, Transaction>().ReverseMap();


            CreateMap<IdentityRole, RoleResponse>().ReverseMap();
            CreateMap<RoleRequest, IdentityRole>().ReverseMap();

        }
    }
}