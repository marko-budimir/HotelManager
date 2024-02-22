﻿using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class InvoiceReceipt
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public Guid ReservationId { get; set; }
        public Guid DiscountId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal PricePerNight { get; set; }
        public int RoomNumber { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountPercent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
