using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class Wishlist
    {
        public int WishlistId { get; set; }
        public List<WishlistItem> WishlistedItems { get; set; }
        public string WishlistUserEmail { get; set; }

    }
}