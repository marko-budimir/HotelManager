using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;



namespace HotelManager.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["HotelManager"].ConnectionString;
        public async Task<List<IReceipt>> GetAllAsync(ReceiptFilter filter, Sorting sorting, Paging paging)
        {
            List<IReceipt> receipts = new List<IReceipt>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
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
                            InvoiceNumber = (string)reader["InvoiceNumber"]
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
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string commandText = $"SELECT * FROM \"Invoice\" WHERE \"Id\" = @id AND \"Invoice\".\"IsActive\"=true ";
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
                                Id = (Guid)reader.GetGuid(reader.GetOrdinal("Id")),
                                TotalPrice = (decimal)reader["TotalPrice"],
                                IsPaid = (bool)reader["IsPaid"],
                                ReservationId = reader.GetGuid(reader.GetOrdinal("ReservationId")),
                                DiscountId = reader.GetGuid(reader.GetOrdinal("DiscountId")),
                                CreatedBy = (Guid)reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                UpdatedBy = (Guid)reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                IsActive = (bool)reader["IsActive"],
                                InvoiceNumber = (string)reader["InvoiceNumber"]
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

        private void ApplyFilter(NpgsqlCommand command, ReceiptFilter filter)
        {
            StringBuilder commandText = new StringBuilder();
            if (string.IsNullOrEmpty(filter.userEmailQuery))
            {
                commandText.Append("SELECT * FROM \"Invoice\" WHERE \"Invoice\".\"IsActive\"=true");
            }
            else
            {
                commandText.Append("SELECT * FROM \"Invoice\" ");
            }

            if (!string.IsNullOrEmpty(filter.userEmailQuery))
            {
                commandText.Append(" LEFT JOIN \"Reservation\" ON \"Invoice\".\"ReservationId\" = \"Reservation\".\"Id\"");
                commandText.Append(" LEFT JOIN \"User\" ON \"Reservation\".\"UserId\" = \"User\".\"Id\"");
                commandText.Append(" WHERE \"User\".\"Email\" LIKE @emailQuery");
                commandText.Append(" AND \"Invoice\".\"IsActive\"=true");
                command.Parameters.AddWithValue("@emailQuery", $"%{filter.userEmailQuery}%");
            }

            if (filter.minPrice > 0 && filter.maxPrice < 1000)
            {
                commandText.Append(" AND \"Invoice\".\"TotalPrice\" BETWEEN @minPrice::money AND @maxPrice::money");
                command.Parameters.AddWithValue("minPrice", filter.minPrice);
                command.Parameters.AddWithValue("maxPrice", filter.maxPrice);
            }

            if (filter.isPaid.HasValue)
            {
                commandText.Append(" AND \"Invoice\".\"IsPaid\" = @isPaid");
                command.Parameters.AddWithValue("isPaid", NpgsqlTypes.NpgsqlDbType.Boolean).Value = filter.isPaid.Value;
            }

            if (filter.dateCreated != null)
            {
                commandText.Append(" AND \"Invoice\".\"DateCreated\"=@startDate");
                command.Parameters.AddWithValue("startDate", filter.dateCreated);
            }
            if (filter.dateUpdated != null)
            {
                commandText.Append(" AND \"Invoice\".\"DateUpdated\"=@endDate");
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
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Invoice\"";
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