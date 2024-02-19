using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IProfileRepository
    {
        Task<IUser> GetProfileByIdAsync(Guid Id);
        Task<bool> CreateProfileAsync(IUser Profile);
        Task<bool> UpdateProfileAsync(Guid Id, IUser Profile);
        Task<bool> DeleteProfileAsync(Guid Id);
        Task<IUser> ValidateUserAsync(string username, string password);
    }
}
