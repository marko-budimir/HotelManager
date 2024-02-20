using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/Review")]
    public class ReviewController : ApiController
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [Route("{roomId:guid}")]
        public async Task<HttpResponseMessage> GetReviewsForRoomAsync(Guid roomId, [FromUri] int pageNumber = 1, [FromUri] int pageSize = 3, [FromUri] string sortBy = "", [FromUri] string isAsc = "ASC")
        {
            try
            {
                Paging paging = new Paging() { PageNum = pageNumber, PageSize = pageSize };
                var reviews = await _reviewService.GetAllAsync(roomId, paging);
                if (reviews.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, reviews);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "No reviews for the specified room!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("{roomId:guid}")]
        public async Task<HttpResponseMessage> CreateReviewForRoomAsync(Guid roomId, [FromBody] Review review)
        {
            try
            {
                review.RoomId = roomId;
                review.DateCreated = DateTime.Now;
                review.DateUpdated = DateTime.Now;
                review.IsActive = true;

                await _reviewService.CreateAsync(roomId, review);

                return Request.CreateResponse(HttpStatusCode.Created, "Review successfully created.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //// PUT api/values/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
