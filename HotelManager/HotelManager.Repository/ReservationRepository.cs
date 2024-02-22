﻿using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<bool> CheckIfAvailable(Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            bool isAvailable = false;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT COUNT(*)");
                queryBuilder.AppendLine("FROM \"Reservation\"");
                queryBuilder.AppendLine("WHERE \"RoomId\" = @RoomId");
                queryBuilder.AppendLine("AND \"CheckInDate\" < @CheckOutDate");
                queryBuilder.AppendLine("AND \"CheckOutDate\" > @CheckInDate");
                queryBuilder.AppendLine("AND \"IsActive\" = TRUE");
                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@RoomId", roomId);
                    cmd.Parameters.AddWithValue("@CheckInDate", checkInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                    int result = (int)await cmd.ExecuteScalarAsync();
                    isAvailable = result == 0;
                    
                }
                return isAvailable;
            }
        }


        public async Task<IEnumerable<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter)
        {
            var reservations = new List<ReservationWithUserEmail>();

            if (reservationFilter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT res.*, u.\"Email\"");
                    queryBuilder.AppendLine(" FROM \"Reservation\" res");
                    queryBuilder.AppendLine(" JOIN \"User\" u ON res.\"UserId\" = u.\"Id\"");
                    queryBuilder.AppendLine(" WHERE 1=1");
                    queryBuilder.AppendLine(" AND res.\"IsActive\" = TRUE");


                    using (var cmd = new NpgsqlCommand())
                    {

                        if (reservationFilter.CheckInDate != default && reservationFilter.CheckOutDate != default)
                        {
                            cmd.Parameters.AddWithValue("@StartDate", reservationFilter.CheckInDate);
                            cmd.Parameters.AddWithValue("@EndDate", reservationFilter.CheckOutDate);
                            queryBuilder.AppendLine(" AND NOT (res.\"CheckOutDate\" >= @StartDate AND res.\"CheckInDate\" <= @EndDate)");
                        }

                        if (reservationFilter.MinPricePerNight > 0 && reservationFilter.MaxPricePerNight > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinPrice", reservationFilter.MinPricePerNight);
                            cmd.Parameters.AddWithValue("@MaxPrice", reservationFilter.MaxPricePerNight);
                            queryBuilder.AppendLine(" AND res.\"PricePerNight\" BETWEEN @MinPrice AND @MaxPrice");
                        }

                        if(!string.IsNullOrEmpty(reservationFilter.SearchQuery))
{
                            cmd.Parameters.AddWithValue("@SearchQuery", $"%{reservationFilter.SearchQuery}%");
                            queryBuilder.AppendLine(" AND u.\"Email\" ILIKE @SearchQuery");
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
                            queryBuilder.Append(" LIMIT @Limit OFFSET @Offset");
                            cmd.Parameters.AddWithValue("@Limit", paging.PageSize);
                            cmd.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                        }
                        cmd.Connection = connection;
                        cmd.CommandText = queryBuilder.ToString();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reservations.Add(new ReservationWithUserEmail
                                {
                                    Id = (Guid)reader["Id"],
                                    UserId = (Guid)reader["UserId"],
                                    RoomId = (Guid)reader["RoomId"],
                                    ReservationNumber = (string)reader["ReservationNumber"],
                                    PricePerNight = (decimal)reader["PricePerNight"],
                                    CheckInDate = (DateTime)reader["CheckInDate"],
                                    CheckOutDate = (DateTime)reader["CheckOutDate"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DateUpdated"],
                                    IsActive = (bool)reader["IsActive"],
                                    UserEmail = (string)reader["Email"]

                                });
                            }
                        }
                    }
                }
            }
            return reservations;
        }


        public async Task<Reservation> GetByIdAsync(Guid id)
        {

            var reservation = new Reservation();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM \"Reservation\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                           reservation = new Reservation
                            {
                               Id = (Guid)reader["Id"],
                               UserId = (Guid)reader["UserId"],
                               RoomId = (Guid)reader["RoomId"],
                               ReservationNumber = (string)reader["ReservationNumber"],
                               PricePerNight = (decimal)reader["PricePerNight"],
                               CheckInDate = (DateTime)reader["CheckInDate"],
                               CheckOutDate = (DateTime)reader["CheckOutDate"],
                               CreatedBy = (Guid)reader["CreatedBy"],
                               UpdatedBy = (Guid)reader["UpdatedBy"],
                               DateCreated = (DateTime)reader["DateCreated"],
                               DateUpdated = (DateTime)reader["DateUpdated"],
                               IsActive = (bool)reader["IsActive"],

                           };
                        }
                    }
                }
            }
            return reservation;
        }

        public async Task<Reservation> PostAsync(Reservation reservationCreate)
        {


            var dateNow = DateTime.UtcNow;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Reservation\" (");
                queryBuilder.AppendLine("    \"Id\", \"UserId\", \"RoomId\", \"ReservationNumber\", ");
                queryBuilder.AppendLine("    \"PricePerNight\", \"CheckInDate\", \"CheckOutDate\", ");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @UserId, @RoomId, @ReservationNumber, ");
                queryBuilder.AppendLine("    @PricePerNight, @CheckInDate, @CheckOutDate, ");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", reservationCreate.Id);
                    cmd.Parameters.AddWithValue("@UserId", reservationCreate.CreatedBy);
                    cmd.Parameters.AddWithValue("@RoomId", reservationCreate.RoomId);
                    cmd.Parameters.AddWithValue("@ReservationNumber", reservationCreate.ReservationNumber);
                    cmd.Parameters.AddWithValue("@PricePerNight", reservationCreate.PricePerNight);
                    cmd.Parameters.AddWithValue("@CheckInDate", reservationCreate.CheckInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", reservationCreate.CheckOutDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", reservationCreate.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", reservationCreate.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", dateNow);
                    cmd.Parameters.AddWithValue("@DateUpdated", dateNow);
                    cmd.Parameters.AddWithValue("@IsActive", true);

                    cmd.ExecuteNonQuery();
                }
            }
            reservationCreate.DateCreated = dateNow;
            reservationCreate.DateUpdated = dateNow;
            return reservationCreate;
        }


        public async Task<ReservationUpdate> UpdateAsync(Guid id, ReservationUpdate reservationUpdate, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Reservation\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    if (reservationUpdate.CheckInDate != null)
                    {
                        queryBuilder.AppendLine("    \"CheckInDate\" = @CheckInDate,");
                        cmd.Parameters.AddWithValue("@CheckInDate", reservationUpdate.CheckInDate);
                    }

                    if (reservationUpdate.CheckOutDate != null)
                    {
                        queryBuilder.AppendLine("    \"CheckOutDate\" = @CheckOutDate,");
                        cmd.Parameters.AddWithValue("@CheckOutDate", reservationUpdate.CheckOutDate);
                    }

                    if (!string.IsNullOrEmpty(reservationUpdate.RoomNumber))
                    {
                        queryBuilder.AppendLine("    \"RoomNumber\" = @RoomNumber,");
                        cmd.Parameters.AddWithValue("@RoomNumber", reservationUpdate.RoomNumber);
                    }

                    if (reservationUpdate.PricePerNight != null)
                    {
                        queryBuilder.AppendLine("    \"PricePerNight\" = @PricePerNight,");
                        cmd.Parameters.AddWithValue("@PricePerNight", reservationUpdate.PricePerNight);
                    }

                    if (reservationUpdate.IsActive != null)
                    {
                        queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                        cmd.Parameters.AddWithValue("@IsActive", reservationUpdate.IsActive);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DateUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.CommandText = queryBuilder.ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return reservationUpdate;
        }
    }
}
