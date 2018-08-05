using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public string UserEmail { get; set; }
        public double TotalPrice { get; set; }
        public string DiscountCode { get; set; }
        public virtual List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}