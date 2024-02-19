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
    public class HotelServiceRepository : IHotelServiceRepository
    {
        private const string _ConnectionString = "Host=localhost:5432;" +
         "Username=postgres;" +
         "Password=postgres;" +
         "Database=HotelManager";

        public async Task<IEnumerable<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter)
        {
            var services = new List<HotelService>();

            using (var connection = new NpgsqlConnection(_ConnectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT \"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\" FROM \"Service\" WHERE 1 = 1";

                if (hotelServiceFilter != null)
                {
                    if (!string.IsNullOrWhiteSpace(hotelServiceFilter.SearchQuery))
                    {
                        query += " AND (\"Name\" ILIKE @SearchQuery OR \"Description\" ILIKE @SearchQuery)";
                    }

                    if (hotelServiceFilter.MinPrice.HasValue)
                    {
                        query += " AND \"Price\"::money >= @MinPrice::money";
                    }

                    if (hotelServiceFilter.MaxPrice.HasValue)
                    {
                        query += " AND \"Price\"::money <= @MaxPrice::money";
                    }
                }

                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (hotelServiceFilter != null)
                    {
                        if (!string.IsNullOrWhiteSpace(hotelServiceFilter.SearchQuery))
                        {
                            command.Parameters.AddWithValue("@SearchQuery", $"%{hotelServiceFilter.SearchQuery}%");
                        }

                        if (hotelServiceFilter.MinPrice.HasValue)
                        {
                            command.Parameters.AddWithValue("@MinPrice", hotelServiceFilter.MinPrice.Value);
                        }

                        if (hotelServiceFilter.MaxPrice.HasValue)
                        {
                            command.Parameters.AddWithValue("@MaxPrice", hotelServiceFilter.MaxPrice.Value);
                        }
                    }

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

            using (var connection = new NpgsqlConnection(_ConnectionString))
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

        public async Task<bool> CreateServiceAsync(HotelService newService)
        {
            int rowsChanged;
            NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString);

            using (connection)
            {
                string insertQuery = "INSERT INTO \"Service\" (\"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
                    "VALUES (@Id, @Name, @Description, @Price, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

                // Dodjeljivanje novog Guida Id-u
                newService.Id = Guid.NewGuid();

                // Dodavanje parametara
                insertCommand.Parameters.AddWithValue("@Id", newService.Id);
                insertCommand.Parameters.AddWithValue("@Name", newService.Name);
                insertCommand.Parameters.AddWithValue("@Description", newService.Description);
                insertCommand.Parameters.AddWithValue("@Price", newService.Price);
                insertCommand.Parameters.AddWithValue("@CreatedBy", newService.CreatedBy);
                insertCommand.Parameters.AddWithValue("@UpdatedBy", newService.UpdatedBy);
                insertCommand.Parameters.AddWithValue("@DateCreated", newService.DateCreated);
                insertCommand.Parameters.AddWithValue("@DateUpdated", newService.DateUpdated);
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

        public async Task<bool> UpdateServiceAsync(Guid id, HotelService service)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString);

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

                updateQuery += " WHERE \"Id\" = @Id";

                using (connection)
                {
                    NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);

                    // Dodavanje parametara
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@Name", service.Name ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@Description", service.Description ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@Price", service.Price != default(decimal) ? service.Price : (object)DBNull.Value);

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

        public async Task<bool> DeleteServiceAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString);

            try
            {
                string updateQuery = "UPDATE \"Service\" SET \"IsActive\" = FALSE WHERE \"Id\" = @Id";

                using (connection)
                {
                    NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@Id", id);

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
