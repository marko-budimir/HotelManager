using HotelManager.Common;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IReceiptRepository
    {

        Task<List<IReceipt>> GetAllAsync(ReceiptFilter filter, Sorting sorting, Paging paging);
        Task<IReceipt> GetByIdAsync(Guid id);
    }
}
