using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public virtual List<OrderDetails> OrdersDetails { get; set; }
    }
}