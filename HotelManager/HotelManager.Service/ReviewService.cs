using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IRoomRepository _roomRepository;

        public ReviewService (IReviewRepository reviewRepository, IRoomRepository roomRepository)
        {
            _reviewRepository = reviewRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Review>> GetAllAsync(Guid roomId, Paging paging)
        {
            try
            {

                return await _reviewRepository.GetAllAsync(roomId, paging);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateAsync(Guid roomId, Review review)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            var roomExists = await _roomRepository.GetByIdAsync(roomId);
            if (roomExists == null)
            {
                throw new Exception("Room with the specified Id does not exist.");
            }

            try
            {
                return await _reviewRepository.CreateAsync(roomId, review, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
