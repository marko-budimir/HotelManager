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
        Task<IProfile> GetProfileById(Guid Id);
        Task<bool> CreateProfile(IProfile Profile);
        Task<bool> UpdateProfile(Guid Id, IProfile Profile);
        Task<bool> DeleteProfile(Guid Id);
        Task<IProfile> ValidateUserAsync(string username, string password);
    }
}
