using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DisplayName("Author")]
        public string BookAuthor { get; set; }

        [Required]
        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Description")]
        public string BookDescription { get; set; }

        [DisplayName("Condition")]
        [DefaultValue("Brand New")]
        public string Description { get; set; }

        [DisplayName("Cover")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater or equal to 0.")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "The value can't have more than 2 decimal places.")]
        public double Price { get; set; }

        [Required]
        public int Amount { get; set; }

        [DisplayName("Books Sold")]
        public int BooksSold { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DisplayName("Available for trade")]
        public bool Tradeable { get; set; }

        public string ISBN { get; set; }

        [DisplayName("Seller")]
        public string SellerEmail { get; set; }
    }
}