﻿using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
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
    public class RoomController : ApiController
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
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
                Paging paging = new Paging() { PageNumber=pageNumber,PageSize=pageSize};
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                RoomFilter roomFilter = new RoomFilter() { SearchQuery = SearchQuery,StartDate=StartDate,EndDate=EndDate,MinBeds=MinBeds,MaxPrice=MaxPrice,MinPrice=MinPrice,RoomTypeId=RoomTypeId };
                var rooms = await _roomService.GetAllAsync(paging, sorting,roomFilter);
                if(rooms.Any())
                {
                    var roomViews = rooms.Select(room => _mapper.Map<RoomView>(room)).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, roomViews);
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
                var room = await _roomService.GetByIdAsync(id);

                if (room != null)
                {


                    var roomView = _mapper.Map<RoomView>(room);
                    return Request.CreateResponse(HttpStatusCode.OK, roomView);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Room not found.");
                }
            }

            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
