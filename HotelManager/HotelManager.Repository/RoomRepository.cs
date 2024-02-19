using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class RoomRepository : IRoomRepository
    {

        private const string CONNECTION_STRING = "Host=localhost:5432;" +
         "Username=postgres;" +
         "Password=postgres;" +
         "Database=postgres";

        public async Task<IEnumerable<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter)
        {
            var rooms = new List<Room>();

            if (roomFilter != null)
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT r.*, rt.\"Name\", res.\"CheckInDate\", res.\"CheckInDate\"");
                    queryBuilder.AppendLine(" FROM \"Room\" r");
                    queryBuilder.AppendLine(" JOIN \"RoomType\" rt ON r.\"TypeId\" = rt.\"Id\"");
                    queryBuilder.AppendLine(" LEFT JOIN \"Reservation\" res ON r.\"Id\" = res.\"RoomId\"");
                    queryBuilder.AppendLine(" WHERE 1=1");
                    queryBuilder.AppendLine(" AND r.\"IsActive\" = true");

                    using (var cmd = new NpgsqlCommand())
                    {
                        if (roomFilter.StartDate != null && roomFilter.EndDate != null)
                        {
                            cmd.Parameters.AddWithValue("@StartDate", roomFilter.StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", roomFilter.EndDate);
                            queryBuilder.Append(" AND NOT (res.\"CheckOutDate\" >= @StartDate AND res.\"CheckInDate\" <= @EndDate)");
                        }

                        if (roomFilter.MinPrice != null && roomFilter.MaxPrice != null)
                        {
                            cmd.Parameters.AddWithValue("@MinPrice", roomFilter.MinPrice);
                            cmd.Parameters.AddWithValue("@MaxPrice", roomFilter.MaxPrice);
                            queryBuilder.Append(" AND r.\"Price\" BETWEEN @MinPrice::money AND @MaxPrice::money");
                        }

                        if (roomFilter.MinBeds > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinBeds", roomFilter.MinBeds);
                            queryBuilder.Append(" AND r.\"BedCount\" >= @MinBeds");
                        }

                        if (roomFilter.RoomTypeId != null)
                        {
                            cmd.Parameters.AddWithValue("@RoomTypeId", roomFilter.RoomTypeId);
                            queryBuilder.Append(" AND r.\"TypeId\" = @RoomTypeId");
                        }
                        if (sorting != null && !string.IsNullOrEmpty(sorting.SortBy))
                        {
                            queryBuilder.Append(" ORDER BY ");
                            queryBuilder.Append(sorting.SortBy);

                            if (!string.IsNullOrEmpty(sorting.SortOrder))
                            {
                                queryBuilder.Append(" ");
                                queryBuilder.Append(sorting.SortOrder);
                            }
                        }


                        if (paging != null)
                        {
                            cmd.Parameters.AddWithValue("@Limit", paging.PageSize);
                            cmd.Parameters.AddWithValue("@Offset", (paging.PageNum - 1) * paging.PageSize);
                            queryBuilder.Append(" LIMIT @Limit OFFSET @Offset");
                        }


                        cmd.Connection = connection;
                        cmd.CommandText = queryBuilder.ToString();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                rooms.Add(new Room
                                {
                                    Id = (Guid)reader["Id"],
                                    Number = (int)reader["Number"],
                                    BedCount = (int)reader["BedCount"],
                                    Price = (decimal)reader["Price"],
                                    ImageUrl = (string)reader["ImageUrl"],
                                    TypeId = (Guid)reader["TypeId"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DateUpdated"],
                                    IsActive = (bool)reader["IsActive"]

                                });
                            }
                        }
                        return rooms;
                    }
                }
            }
            return rooms;
        }

        public async Task<Room> GetByIdAsync(Guid id)
        {
            Room room = null;

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM \"Room\" WHERE \"Id\" = @Id";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            room = new Room
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Number = reader.GetInt32(reader.GetOrdinal("Number")),
                                BedCount = reader.GetInt32(reader.GetOrdinal("BedCount")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                TypeId = reader.GetGuid(reader.GetOrdinal("TypeId")),
                                CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                UpdatedBy = reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }

            return room;
        }

        //Need to update parametar UpdatedBy
        public async Task<RoomUpdate> UpdateRoomAsync(Guid id, RoomUpdate roomUpdate)
        { 
            Room room = await GetByIdAsync(id);

            if (roomUpdate == null)
                return null;
            if (room == null)
                return null;


            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Room\" SET ");

                using (var cmd = new NpgsqlCommand())
                {
                    if (roomUpdate.Number != null)
                    {
                        cmd.Parameters.AddWithValue("@Number", roomUpdate.Number);
                        queryBuilder.AppendLine(" \"Number\" = @Number,");
                    }

                    if (roomUpdate.BedCount != null)
                    {
                        cmd.Parameters.AddWithValue("@BedCount", roomUpdate.BedCount);
                        queryBuilder.AppendLine(" \"BedCount\" = @BedCount,");
                    }

                    if (roomUpdate.Price != null)
                    {
                        cmd.Parameters.AddWithValue("@Price", roomUpdate.Price);
                        queryBuilder.AppendLine(" \"Price\" = @Price,");
                    }

                    if (roomUpdate.ImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", roomUpdate.ImageUrl);
                        queryBuilder.AppendLine(" \"ImageUrl\" = @ImageUrl,");
                    }

                    if (roomUpdate.TypeId != null)
                    {
                        cmd.Parameters.AddWithValue("@TypeId", roomUpdate.TypeId);
                        queryBuilder.AppendLine(" \"TypeId\" = @TypeId,");
                    }

                    if (roomUpdate.IsActive != null)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", roomUpdate.IsActive);
                        queryBuilder.AppendLine(" \"IsActive\" = @IsActive,");
                    }


                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    queryBuilder.AppendLine(" \"DateUpdated\" = @DateUpdated,");

                    //Updated by should be admin id who updated it
                    cmd.Parameters.AddWithValue("@UpdatedBy", room.CreatedBy) ;
                    queryBuilder.AppendLine(" \"UpdatedBy\" = @UpdatedBy");

                    cmd.Parameters.AddWithValue("@id", id);
                    queryBuilder.AppendLine(" WHERE \"Id\" = @id");


                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    await cmd.ExecuteNonQueryAsync();

                }
            }

            Room editedRoom = await GetByIdAsync(id);
            RoomUpdate roomUpdated = new RoomUpdate();
            SetValues(editedRoom,roomUpdated);
            return roomUpdated;
        }


        public  async Task<RoomUpdate> GetRoomUpdateByIdAsync(Guid id)
        {
            Room room = await GetByIdAsync(id);
            RoomUpdate roomUpdate = new RoomUpdate();

            SetValues(room, roomUpdate);

            return roomUpdate;
        }

        public async Task<IEnumerable<RoomUpdate>> GetUpdatedRoomsAsync(Paging paging, Sorting sorting, RoomFilter roomsFilter)
        {
            var updatedRooms = new List<RoomUpdate>();

            if (roomsFilter != null)
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT r.*, rt.\"Name\", res.\"CheckInDate\", res.\"CheckInDate\"");
                    queryBuilder.AppendLine(" FROM \"Room\" r");
                    queryBuilder.AppendLine(" JOIN \"RoomType\" rt ON r.\"TypeId\" = rt.\"Id\"");
                    queryBuilder.AppendLine(" LEFT JOIN \"Reservation\" res ON r.\"Id\" = res.\"RoomId\"");
                    queryBuilder.AppendLine(" WHERE 1=1");
                    queryBuilder.AppendLine(" AND r.\"IsActive\" = true");

                    using (var cmd = new NpgsqlCommand())
                    {
                        if (roomsFilter.StartDate != null && roomsFilter.EndDate != null)
                        {
                            cmd.Parameters.AddWithValue("@StartDate", roomsFilter.StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", roomsFilter.EndDate);
                            queryBuilder.AppendLine(" AND (res.\"CheckInDate\" >= @StartDate AND res.\"CheckInDate\" <= @EndDate)");
                        }

                        if (roomsFilter.MinPrice != null && roomsFilter.MaxPrice != null)
                        {
                            cmd.Parameters.AddWithValue("@MinPrice", roomsFilter.MinPrice);
                            cmd.Parameters.AddWithValue("@MaxPrice", roomsFilter.MaxPrice);
                            queryBuilder.AppendLine(" AND r.\"Price\" BETWEEN @MinPrice::money AND @MaxPrice::money");
                        }

                        if (roomsFilter.MinBeds > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinBeds", roomsFilter.MinBeds);
                            queryBuilder.AppendLine(" AND r.\"BedCount\" >= @MinBeds");
                        }

                        if (roomsFilter.RoomTypeId != null)
                        {
                            cmd.Parameters.AddWithValue("@RoomTypeId", roomsFilter.RoomTypeId);
                            queryBuilder.AppendLine(" AND r.\"TypeId\" = @RoomTypeId");
                        }

                        if (sorting != null && !string.IsNullOrEmpty(sorting.SortBy))
                        {
                            queryBuilder.Append(" ORDER BY ");
                            queryBuilder.Append(sorting.SortBy);

                            if (!string.IsNullOrEmpty(sorting.SortOrder))
                            {
                                queryBuilder.Append(" ");
                                queryBuilder.Append(sorting.SortOrder);
                            }
                        }

                        if (paging != null)
                        {
                            cmd.Parameters.AddWithValue("@Limit", paging.PageSize);
                            cmd.Parameters.AddWithValue("@Offset", (paging.PageNum - 1) * paging.PageSize);
                            queryBuilder.Append(" LIMIT @Limit OFFSET @Offset");
                        }

                        cmd.Connection = connection;
                        cmd.CommandText = queryBuilder.ToString();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var roomUpdate = new RoomUpdate();
                                SetValues(new Room
                                {
                                    Id = (Guid)reader["Id"],
                                    Number = (int)reader["Number"],
                                    BedCount = (int)reader["BedCount"],
                                    Price = (decimal)reader["Price"],
                                    ImageUrl = (string)reader["ImageUrl"],
                                    TypeId = (Guid)reader["TypeId"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DateUpdated"],
                                    IsActive = (bool)reader["IsActive"]
                                }, roomUpdate);

                                updatedRooms.Add(roomUpdate);
                            }
                        }
                    }
                }
            }

            return updatedRooms;
        }




        private static void SetValues(Room room, RoomUpdate roomUpdate)
        {
            roomUpdate.BedCount = room.BedCount;
            roomUpdate.Number = room.Number;
            roomUpdate.DateUpdated = room.DateUpdated;
            roomUpdate.UpdatedBy = room.UpdatedBy;
            roomUpdate.Price = room.Price;
            roomUpdate.ImageUrl = room.ImageUrl;
            roomUpdate.IsActive = room.IsActive;
            roomUpdate.TypeId = room.TypeId;
        }
    }
}
