using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    public class RoomTypeController : ApiController
    {


        private readonly IRoomTypeService RoomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            RoomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            string isAsc = "ASC"
            )
        {
            try
            {
                Paging paging = new Paging() { PageNum = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                var roomTypes = await RoomTypeService.GetAllAsync(paging, sorting);
                if (roomTypes.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, roomTypes);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Room type was not found!");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetById(
                [FromUri] Guid id)
        {
            try
            {

                var roomTypes = await RoomTypeService.GetByIdAsync(id);
                if (roomTypes != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, roomTypes);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Room type was not found!");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateAsync(
            [FromUri] Guid id
           , [FromBody] RoomTypeUpdate roomTypeUpdate
           )
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await RoomTypeService.UpdateAsync(id, roomTypeUpdate));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(
            [FromBody] RoomTypePost roomTypePost
            )
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await RoomTypeService.PostAsync(roomTypePost));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}