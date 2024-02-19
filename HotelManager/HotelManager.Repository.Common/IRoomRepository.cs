using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{

    public interface IRoomRepository
    {
        Task<Room> GetByIdAsync(Guid id);
        Task<IEnumerable<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter);

        Task <RoomUpdate> UpdateRoomAsync(Guid id,RoomUpdate roomUpdate);
        Task <RoomUpdate> GetRoomUpdateByIdAsync(Guid id);
        Task<IEnumerable<RoomUpdate>> GetUpdatedRoomsAsync(Paging paging, Sorting sorting, RoomFilter roomsFilter);
    }
}

