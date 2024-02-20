using HotelManager.Common;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IDiscountService
    {
        Task<List<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging);
        Task<IDiscount> GetDiscountByIdAsync(Guid id);
        Task<int> DeleteDiscountAsync(Guid id);
        Task<string> CreateDiscountAsync(IDiscount newDiscount);
        Task<string> UpdateDiscountAsync(Guid id, IDiscount discountUpdated);
        Task<IDiscount> GetDiscountByCodeAsync(string code);
    }
}
