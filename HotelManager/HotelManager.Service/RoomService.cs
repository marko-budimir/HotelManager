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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository RoomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            RoomRepository = roomRepository;
        }
        public async Task<IEnumerable<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter)
  
        {
            var rooms = await RoomRepository.GetAllAsync(paging, sorting, roomFilter);
            if(rooms==null)
                throw new ArgumentException("Rooms not found");
            return rooms;
        }

        public async Task<Room> GetByIdAsync(Guid id)
        {
            var room= await RoomRepository.GetByIdAsync(id);
            if(room == null)
                throw new ArgumentException("Room not found");
            return room;
        }

        public async Task<RoomUpdate> GetRoomUpdateById(Guid id)
        {
            var roomUpdated = await RoomRepository.GetRoomUpdateById(id);
           if(roomUpdated == null)
                return null;
            return roomUpdated;

        }

        public async Task<IEnumerable<RoomUpdate>> GetUpdatedRooms(Paging paging, Sorting sorting, RoomFilter roomsFilter)
        {
            var roomsUpdated = await RoomRepository.GetUpdatedRooms(paging, sorting, roomsFilter);
            if (roomsUpdated == null)
                return null;
            return roomsUpdated;
            
        }

        public async Task<RoomUpdate> UpdateRoom(Guid id, RoomUpdate roomUpdate)
        {
            var room = await RoomRepository.GetByIdAsync(id);
            if(room == null)
                throw new ArgumentException("Room not found");

            return await RoomRepository.UpdateRoomAsync(id,roomUpdate);
        }
    }
}
