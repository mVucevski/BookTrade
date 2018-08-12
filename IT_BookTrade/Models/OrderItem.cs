using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public string BookTitle { get; set; }
        public double BookPrice { get; set; }
        public int OrderDetailsId { get; set; }
    }
}