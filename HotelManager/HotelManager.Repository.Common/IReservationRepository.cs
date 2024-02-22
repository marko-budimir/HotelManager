﻿using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(Guid id);
        Task<IEnumerable<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter);

        Task<ReservationUpdate> UpdateAsync(Guid id, ReservationUpdate reservationUpdate, Guid userId);
        Task<Reservation> PostAsync(Reservation reservationCreate);
        Task<bool> CheckIfAvailable(Guid roomId, DateTime checkInDate, DateTime checkOutDate);
    }
}