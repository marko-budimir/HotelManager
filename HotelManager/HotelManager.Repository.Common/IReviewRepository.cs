using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync(Guid roomId, Paging paging);

        Task<bool> CreateAsync(Guid roomId, Review review, Guid userId);
    }
}
