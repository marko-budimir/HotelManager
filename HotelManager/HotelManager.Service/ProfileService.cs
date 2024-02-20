using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _profileRepository;
        private readonly IRoleTypeRepository _roleTypeRepository;
        public ClaimsIdentity CurrentUser { get; set; }
        

        public ProfileService(IUserRepository profileRepository, IRoleTypeRepository roleTypeRepository)
        {
            _profileRepository = profileRepository;
            _roleTypeRepository = roleTypeRepository;
        }

        public async Task<Model.Common.IUser> GetProfileAsync()
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _profileRepository.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateProfileAsync(Model.Common.IUser profile)
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            profile.CreatedBy = id;
            profile.UpdatedBy = id;
            profile.Id = id;
            profile.IsActive = true;
            try
            {
                return await _profileRepository.CreateAsync(profile);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateProfileAsync (Model.Common.IUser profile)
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _profileRepository.UpdateAsync(id, profile);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteProfileAsync()
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _profileRepository.DeleteAsync(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Model.Common.IUser> ValidateUserAsync(string email, string password)
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

        public async Task<string> GetUserEmailByIdAsync(Guid id)
        {
            try
            {
                var user = await _profileRepository.GetByIdAsync(id);
                return user.Email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
