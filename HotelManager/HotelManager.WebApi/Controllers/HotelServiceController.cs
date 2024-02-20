using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using System;
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
        private readonly IHotelServiceService _hotelServiceService;

        public HotelServiceController(IHotelServiceService hotelServiceService)
        {
            _hotelServiceService = hotelServiceService;
        }

        // GET api/values
        [Authorize(Roles = "Admin")]
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
                var services = await _hotelServiceService.GetAllAsync(paging, sorting, hotelServiceFilter);
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> GetServiceByIdAsync([FromUri] Guid id)
        {
            try
            {
                if(id.Equals(Guid.Empty))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No hotel service with such ID!");
                }
                return Request.CreateResponse(HttpStatusCode.OK, await _hotelServiceService.GetByIdAsync(id));
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/values
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> CreateServiceAsync([FromBody] HotelService service)
        {
            if(service == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                bool created = await _hotelServiceService.CreateServiceAsync(service);
                if (created) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/values/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> UpdateServiceAsync(Guid id, [FromBody] HotelService updatedService)
        {
            if(updatedService == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Task<HotelService> serviceInBase = _hotelServiceService.GetByIdAsync(id);
            if(serviceInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            try
            {
                bool updated = await _hotelServiceService.UpdateServiceAsync(id, new HotelService()
                {
                    Name = updatedService.Name,
                    Description = updatedService.Description,
                    Price = updatedService.Price
                });
                if (updated) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // DELETE api/values/5
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> DeleteService(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Task<HotelService> service = _hotelServiceService.GetByIdAsync(id);
            if (service == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                bool deleted = await _hotelServiceService.DeleteServiceAsync(id);
                if (deleted)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(InternalServerError(ex));
            }
        }
    }
}
