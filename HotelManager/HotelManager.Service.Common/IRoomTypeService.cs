﻿using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IRoomTypeService
    {
        Task<RoomType> GetByIdAsync(Guid id);
        Task<IEnumerable<RoomType>> GetAllAsync(Paging paging, Sorting sorting);

        Task<RoomTypeUpdate> UpdateAsync(Guid id, RoomTypeUpdate roomTypeUpdate);
        Task<RoomType> PostAsync(RoomTypePost roomTypePost);
    }
}
