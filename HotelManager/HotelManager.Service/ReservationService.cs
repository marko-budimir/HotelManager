using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter)
        {
            return await _reservationRepository.GetAllAsync(paging, sorting, reservationFilter);
        }

        public async Task<Reservation> GetByIdAsync(Guid id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }

        public Task<Reservation> PostAsync(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationUpdate> UpdateAsync(Guid id, ReservationUpdate reservationUpdate)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            throw new NotImplementedException();
        }
    }
}
