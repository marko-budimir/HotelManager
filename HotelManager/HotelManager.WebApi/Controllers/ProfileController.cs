using AutoMapper;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/Profile")]
    public class ProfileController : ApiController
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        public ProfileController(IProfileService profileService, IMapper mapper) {
            _profileService = profileService;
            _mapper = mapper;
        }

        // GET api/Prfile/5
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> GetProfileByIdAsync()
        {
            try
            {
                IUser profile = await _profileService.GetProfileAsync();

                if (profile == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                var profileView = _mapper.Map<ProfileView>(profile);
                return Request.CreateResponse(HttpStatusCode.OK, profileView);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // POST api/Profile
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> CreateProfileAsync(ProfileRegistered newProfile)
        {
            if(newProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            IUser profile = _mapper.Map<User>(newProfile);
            try
            {
                bool created = await _profileService.CreateProfileAsync(profile);
                if (created) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // PUT api/Profile/5
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        [Route("")]
        public async Task<HttpResponseMessage> UpdateProfileAsync(ProfileUpdated updatedProfile)
        {
            if(updatedProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Task<IUser> profileInBase = _profileService.GetProfileAsync();
            if(profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            IUser user = _mapper.Map<User>(updatedProfile);
            try
            {
                bool updated = await _profileService.UpdateProfileAsync(user);
                if (updated) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //// DELETE api/Profile/5
        //public Task<HttpResponseMessage> DeleteProfileAsync(int id)
        //{
        //}
    }
}
