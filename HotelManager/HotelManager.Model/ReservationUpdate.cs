using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class ReservationUpdate
    {
        public ReservationUpdate() {
            DateUpdated = DateTime.Now;
        }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string RoomNumber { get; set; }
        public decimal? PricePerNight { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool? IsActive { get; set; }
    }
}
