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

                var query = "SELECT \"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\" FROM \"Service\"";

                using (var command = new NpgsqlCommand(query, connection))
                {
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

    }
}
