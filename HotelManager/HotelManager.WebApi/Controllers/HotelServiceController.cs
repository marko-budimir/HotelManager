using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/HotelService")]
    public class HotelServiceController : ApiController
    {
        private readonly IHotelServiceService DashboardServiceService;

        public HotelServiceController(IHotelServiceService dashboardServiceService)
        {
            DashboardServiceService = dashboardServiceService;
        }

        // GET api/values
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> GetAllServicesAsync(
            [FromUri] int pageNumber = 1,
            [FromUri] int pageSize = 10,
            [FromUri] string sortBy = "",
            [FromUri] string isAsc = "ASC",
            [FromUri] string searchQuery = null,
            [FromUri] decimal? minPrice = null,
            [FromUri] decimal? maxPrice = null
            )
        {
            try
            {
                Paging paging = new Paging() { PageNum = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                HotelServiceFilter hotelServiceFilter = new HotelServiceFilter() { SearchQuery = searchQuery, MinPrice = minPrice, MaxPrice = maxPrice };
                var services = await DashboardServiceService.GetAllAsync(paging, sorting, hotelServiceFilter);
                if (services.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, services);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "No services!");
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        // GET api/values/5
        public string Get(Guid id)
        {
            return "value";
        }

        //// POST api/values
        //public void Post([FromBody] string value)
        //{
        //}

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
