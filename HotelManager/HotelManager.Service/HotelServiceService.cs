using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service
{
    public class HotelServiceService : IHotelServiceService
    {
        private readonly IHotelServiceRepository _DashboardServiceRepository;

        public HotelServiceService(IHotelServiceRepository hotelServiceRepository)
        {
            _DashboardServiceRepository = hotelServiceRepository;
        }

        public async Task<IEnumerable<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter)
        {
            var services = await _DashboardServiceRepository.GetAllAsync(paging, sorting, hotelServiceFilter);
            if(services == null)
            {
                throw new ArgumentException("No services found!");
            }
            return services;
        }
    }
}
