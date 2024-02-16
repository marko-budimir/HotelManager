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
        public async Task<HttpResponseMessage> GetReceipts([FromUri] int minPrice = 0, int maxPrice = 100, string userEmail="", int pageNum = 1, int pageSize=10, string sortOrder = "ASC", string sortBy="TotalPrice", DateTime? dateCreated = null,  DateTime? dateUpdated = null)
        {
                ReceiptFilter filter = new ReceiptFilter
                {
                    minPrice = minPrice,
                    maxPrice = maxPrice,
                    userEmailQuery = userEmail,
                    dateCreated = dateCreated,
                    dateUpdated = dateUpdated

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
            return Request.CreateResponse(HttpStatusCode.OK); 
        }

        [HttpDelete]
        // DELETE: api/DashboardReceipt/5
        public async Task<HttpResponseMessage> Delete(Guid id, Receipt receipt)
        {

            if (receipt == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            int rowsAffected = await _receiptService.DeleteAsync(id, new Receipt()
            {
                Id = receipt.Id,
                TotalPrice = receipt.TotalPrice,
                IsPaid = receipt.IsPaid,
                ReservationId = receipt.ReservationId,
                DiscountId = receipt.DiscountId,
                CreatedBy = receipt.CreatedBy,
                UpdatedBy = receipt.UpdatedBy,
                DateUpdated = receipt.DateUpdated,
                DateCreated = receipt.DateCreated,
                IsActive = receipt.IsActive,
                InvoiceNumber = receipt.InvoiceNumber
            });
            if (rowsAffected == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Receipt phone with this ID doesn't exists");
            }
            else if (rowsAffected < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, "Error while updating receipt");
            }
            IReceipt resultReceipt = await _receiptService.GetByIdAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK, receipt);
        }
    }
}