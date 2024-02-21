using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class ServiceHistoryView
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
    }
}