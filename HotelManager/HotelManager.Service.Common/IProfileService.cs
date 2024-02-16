using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IProfileService
    {
        Task<IProfile> GetProfileById(Guid id);
        Task<bool> CreateProfile(IProfile profile);
        Task<bool> UpdateProfile(Guid Id, IProfile profile);
        Task<bool> DeleteProfile(Guid Id);
    }
}
