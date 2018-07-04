using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT_BookTrade_1.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageURL { get; set; }

        [Range(1,10)]
        public int Rating { get; set; }
    }
}