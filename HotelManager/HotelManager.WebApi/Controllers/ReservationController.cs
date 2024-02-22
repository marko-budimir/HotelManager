﻿using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
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
    public class ReservationController : ApiController
    {
        private readonly IReservationService _reservationService;


        private readonly IMapper _mapper;
        public ReservationController(IReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAllAsync
            (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            string isAsc = "ASC",
            string searchQuery = null,
            DateTime? checkInDate = null,
            DateTime? checkOutDate = null,
            decimal? maxPrice= null,
            decimal? minPrice = null

            )
        {
            try
            {
                Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                ReservationFilter reservationFilter = new ReservationFilter() { SearchQuery = searchQuery,CheckInDate = checkInDate,CheckOutDate = checkOutDate, MaxPricePerNight=maxPrice, MinPricePerNight=minPrice};
                var reservations = await _reservationService.GetAllAsync(paging, sorting, reservationFilter);
                var reservationsView = _mapper.Map<IEnumerable<ReservationView>>(reservations);
                return Request.CreateResponse(HttpStatusCode.OK, reservationsView);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetByIdAsync(Guid id)
        {

            try {
                return Request.CreateResponse(HttpStatusCode.OK,await _reservationService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        public async Task<HttpResponseMessage> PostAsync(ReservationCreate reservation)
        {
            try
            {
                Reservation reservationInvoiceCreated = await _reservationService.PostAsync(reservation);
                if(reservationInvoiceCreated != null)
                    return Request.CreateResponse(HttpStatusCode.OK,reservationInvoiceCreated);

                return Request.CreateResponse(HttpStatusCode.BadRequest,"Bad request!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> UpdateAsync(
            Guid id,
            Guid invoiceId, 
            ReservationUpdate reservationUpdate)
        {
            try
            {
                if (reservationUpdate.IsActive == null)
                    reservationUpdate.IsActive = true;
                ReservationUpdate reservationUpdated = await _reservationService.UpdateAsync(id,invoiceId, reservationUpdate);
                if (reservationUpdated != null)
                {
                    Reservation reservation = await _reservationService.GetByIdAsync(id);
                    return Request.CreateResponse(HttpStatusCode.OK, reservation);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> DeleteAsync(Guid id,Guid invoiceId)
        {
            var reservationUpdate = new ReservationUpdate(){
                IsActive=false
            };

            try
            {
                ReservationUpdate reservationUpdated = await _reservationService.UpdateAsync(id,invoiceId, reservationUpdate);
                Reservation reservation = await _reservationService.GetByIdAsync(id);
                return Request.CreateResponse(HttpStatusCode.OK, reservation);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }        
        }
    }
}