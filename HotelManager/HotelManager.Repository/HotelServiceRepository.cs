using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class HotelServiceRepository : IHotelServiceRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<IEnumerable<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter)
        {
            var services = new List<HotelService>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = new StringBuilder("SELECT \"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\" FROM \"Service\" WHERE 1=1");

                // Filter
                if (hotelServiceFilter != null)
                {
                    if (!string.IsNullOrWhiteSpace(hotelServiceFilter.SearchQuery))
                    {
                        query.Append(" AND (\"Name\" ILIKE @SearchQuery OR \"Description\" ILIKE @SearchQuery)");
                    }

                    if (hotelServiceFilter.MinPrice.HasValue)
                    {
                        query.Append(" AND \"Price\" >= @MinPrice");
                    }

                    if (hotelServiceFilter.MaxPrice.HasValue)
                    {
                        query.Append(" AND \"Price\" <= @MaxPrice");
                    }
                }

                // Sorting
                if (!string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    query.Append($" ORDER BY \"{sorting.SortBy}\" {(sorting.SortOrder.ToUpper() == "ASC" ? "ASC" : "DESC")}");
                }
                else
                {
                    query.Append(" ORDER BY \"DateCreated\" DESC");
                }

                // Pagination
                query.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

                using (var command = new NpgsqlCommand(query.ToString(), connection))
                {
                    if (hotelServiceFilter != null)
                    {
                        command.Parameters.AddWithValue("@SearchQuery", $"%{hotelServiceFilter.SearchQuery}%");
                        command.Parameters.AddWithValue("@MinPrice", hotelServiceFilter.MinPrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@MaxPrice", hotelServiceFilter.MaxPrice ?? (object)DBNull.Value);
                    }

                    int offset = (paging.PageNum - 1) * paging.PageSize;
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", paging.PageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var service = new HotelService
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                UpdatedBy = Guid.Parse(reader["UpdatedBy"].ToString()),
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };

                            services.Add(service);
                        }
                    }
                }
            }

            return services;
        }

        public async Task<HotelService> GetByIdAsync(Guid id)
        {
            HotelService service = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT \"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\" FROM \"Service\" WHERE \"Id\" = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            service = new HotelService
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                UpdatedBy = Guid.Parse(reader["UpdatedBy"].ToString()),
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
            }

            return service;
        }

        public async Task<bool> CreateServiceAsync(HotelService newService, Guid userId)
        {
            int rowsChanged;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            using (connection)
            {
                string insertQuery = "INSERT INTO \"Service\" (\"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
                    "VALUES (@Id, @Name, @Description, @Price, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

                newService.Id = Guid.NewGuid();

                insertCommand.Parameters.AddWithValue("@Id", newService.Id);
                insertCommand.Parameters.AddWithValue("@Name", newService.Name);
                insertCommand.Parameters.AddWithValue("@Description", newService.Description);
                insertCommand.Parameters.AddWithValue("@Price", newService.Price);
                insertCommand.Parameters.AddWithValue("@CreatedBy", userId);
                insertCommand.Parameters.AddWithValue("@UpdatedBy", userId);
                insertCommand.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow);
                insertCommand.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                insertCommand.Parameters.AddWithValue("@IsActive", newService.IsActive);

                try
                {
                    await connection.OpenAsync();
                    rowsChanged = await insertCommand.ExecuteNonQueryAsync();
                    return rowsChanged != 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<bool> UpdateServiceAsync(Guid id, HotelService service, Guid userId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try
            {
                string updateQuery = "UPDATE \"Service\" SET ";
                bool hasSet = false;

                if (!string.IsNullOrEmpty(service.Name))
                {
                    updateQuery += "\"Name\" = @Name";
                    hasSet = true;
                }

                if (!string.IsNullOrEmpty(service.Description))
                {
                    if (hasSet)
                        updateQuery += ", ";

                    updateQuery += "\"Description\" = @Description";
                    hasSet = true;
                }

                if (service.Price != default(decimal))
                {
                    if (hasSet)
                        updateQuery += ", ";

                    updateQuery += "\"Price\" = @Price";
                    hasSet = true;
                }

                if (!hasSet)
                {
                    return false;
                }
                updateQuery += ", \"UpdatedBy\" = @UpdatedBy, \"DateUpdated\" = @DateUpdated";
                updateQuery += " WHERE \"Id\" = @Id";

                using (connection)
                {
                    NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@Name", service.Name ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@Description", service.Description ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@Price", service.Price != default(decimal) ? service.Price : (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@UpdatedBy", userId);
                    updateCommand.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);

                    await connection.OpenAsync();
                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<bool> DeleteServiceAsync(Guid id, Guid userId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try
            {
                string updateQuery = "UPDATE \"Service\" SET \"IsActive\" = FALSE, \"DateUpdated\" = @DateUpdated, \"UpdatedBy\" = @UpdatedBy WHERE \"Id\" = @Id";

                using (connection)
                {
                    NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                    updateCommand.Parameters.AddWithValue("@UpdatedBy", userId);

                    await connection.OpenAsync();
                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
