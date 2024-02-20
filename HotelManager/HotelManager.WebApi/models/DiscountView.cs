using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class DiscountView
    {
        public string Code { get; set; }
        public int Percent { get; set; }
    }
}