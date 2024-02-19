using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;" +
         "Username=postgres;" +
         "Password=postgres;" +
         "Database=postgres";
        public async Task<IEnumerable<RoomType>> GetAllAsync(Paging paging, Sorting sorting)
        {
            var roomTypes = new List<RoomType>();

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM \"RoomType\"");

                using (var cmd = new NpgsqlCommand())
                {
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
                            var roomType = new RoomType
                            {
                                Id = (Guid)(reader["Id"]),
                                Name = (String)reader["Name"],
                                Description = (String)reader["Description"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                            roomTypes.Add(roomType);
                        }
                    }
                }
            }
            return roomTypes;
        }

        public async Task<RoomType> GetByIdAsync(Guid id)
        {
            var roomType = new RoomType();

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM \"RoomType\" WHERE \"Id\" = @Id";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            roomType = new RoomType
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
            }
            return roomType;
        }
        //Need to set who created roomtype
        public async Task<RoomType> PostAsync(RoomTypePost roomTypePost)
        {
            Guid userId = new Guid("73dd2485-b158-420a-86ca-599c3abba0aa");
            Guid creationId = Guid.NewGuid();

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO \"RoomType\" (\"Id\", \"Name\", \"Description\", \"IsActive\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\") " +
                            "VALUES (@Id, @Name, @Description, @IsActive, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated)";

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Parameters.AddWithValue("@Id",creationId);
                    cmd.Parameters.AddWithValue("@Name",roomTypePost.Name);
                    cmd.Parameters.AddWithValue("@Description", roomTypePost.Description);
                    cmd.Parameters.AddWithValue("@IsActive", roomTypePost.IsActive);
                    // set user who created
                    cmd.Parameters.AddWithValue("@CreatedBy", userId);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);

                    cmd.Connection = connection;
                    cmd.CommandText = query.ToString();
                    await cmd.ExecuteNonQueryAsync();
                }
                
            }
            return await GetByIdAsync(creationId);

        }

        


        //Need to update parametar UpdatedBy
        public async Task<RoomTypeUpdate> UpdateAsync(Guid id, RoomTypeUpdate roomTypeUpdate)
        {
            RoomType roomType = await GetByIdAsync(id);

            if (roomTypeUpdate == null)
                return null;
            if (roomType == null)
                return null;


            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"RoomType\" SET ");

                using (var cmd = new NpgsqlCommand())
                {
                    if (roomTypeUpdate.Name != null)
                    {
                        cmd.Parameters.AddWithValue("@Name", roomTypeUpdate.Name);
                        queryBuilder.AppendLine(" \"Name\" = @Name,");
                    }

                    if (roomTypeUpdate.Description != null)
                    {
                        cmd.Parameters.AddWithValue("@Description", roomTypeUpdate.Description);
                        queryBuilder.AppendLine(" \"Description\" = @Description,");
                    }

                    if (roomTypeUpdate.IsActive != null)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", roomTypeUpdate.IsActive);
                        queryBuilder.AppendLine(" \"IsActive\" = @IsActive,");
                    }

                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    queryBuilder.AppendLine(" \"DateUpdated\" = @DateUpdated,");

                    // need update on userId who updated
                    cmd.Parameters.AddWithValue("@UpdatedBy", roomType.CreatedBy);
                    queryBuilder.AppendLine(" \"UpdatedBy\" = @UpdatedBy");

                    cmd.Parameters.AddWithValue("@id", id);
                    queryBuilder.AppendLine(" WHERE \"Id\" = @id");


                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    await cmd.ExecuteNonQueryAsync();


                    roomType = await GetByIdAsync(id);
                    RoomTypeUpdate editedRoomType = new RoomTypeUpdate();
                    editedRoomType = SetValue(roomType, editedRoomType);
                    return editedRoomType;
                }
            }

        }


        private RoomTypeUpdate SetValue(RoomType roomType, RoomTypeUpdate editedRoomType)
        {
            editedRoomType.UpdatedBy = roomType.UpdatedBy;
            editedRoomType.DateUpdated = roomType.DateUpdated;
            editedRoomType.Description = roomType.Description;
            editedRoomType.Name=roomType.Name;
            editedRoomType.IsActive = roomType.IsActive;
            return editedRoomType;
        }
    }
}
