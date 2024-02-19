using HotelManager.Common;
using HotelManager.Model;
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
    public class RoomController : ApiController
    {
        private readonly IRoomService RoomService;

        public RoomController(IRoomService roomService)
        {
            RoomService = roomService;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            string isAsc = "ASC",
            string SearchQuery = null,
            DateTime? StartDate = null,
            DateTime? EndDate = null,
            decimal? MinPrice = null,
            decimal? MaxPrice = null,
            int? MinBeds = null,
            Guid? RoomTypeId = null)
        {
            try { 
                Paging paging = new Paging() { PageNum=pageNumber,PageSize=pageSize};
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                RoomFilter roomFilter = new RoomFilter() { SearchQuery = SearchQuery,StartDate=StartDate,EndDate=EndDate,MinBeds=MinBeds,MaxPrice=MaxPrice,MinPrice=MinPrice,RoomTypeId=RoomTypeId };
                var rooms = await RoomService.GetAllAsync(paging, sorting,roomFilter);
                if(rooms.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, rooms);
                }
                return  Request.CreateResponse(HttpStatusCode.NotFound, "Room was not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetByIdAsync(
            [FromUri] Guid id
            )
        {
            try
            { 
                return Request.CreateResponse(HttpStatusCode.OK, await RoomService.GetByIdAsync(id));
            }

            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
      


    }
}
