using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BAL.Entity;
using BAL.Entity.Auth;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Files, FileModel>().ReverseMap();
            CreateMap<User, UserModel>()
                .ForMember(x => x.UserId, r => r.MapFrom(x => x.Id))
                .ReverseMap();
        }
    }
}
