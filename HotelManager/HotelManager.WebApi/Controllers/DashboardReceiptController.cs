using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Web.Http;


namespace HotelManager.WebApi.Controllers
{
    public class DashboardReceiptController : ApiController
    {

        private readonly IReceiptService _receiptService;
        public DashboardReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        // GET: api/DashboardReceipt
        public async Task<HttpResponseMessage> GetReceipts([FromUri] int minPrice = 0, int maxPrice = 100, bool? isPaid = null, string userEmailQuery = null, int pageNum = 1, int pageSize = 10, string sortOrder = "ASC", string sortBy = "TotalPrice", DateTime? dateCreated = null, DateTime? dateUpdated = null)
        {
            ReceiptFilter filter = new ReceiptFilter
            {
                minPrice = minPrice,
                maxPrice = maxPrice,
                userEmailQuery = userEmailQuery,
                dateCreated = dateCreated,
                dateUpdated = dateUpdated,
                isPaid = isPaid
            };

            Sorting sorting = new Sorting
            {
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            Paging paging = new Paging
            {
                PageNum = pageNum,
                PageSize = pageSize
            };

            List<IReceipt> receipts;

            try
            {
                receipts = await _receiptService.GetAllAsync(filter, sorting, paging);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, receipts);
        }

        [HttpGet]
        // GET: api/DashboardReceipt/5
        public async Task<HttpResponseMessage> GetReceipt(Guid id)
        {
            try
            {
                IReceipt receipt = await _receiptService.GetByIdAsync(id);
                return Request.CreateResponse(HttpStatusCode.OK, receipt);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
        }
        
    }
}