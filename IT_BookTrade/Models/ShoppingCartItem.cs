using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public virtual Book Book { get; set; }
        public int ShoppingCartId { get; set; }
    }
}