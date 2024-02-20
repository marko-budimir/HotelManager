using AutoMapper;
using HotelManager.Model;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.AutoMapper
{
    public class UserMapperPofile : Profile
    {
        public UserMapperPofile()
        {
            CreateMap<User, ProfileView>();
            CreateMap<ProfileView, User>();
            CreateMap<ProfileRegistered, User>();
            CreateMap<ProfileUpdated, User>();
        }
    }
}