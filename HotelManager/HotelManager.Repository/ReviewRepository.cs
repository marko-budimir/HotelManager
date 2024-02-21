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
using System.Xml.Linq;

namespace HotelManager.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<IEnumerable<Review>> GetAllAsync(Guid roomId, Paging paging)
        {
            List<Review> reviews = new List<Review>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT r.\"Id\", r.\"Rating\", r.\"Comment\", r.\"UserId\", r.\"RoomId\", r.\"CreatedBy\", r.\"UpdatedBy\", r.\"DateCreated\", r.\"DateUpdated\", r.\"IsActive\", u.\"FirstName\", u.\"LastName\" " +
                               "FROM \"Review\" r " +
                               "JOIN \"User\" u ON r.\"UserId\" = u.\"Id\" " +
                               "WHERE r.\"RoomId\" = @RoomId " +
                               "ORDER BY r.\"DateCreated\" DESC " +
                               "LIMIT @Limit OFFSET @Offset";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@Limit", paging.PageSize);
                    command.Parameters.AddWithValue("@Offset", (paging.PageNum - 1) * paging.PageSize);

                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Review review = new Review
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                Rating = Convert.ToInt32(reader["Rating"]),
                                Comment = reader["Comment"].ToString(),
                                UserId = Guid.Parse(reader["UserId"].ToString()),
                                UserFullName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                                RoomId = Guid.Parse(reader["RoomId"].ToString()),
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                UpdatedBy = Guid.Parse(reader["UpdatedBy"].ToString()),
                                DateCreated = ((DateTime)reader["DateCreated"]).Date,
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                            reviews.Add(review);
                        }
                    }
                }
            }
            return reviews;
        }


        public async Task<bool> CreateAsync(Guid roomId, Review review, Guid userId)
        {
            int rowsChanged = 0;

            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO \"Review\" (\"Id\", \"Rating\", \"Comment\", \"UserId\", \"RoomId\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
               "VALUES (@Id, @Rating, @Comment, @UserId, @RoomId, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    Guid id = Guid.NewGuid();
                    DateTime currentTime = DateTime.UtcNow;

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Rating", review.Rating);
                    command.Parameters.AddWithValue("@Comment", review.Comment);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@CreatedBy", userId);
                    command.Parameters.AddWithValue("@UpdatedBy", userId);
                    command.Parameters.AddWithValue("@DateCreated", currentTime);
                    command.Parameters.AddWithValue("@DateUpdated", currentTime);
                    command.Parameters.AddWithValue("@IsActive", review.IsActive);

                    rowsChanged = await command.ExecuteNonQueryAsync();
                }
            }

            return rowsChanged > 0;
        }

    }
}

