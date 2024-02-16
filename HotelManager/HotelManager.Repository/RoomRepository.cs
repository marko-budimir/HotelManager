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
                            queryBuilder.Append(" AND r.\"Price\" BETWEEN @MinPrice AND @MaxPrice");
                        }

                        if (roomFilter.MinBeds > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinBeds", roomFilter.MinBeds);
                            queryBuilder.Append(" AND r.\"BedCount\" >= @MinBeds");
                        }

                        if (roomFilter.RoomTypeId != null)
                        {
                            cmd.Parameters.AddWithValue("@uuid", roomFilter.RoomTypeId);
                            queryBuilder.Append(" AND CAST(r.\"TypeId\" AS UUID) = @uuid");
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
                                    DateUpdated = (DateTime)reader["DatedUpdated"],
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
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DatedUpdated")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }

            return room;
        }
    }
}
