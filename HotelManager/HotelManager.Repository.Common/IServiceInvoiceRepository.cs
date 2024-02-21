using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IServiceInvoiceRepository
    {
        Task<List<IServiceInvoice>> GetAllInvoiceServiceAsync(Sorting sorting, Paging paging);
        Task<int> UpdateAsync(Guid id);
        Task<ServiceHistoryView> GetServiceInvoiceByInvoiceIdAsync(Guid id);
        Task<string> CreateInvoiceServiceAsync(IServiceInvoice serviceInvoice);
    }
}
