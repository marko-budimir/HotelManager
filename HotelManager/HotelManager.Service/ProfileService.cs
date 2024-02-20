using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManager.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IRoleTypeRepository _roleTypeRepository;
        public ClaimsIdentity CurrentUser { get; set; }
        

        public ProfileService(IProfileRepository profileRepository, IRoleTypeRepository roleTypeRepository)
        {
            _profileRepository = profileRepository;
            _roleTypeRepository = roleTypeRepository;
        }

        public Task<IUser> GetProfileByIdAsync()
        {
            var id = Guid.Parse(CurrentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            try
            {
                return _profileRepository.GetProfileByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> CreateProfileAsync(IUser profile)
        {
            Guid id = Guid.NewGuid();
            profile.CreatedBy = id;
            profile.UpdatedBy = id;
            profile.Id = id;
            profile.IsActive = true;
            try
            {
                return _profileRepository.CreateProfileAsync(profile);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UpdateProfileAsync (IUser profile)
        {
            var id = Guid.Parse(CurrentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            try
            {
                return _profileRepository.UpdateProfileAsync(id, profile);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> DeleteProfileAsync()
        {
            var id = Guid.Parse(CurrentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            try
            {
                return _profileRepository.DeleteProfileAsync(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IUser> ValidateUserAsync(string email, string password)
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

        public async Task<string> GetRoleTypeByRoleIdAsync(Guid id)
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
