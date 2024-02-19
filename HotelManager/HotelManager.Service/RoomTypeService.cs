using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository RoomTypeRepository;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            RoomTypeRepository = roomTypeRepository;
        }

        public async Task<IEnumerable<RoomType>> GetAllAsync(Paging paging, Sorting sorting)
        {
            return await RoomTypeRepository.GetAllAsync(paging,sorting);
        }

        public async Task<RoomType> GetByIdAsync(Guid id)
        {
            return await RoomTypeRepository.GetByIdAsync(id);
        }

        public async Task<RoomType> PostAsync(RoomTypePost roomTypePost)
        {
            return await RoomTypeRepository.PostAsync(roomTypePost);
        }

        public  async Task<RoomTypeUpdate> UpdateAsync(Guid id, RoomTypeUpdate roomTypeUpdate)
        {
            return await RoomTypeRepository.UpdateAsync(id, roomTypeUpdate);    
        }
       
    }
}
