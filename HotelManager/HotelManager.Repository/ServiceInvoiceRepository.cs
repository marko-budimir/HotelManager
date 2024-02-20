using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class ServiceInvoiceRepository : IServiceInvoiceRepository
    {
        private const string _connectionString = "Server = 127.0.0.1;Port=5432;Database=HotelManager;User Id = postgres;Password=postgres;";

        public async Task<List<IServiceInvoice>> GetAllInvoiceServiceAsync(Sorting sorting, Paging paging)
        {
            List<IServiceInvoice> invoiceService = new List<IServiceInvoice>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM \"InvoiceService\" WHERE \"InvoiceService\".\"IsActive\"=true";
                ApplySorting(command, sorting);
                int itemCount = await GetItemCountAsync();
                ApplyPaging(command, paging, itemCount);

                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        invoiceService.Add(new ServiceInvoice()
                        {
                            Id = (Guid)reader["Id"],
                            NumberOfService = (int)reader["NumberOfService"],
                            InvoiceId = (Guid)reader["InvoiceId"],
                            ServiceId = (Guid)reader["ServiceId"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
                catch (NpgsqlException e)
                {
                    throw e;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
            return invoiceService;
        }

        
        public async Task<string> CreateInvoiceAsync(IServiceInvoice newServiceInvoice)
        {

            if (newServiceInvoice == null)
            {
                return "Invoice object is null";
            }
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            int numberOfAffectedRows;

            using (connection)
            {
                string insert = $"INSERT INTO \"InvoiceService\" (\"Id\",\"NumberOfService\",\"InvoiceId\",\"ServiceId\",\"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") VALUES (@id,@numOfService,@invoiceId,@serviceId, @createdBy, @updatedBy, @dateCreated, @dateUpdated, true)";
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", newServiceInvoice.Id);
                command.Parameters.AddWithValue("numOfService", newServiceInvoice.NumberOfService);
                command.Parameters.AddWithValue("invoiceId", newServiceInvoice.InvoiceId);
                command.Parameters.AddWithValue("serviceId", newServiceInvoice.ServiceId);
                command.Parameters.AddWithValue("createdBy", newServiceInvoice.CreatedBy);
                command.Parameters.AddWithValue("updatedBy", newServiceInvoice.UpdatedBy);
                command.Parameters.AddWithValue("dateCreated", newServiceInvoice.DateCreated);
                command.Parameters.AddWithValue("dateUpdated", newServiceInvoice.DateUpdated);
                numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return ("Invoice added");
            }
            return ("Invoice not added");


        }

        public async Task<int> UpdateAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"UPDATE \"InvoiceService\" " +
                    $"SET \"IsActive\" = false " + $"WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                try
                {
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
                catch (NpgsqlException e)
                {
                    throw e;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
        

        private void ApplySorting(NpgsqlCommand command, Sorting sorting)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);
            commandText.Append(" ORDER BY \"");
            commandText.Append(sorting.SortBy).Append("\" ");
            commandText.Append(sorting.SortOrder == "ASC" ? "ASC" : "DESC");
            command.CommandText = commandText.ToString();
        }

        private void ApplyPaging(NpgsqlCommand command, Paging paging, int itemCount)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);
            int currentItem = (paging.PageNum - 1) * paging.PageSize;
            if (currentItem >= 0 && currentItem < itemCount)
            {
                commandText.Append(" LIMIT ").Append(paging.PageSize).Append(" OFFSET ").Append(currentItem);
                command.CommandText = commandText.ToString();
            }
            else
            {
                commandText.Append(" LIMIT 10");
                command.CommandText = commandText.ToString();
            }
        }

        private async Task<int> GetItemCountAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"InvoiceService\"";
                command.Connection = connection;
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    return reader.GetInt32(0);
                }
                catch (Exception e)
                {
                    return 0;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}
