using AutoMapper;
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
        private readonly IReceiptService _receiptService;


        private readonly IMapper _mapper;
        public ReservationController(IReceiptService receiptService, IReservationService reservationService, IMapper mapper)
        {
            _receiptService = receiptService;
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
            string SearchQuery = null,
            DateTime? checkInDate = null,
            DateTime? checkOutDate = null,
            decimal? maxPrice= null,
            decimal? minPrice = null

            )
        {
            try
            {
                Paging paging = new Paging() { PageNum = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                ReservationFilter reservationFilter = new ReservationFilter() { SearchQuery = SearchQuery,CheckInDate = checkInDate,CheckOutDate = checkOutDate, MaxPricePerNight=maxPrice, MinPricePerNight=minPrice};
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
        public async Task<HttpResponseMessage> PostAsync(Reservation reservation)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> UpdateAsync(Guid id, ReservationUpdate reservationUpdate)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        [HttpPost]
        [Route("api/Invoice/")]
        public async Task<HttpResponseMessage> CreateInvoiceAsync([FromBody] Invoice invoice)
        {

            return Request.CreateResponse(HttpStatusCode.OK, await _receiptService.CreateInvoiceAsync(invoice));
        }
    }
}
