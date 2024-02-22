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
    public class DashBoardRoomController : ApiController
    {

        private readonly IRoomService _roomService;

        public DashBoardRoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateRoomAsync(
            [FromUri] Guid id
           , [FromBody] RoomUpdate roomUpdate
           )
        {
            try
            {
                if (id.Equals(Guid.Empty))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.OK, await _roomService.UpdateRoomAsync(id, roomUpdate));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            [FromUri] int pageNumber = 1,
            [FromUri] int pageSize = 10,
            [FromUri] string sortBy = "",
            [FromUri] string isAsc = "ASC",
            [FromUri] string SearchQuery = null,
            [FromUri] DateTime? StartDate = null,
            [FromUri] DateTime? EndDate = null,
            [FromUri] decimal? MinPrice = null,
            [FromUri] decimal? MaxPrice = null,
            [FromUri] int? MinBeds = null,
            [FromUri] Guid? RoomTypeId = null)
        {
            try
            {
                Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                RoomFilter roomFilter = new RoomFilter() { SearchQuery = SearchQuery, StartDate = StartDate, EndDate = EndDate, MinBeds = MinBeds, MaxPrice = MaxPrice, MinPrice = MinPrice, RoomTypeId = RoomTypeId };
                var roomsUpdated = await _roomService.GetUpdatedRoomsAsync(paging, sorting, roomFilter);
                if (roomsUpdated.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, roomsUpdated);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Room was not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetRoomsUpdateByIdAsync(
            [FromUri] Guid id
            )
        {
            try
            {
                if (id.Equals(Guid.Empty))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);


                return Request.CreateResponse(HttpStatusCode.OK, await _roomService.GetRoomUpdateByIdAsync(id));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        }
}
