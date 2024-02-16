using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace HotelManager.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";

        public async Task<List<IReceipt>> GetAllAsync(ReceiptFilter filter, Sorting sorting, Paging paging)
        {
            List<IReceipt> receipts = new List<IReceipt>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                ApplyFilter(command, filter);
                ApplySorting(command, sorting);
                int itemCount = await GetItemCountAsync(filter);
                ApplyPaging(command, paging, itemCount);

                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        receipts.Add(new Receipt()
                        {
                            Id = (Guid)reader["Id"],
                            TotalPrice = (decimal)reader["TotalPrice"],
                            IsPaid = (bool)reader["IsPaid"],
                            ReservationId = (Guid)reader["ReservationId"],
                            DiscountId = (Guid)reader["DiscountId"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            InvoiceNumber = (Guid)reader["InvoiceNumber"]
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
            return receipts;
        }




        public async Task<IReceipt> GetByIdAsync(Guid id)
        {
            IReceipt receipt = null;
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            string commandText = $"SELECT * FROM \"Receipt\" WHERE \"Id\" = @id";
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        if (receipt == null)
                        {
                            receipt = new Receipt()
                            {
                                Id = (Guid)reader["Id"],
                                TotalPrice = (decimal)reader["TotalPrice"],
                                IsPaid = (bool)reader["IsPaid"],
                                ReservationId = (Guid)reader["ReservationId"],
                                DiscountId = (Guid)reader["DiscountId"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"],
                                InvoiceNumber = (Guid)reader["InvoiceNumber"]
                            };
                        }
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
            return receipt;
        }


        public async Task<int> UpdateAsync(Guid id, IReceipt newReceipt)
        {
            IReceipt receipt = await GetByIdAsync(id);
            if (receipt == null)
            {
                return 0;
            }
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"UPDATE \"Receipt\" " +
                    $"SET \"IsActive\" = @isActive " + $"WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("isActive", newReceipt.IsActive ? receipt.IsActive : false);
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
        private void ApplyFilter(NpgsqlCommand command, ReceiptFilter filter)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);

            commandText.Append($"SELECT * FROM \"Invoice\"");
            if (filter.minPrice > 0 && filter.maxPrice < 1000)
            {
                commandText.Append(" WHERE 1=1");
                commandText.Append(" AND \"Invoice\".\"TotalPrice\" BETWEEN @minPrice AND @maxPrice");
                command.Parameters.AddWithValue("minPrice", filter.minPrice);
                command.Parameters.AddWithValue("maxPrice", filter.maxPrice);
            }
            if (filter.userEmailQuery != null)
            {
                commandText.Append($" LEFT JOIN \"Reservation\" ON \"Invoice\".\"ReservationId\" = \"Reservation\".\"Id\" LEFT JOIN \"User\" ON \"Reservation\".\"UserId\" = \"User\".\"Id\" WHERE \"User\".\"Email\" LIKE @emailQuery");
                command.Parameters.AddWithValue("@emailQuery", $"%{filter.userEmailQuery}%");
            }
            if (filter.isPaid != false)
            {
                commandText.Append(" WHERE 1=1");
                commandText.Append(" AND \"Invoice\".\"IsPaid\" = @isPaid");
                command.Parameters.AddWithValue("isPaid", filter.isPaid);
            }
            if(filter.dateCreated != null && filter.dateUpdated != null)
            {
                commandText.Append(" WHERE 1=1");
                commandText.Append(" AND \"Invoice\".\"DateCreated\" BETWEEN @startDate AND @endDate");
                command.Parameters.AddWithValue("startDate", filter.dateCreated);
                command.Parameters.AddWithValue("endDate", filter.dateUpdated);
            }
            command.CommandText = commandText.ToString();
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

        private async Task<int> GetItemCountAsync(ReceiptFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Receipt\"";
                ApplyFilter(command, filter);
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