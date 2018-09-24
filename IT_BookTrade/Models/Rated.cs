using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class Rated
    {
        public int Id { get; set; }
        public string User { get; set; }
        public int BookId { get; set; }
    }
}