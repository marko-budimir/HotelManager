﻿using HotelManager.Common;
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
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
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
        public async Task<InvoiceReceipt> GetByIdAsync(Guid id)
        {
            InvoiceReceipt receipt = null;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT i.*, r.\"CheckInDate\", r.\"CheckOutDate\", r.\"PricePerNight\", " +
                        "rm.\"Number\" AS \"RoomNumber\", d.\"Code\" AS \"DiscountCode\", d.\"Percent\" AS \"DiscountPercent\", u.\"FirstName\", u.\"LastName\", u.\"Email\" " +
                        "FROM \"Invoice\" i " +
                        "JOIN \"Reservation\" r ON i.\"ReservationId\" = r.\"Id\" " +
                        "JOIN \"Room\" rm ON r.\"RoomId\" = rm.\"Id\" " +
                        "LEFT JOIN \"Discount\" d ON i.\"DiscountId\" = d.\"Id\" " +
                        "JOIN \"User\" u ON r.\"UserId\" = u.\"Id\" " +
                        "WHERE i.\"Id\" = @Id AND i.\"IsActive\"=true";
    
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                command.Parameters.AddWithValue("Id", id);
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        if (receipt == null)
                        {
                            receipt = new InvoiceReceipt()
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
                                InvoiceNumber = (string)reader["InvoiceNumber"],
                                CheckInDate = reader.GetDateTime(reader.GetOrdinal("CheckInDate")),
                                CheckOutDate = reader.GetDateTime(reader.GetOrdinal("CheckOutDate")),
                                PricePerNight = (decimal)reader["PricePerNight"],
                                RoomNumber = (int)reader["RoomNumber"],
                                DiscountCode = reader["DiscountCode"] != DBNull.Value ? (string)reader["DiscountCode"] : null,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? (int)reader["DiscountPercent"] : 0,
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Email = (string)reader["Email"]
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
        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Invoice\" (");
                queryBuilder.AppendLine("    \"Id\", \"TotalPrice\", \"IsPaid\", \"ReservationId\", \"DiscountId\", ");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", ");
                queryBuilder.AppendLine("    \"IsActive\", \"InvoiceNumber\"");
                queryBuilder.AppendLine(") VALUES (");
                queryBuilder.AppendLine("    @Id, @TotalPrice, @IsPaid, @ReservationId, @DiscountId, ");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, ");
                queryBuilder.AppendLine("    @IsActive, @InvoiceNumber");
                queryBuilder.AppendLine(")");

                string query = queryBuilder.ToString();


                NpgsqlCommand command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", invoice.Id);
                command.Parameters.AddWithValue("@TotalPrice", invoice.TotalPrice);
                command.Parameters.AddWithValue("@IsPaid", invoice.IsPaid);
                command.Parameters.AddWithValue("@ReservationId", invoice.ReservationId);
                command.Parameters.AddWithValue("@DiscountId", invoice.DiscountId);
                command.Parameters.AddWithValue("@CreatedBy", invoice.CreatedBy);
                command.Parameters.AddWithValue("@UpdatedBy", invoice.UpdatedBy);
                command.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow);
                command.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                command.Parameters.AddWithValue("@IsActive", invoice.IsActive);
                command.Parameters.AddWithValue("@InvoiceNumber", invoice.InvoiceNumber);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return invoice;
                    }
                    else
                    {
                        throw new Exception("Failed to create the invoice.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while creating the invoice.", ex);
                }

            }
        }

        public async Task<Invoice> PutTotalPriceAsync(Guid invoiceId, InvoiceUpdate invoiceUpdate)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string query = "UPDATE \"Invoice\" SET \"TotalPrice\" = @TotalPrice, \"UpdatedBy\" = @UpdatedBy, \"DateUpdated\" = @DateUpdated, \"IsActive\" = @IsActive WHERE \"Id\" = @Id";

                NpgsqlCommand command = new NpgsqlCommand(query, connection);

                command.Parameters.AddWithValue("@IsActive", invoiceUpdate.IsActive);
                command.Parameters.AddWithValue("@TotalPrice", invoiceUpdate.TotalPrice);
                command.Parameters.AddWithValue("@UpdatedBy", invoiceUpdate.UpdatedBy);
                command.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", invoiceId);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return new Invoice
                        {
                            Id = invoiceId,
                            TotalPrice = invoiceUpdate.TotalPrice,
                            UpdatedBy = invoiceUpdate.UpdatedBy,
                            DateUpdated = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        throw new Exception("Failed to update the total price of the invoice.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the total price of the invoice.", ex);
                }
            }
        }
    }
}
