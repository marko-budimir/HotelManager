using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
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
    public class ServiceInvoiceController : ApiController
    {
        private readonly IReceiptService _receiptService;

        public ServiceInvoiceController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetServiceInvoiceByInvoiceIdAsync (Guid id)
        {
            try
            {
                ServiceHistoryView historyView = await _receiptService.GetServiceInvoiceByInvoiceIdAsync(id);
                return Request.CreateResponse(HttpStatusCode.OK, historyView);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllInvoiceServiceAsync(int pageNum = 1, int pageSize = 10, string sortOrder = "ASC", string sortBy = "DateCreated", DateTime? dateCreated = null, DateTime? dateUpdated = null)
        {
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

            List<IServiceInvoice> invoices;

            try
            {
                invoices = await _receiptService.GetAllInvoiceServiceAsync(sorting, paging);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, invoices);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            int result = await _receiptService.DeleteAsync(id);
            if (result == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invoice deleted");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateInvoiceAsync([FromBody]ServiceInvoice invoiceCreate)
        {
            IServiceInvoice invoice = new ServiceInvoice
            {
                Id = Guid.NewGuid(),
                NumberOfService = invoiceCreate.NumberOfService,
                InvoiceId = invoiceCreate.InvoiceId,
                ServiceId = invoiceCreate.ServiceId,
                CreatedBy = invoiceCreate.CreatedBy,
                UpdatedBy = invoiceCreate.UpdatedBy,
                DateCreated = invoiceCreate.DateCreated,
                DateUpdated = invoiceCreate.DateUpdated,
                IsActive = invoiceCreate.IsActive
            };

            string result = await _receiptService.CreateInvoiceAsync(invoice);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
