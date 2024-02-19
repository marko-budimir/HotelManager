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
        private readonly IHotelServiceRepository _HotelServiceRepository;

        public HotelServiceService(IHotelServiceRepository hotelServiceRepository)
        {
            _HotelServiceRepository = hotelServiceRepository;
        }

        public async Task<IEnumerable<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter)
        {
            var services = await _HotelServiceRepository.GetAllAsync(paging, sorting, hotelServiceFilter);
            if(services == null)
            {
                throw new ArgumentException("No services found!");
            }
            return services;
        }

        public async Task<HotelService> GetByIdAsync(Guid id)
        {
            var service = await _HotelServiceRepository.GetByIdAsync(id);
            if(service == null)
            {
                return null;
            }
            return service;
        }

        public Task<bool> CreateServiceAsync(HotelService hotelService)
        {
            try
            {
                return _HotelServiceRepository.CreateServiceAsync(hotelService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UpdateServiceAsync(Guid Id, HotelService hotelService)
        {
            try
            {
                return _HotelServiceRepository.UpdateServiceAsync(Id, hotelService);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> DeleteServiceAsync(Guid Id)
        {
            try
            {
                return _HotelServiceRepository.DeleteServiceAsync(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
