using HotelManager.Model.Common;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IProfileService
    {
        Task<IUser> GetProfileAsync();
        Task<bool> CreateProfileAsync(IUser profile);
        Task<bool> UpdateProfileAsync(IUser profile);
        Task<bool> DeleteProfileAsync();
        Task<IUser> ValidateUserAsync(string username, string password);
        Task<string> GetRoleTypeByRoleIdAsync(Guid id);
        Task<string> GetUserEmailByIdAsync(Guid id);
    }
}
