using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private string _ConnectionString = "host=localhost ;port=5432 ;Database=HotelManager ;User ID=postgres ;Password=postgres";
        public async Task<IUser> GetProfileByIdAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                IUser profile = null;
                string commandText = "SELECT * FROM \"User\" WHERE \"Id\" = @Id";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        await connection.OpenAsync();

                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                profile = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    FirstName = (String)reader["FirstName"],
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    RoleId = reader.GetGuid(reader.GetOrdinal("RoleId")),
                                    CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                    UpdatedBy = reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };
                            }
                        }
                    }catch (Exception ex) 
                    {
                        throw ex;
                    }
                }
                return profile;
            }
        }

        public async Task<bool> CreateProfileAsync(IUser newProfile)
        {
            int rowChanged;
            NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString);
            using (connection)
            {
                string insertQuery = "INSERT INTO \"User\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"Password\", \"Phone\", \"RoleId\", \"CreatedBy\", \"UpdatedBy\", \"IsActive\") " +
                    "VALUES (@Id, @FirstName, @LastName, @Email, @Password, @Phone, (SELECT \"Id\" FROM \"Role\" WHERE \"Name\" = 'User'), @CreatedBy, @UpdatedBy, @IsActive)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);
                //required parameters
                insertCommand.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = newProfile.Id;
                insertCommand.Parameters.Add("@FirstName", NpgsqlDbType.Char).Value = newProfile.FirstName;
                insertCommand.Parameters.Add("@LastName", NpgsqlDbType.Char).Value = newProfile.LastName;
                insertCommand.Parameters.Add("@Email", NpgsqlDbType.Text).Value = newProfile.Email;
                insertCommand.Parameters.Add("@Password", NpgsqlDbType.Text).Value = newProfile.Password;
                insertCommand.Parameters.Add("@CreatedBy", NpgsqlDbType.Uuid).Value = newProfile.CreatedBy;
                insertCommand.Parameters.Add("@UpdatedBy", NpgsqlDbType.Uuid).Value = newProfile.UpdatedBy;
                insertCommand.Parameters.Add("@IsActive", NpgsqlDbType.Boolean).Value = newProfile.IsActive;

                //not required parameters
                NpgsqlParameter phoneParam = new NpgsqlParameter("@Phone", NpgsqlDbType.Text);
                phoneParam.Value = (object)newProfile.Phone ?? DBNull.Value;
                insertCommand.Parameters.Add(phoneParam);

                try
                {
                    connection.Open();
                    rowChanged = await insertCommand.ExecuteNonQueryAsync();
                    return rowChanged != 0;
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

        public async Task<bool> UpdateProfileAsync(Guid id, IUser updatedProfile)
        {
            int rowsChanged;

            IUser profile = await GetProfileByIdAsync(id);
            if (profile == null)
            {
                throw new Exception("No user with such ID in the database!");
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE \"User\" SET";
                List<string> updateFields = new List<string>();

                if (!string.IsNullOrEmpty(updatedProfile.FirstName))
                {
                    updateFields.Add("\"FirstName\" = @FirstName");
                }
                if (!string.IsNullOrEmpty(updatedProfile.LastName))
                {
                    updateFields.Add("\"LastName\" = @LastName");
                }
                if (!string.IsNullOrEmpty(updatedProfile.Email))
                {
                    updateFields.Add("\"Email\" = @Email");
                }
                if (!string.IsNullOrEmpty(updatedProfile.Phone))
                {
                    updateFields.Add("\"Phone\" = @Phone");
                }

                updateQuery += " " + string.Join(", ", updateFields);
                updateQuery += " WHERE \"Id\" = @Id";

                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);
                AddProfileParameters(updateCommand, id, updatedProfile);

                rowsChanged = await updateCommand.ExecuteNonQueryAsync();
            }
            return rowsChanged != 0;
        }

        public async Task<bool> DeleteProfileAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                string deleteQuery = "DELETE FROM \"User\" WHERE \"Id\" = @Id";
                NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    await connection.OpenAsync();

                    int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
                catch (NpgsqlException ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IUser> ValidateUserAsync(string email, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                IUser profile = null;
                string commandText = "SELECT \"User\".\"Id\", \"User\".\"Email\", \"User\".\"Password\", \"User\".\"RoleId\"  FROM \"User\" WHERE \"Email\" = @Email AND \"Password\" = @Password";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Email", email);
                    npgsqlCommand.Parameters.AddWithValue("@Password", password);
                    try
                    {
                        await connection.OpenAsync();
                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                profile = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    Email = (String)reader["Email"],
                                    Password = (String)reader["Password"],
                                    RoleId = (Guid)reader["RoleId"],
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return profile;
            }
        }

        private void AddProfileParameters(NpgsqlCommand command, Guid id, IUser updatedProfile)
        {
            command.Parameters.AddWithValue("@Id", id);
            if (!string.IsNullOrEmpty(updatedProfile.FirstName))
            {
                command.Parameters.AddWithValue("@FirstName", updatedProfile.FirstName);
            }
            if (!string.IsNullOrEmpty(updatedProfile.LastName))
            {
                command.Parameters.AddWithValue("@LastName", updatedProfile.LastName);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Email))
            {
                command.Parameters.AddWithValue("@Email", updatedProfile.Email);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Password))
            {
                command.Parameters.AddWithValue("@Password", updatedProfile.Password);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Phone))
            {
                command.Parameters.AddWithValue("@Phone", updatedProfile.Phone);
            }
            if (updatedProfile.RoleId != null)
            {
                command.Parameters.AddWithValue("@RoleId", updatedProfile.RoleId);
            }
            if (updatedProfile.CreatedBy != null)
            {
                command.Parameters.AddWithValue("@CreatedBy", updatedProfile.CreatedBy);
            }
            if (updatedProfile.UpdatedBy != null)
            {
                command.Parameters.AddWithValue("@UpdatedBy", updatedProfile.UpdatedBy);
            }
            if (updatedProfile.CreatedDate != null)
            {
                command.Parameters.AddWithValue("@DateCreated", updatedProfile.CreatedDate);
            }
            if (updatedProfile.UpdatedDate != null)
            {
                command.Parameters.AddWithValue("@DateUpdated", updatedProfile.UpdatedDate);
            }
            if (updatedProfile.IsActive != null)
            {
                command.Parameters.AddWithValue("@IsActive", updatedProfile.IsActive);
            }
        }
    }
}
