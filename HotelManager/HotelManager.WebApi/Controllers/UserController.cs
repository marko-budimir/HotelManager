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
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
        }

        // GET api/Prfile/5
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> GetUserAsync()
        {
            try
            {
                IUser profile = await _userService.GetUserAsync();

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
        public async Task<HttpResponseMessage> CreateUserAsync(ProfileRegistered newProfile)
        {
            if(newProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            IUser profile = _mapper.Map<User>(newProfile);
            try
            {
                bool created = await _userService.CreateUserAsync(profile);
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
        public async Task<HttpResponseMessage> UpdateUserAsync(ProfileUpdated updatedProfile)
        {
            if(updatedProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Task<IUser> profileInBase = _userService.GetUserAsync();
            if(profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            IUser user = _mapper.Map<User>(updatedProfile);
            try
            {
                bool updated = await _userService.UpdateUserAsync(user);
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
