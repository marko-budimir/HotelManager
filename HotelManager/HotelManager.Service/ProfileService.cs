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
        private readonly IProfileRepository profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        public ProfileService()
        {
            profileRepository = new ProfileRepository(); 
        }

        public Task<IProfile> GetProfileById(Guid id)
        {
            try
            {
                return profileRepository.GetProfileById(id);
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
                return profileRepository.CreateProfile(profile);
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
                return profileRepository.UpdateProfile(id, profile);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> DeleteProfile(Guid id)
        {
            try
            {
                return profileRepository.DeleteProfile(id);
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
                return await profileRepository.ValidateUserAsync(email, password);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }   
    }
}
