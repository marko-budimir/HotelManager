﻿using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IRoomService
    {
        Task<Room> GetByIdAsync(Guid id);
        Task<IEnumerable<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter);

    }
}
