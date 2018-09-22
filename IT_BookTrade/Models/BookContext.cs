using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class BookContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BookContext() : base("name=BookContext")
        {
        }

        public DbSet<WishlistItem> Wishlist { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<OrderDetails> OrderDetail { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Chat> Chat { get; set; }

        public System.Data.Entity.DbSet<IT_BookTrade.Models.Book> Books { get; set; }

        public System.Data.Entity.DbSet<IT_BookTrade.Models.ChatMessages> ChatMessages { get; set; }
    }
}
