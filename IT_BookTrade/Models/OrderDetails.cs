using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public string InvoiceID { get; set; }
        public int DiscountPrecentage { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpDate { get; set; }
        public bool Gift { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public int OrderId { get; set; }
        
    }
}