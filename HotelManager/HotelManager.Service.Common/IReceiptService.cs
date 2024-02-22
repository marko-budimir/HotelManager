using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IReceiptService
    {

        Task<List<IReceipt>> GetAllAsync(ReceiptFilter filter, Sorting sorting, Paging paging);
        Task<InvoiceReceipt> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<PagedList<IServiceInvoice>> GetAllInvoiceServiceAsync(Sorting sorting, Paging paging);
        Task<string> CreateInvoiceServiceAsync(IServiceInvoice invoice);
        Task<PagedList<IServiceInvoiceHistory>> GetServiceInvoiceByInvoiceIdAsync(Guid id, Sorting sorting, Paging paging);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task<Invoice> PutTotalPriceAsync(Guid invoiceId, InvoiceUpdate invoiceUpdate);
        Task<bool> SendReceiptAsync(Guid id);
    }
}