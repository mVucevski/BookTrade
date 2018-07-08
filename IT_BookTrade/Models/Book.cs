using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string BookAuthor { get; set; }

        public string BookDescription { get; set; }

        public string Description { get; set; }

        [Required]
        public string ImageURL { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater or equal to 0.")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "The value can't have more than 2 decimal places.")]
        public double Price { get; set; }

        [Required]
        public bool Tradeable { get; set; }
    }
}