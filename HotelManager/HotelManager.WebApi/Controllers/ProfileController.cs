using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/Profile")]
    public class ProfileController : ApiController
    {
        private readonly IProfileService profileService;
        public ProfileController(IProfileService profileService) {
            this.profileService = profileService;
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> GetProfileByIdAsync(Guid id)
        {
            try
            {
                IProfile profile = await profileService.GetProfileById(id);

                if (profile == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ProfileView(profile));
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> CreateProfileAsync([FromBody] ProfileRegistered newProfile)
        {
            if(newProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Guid id = Guid.NewGuid();
            IProfile profile = new Profile
            {
                Id = id,
                FirstName = newProfile.FirstName,
                LastName = newProfile.LastName,
                Email = newProfile.Email,
                Password = newProfile.Password,
                Phone = newProfile.Phone,
                CreatedBy = id,
                UpdatedBy = id,
                IsActive = true
            };
            try
            {
                bool created = await profileService.CreateProfile(profile);
                if (created) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> UpdateProfileAsync(Guid id, [FromBody] Profile updatedProfile)
        {
            if(updatedProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Task<IProfile> profileInBase = profileService.GetProfileById(id);
            if(profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            try
            {
                bool updated = await profileService.UpdateProfile(id, new Profile()
                {
                    FirstName = updatedProfile.FirstName,
                    LastName = updatedProfile.LastName,
                    Email = updatedProfile.Email,
                    Phone = updatedProfile.Phone
                });
                if (updated) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //// DELETE api/values/5
        //public Task<HttpResponseMessage> DeleteProfileAsync(int id)
        //{
        //}
    }
}
