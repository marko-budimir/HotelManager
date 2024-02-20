using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
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
        Task<IReceipt> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<List<IServiceInvoice>> GetAllInvoiceServiceAsync(Sorting sorting, Paging paging);
        Task<string>CreateInvoiceServiceAsync(IServiceInvoice invoice);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);

    }
}