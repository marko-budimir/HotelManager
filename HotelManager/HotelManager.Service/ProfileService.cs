using HotelManager.Model.Common;
using HotelManager.Repository;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IRoleTypeRepository _roleTypeRepository;

        public ProfileService(IProfileRepository profileRepository, IRoleTypeRepository roleTypeRepository)
        {
            _profileRepository = profileRepository;
            _roleTypeRepository = roleTypeRepository;
        }

        public Task<IProfile> GetProfileById(Guid id)
        {
            try
            {
                return _profileRepository.GetProfileById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> CreateProfile(IProfile profile)
        {
            try
            {
                return _profileRepository.CreateProfile(profile);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UpdateProfile (Guid id, IProfile profile)
        {
            try
            {
                return _profileRepository.UpdateProfile(id, profile);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> DeleteProfile(Guid id)
        {
            try
            {
                return _profileRepository.DeleteProfile(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IProfile> ValidateUserAsync(string email, string password)
        {
            try
            {
                return await _profileRepository.ValidateUserAsync(email, password);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetRoleTypeByIdAsync(Guid id)
        {
            try
            {
                return await _roleTypeRepository.GetByIdAsync(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
