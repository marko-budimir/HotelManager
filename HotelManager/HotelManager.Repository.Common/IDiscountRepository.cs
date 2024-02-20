﻿using HotelManager.Common;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IDiscountRepository
    {
        Task<List<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging);
        Task<IDiscount> GetDiscountByIdAsync(Guid id);
        Task<int> UpdateDiscountActiveAsync(Guid id);
        Task<string> CreateDiscountAsync(IDiscount newDiscount, Guid userId);
        Task<string> UpdateDiscountAsync(Guid id ,IDiscount discountUpdated, Guid userId);
        Task<IDiscount> GetDiscountByCodeAsync(string code);
    }
}