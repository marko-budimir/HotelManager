﻿using AutoMapper;
using HotelManager.Model;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.AutoMapper
{
    public class ReceiptMapperProfile : Profile
    {
        public ReceiptMapperProfile()
        {
            CreateMap<Receipt, ReceiptView>();
        }
    }
}