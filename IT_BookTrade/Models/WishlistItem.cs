using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class WishlistItem
    {
        public int WishlistItemId { get; set; }
        public virtual Book Book { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserEmail { get; set; }

    }
}