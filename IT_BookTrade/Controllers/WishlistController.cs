using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IT_BookTrade.Models;

namespace IT_BookTrade.Controllers
{
    public class WishlistController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Wishlist
        [Authorize]
        public ActionResult Index()
        {
            var wishlist = db.Wishlist.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList();
            updateCartIcon();
            if (wishlist == null) System.Diagnostics.Debug.WriteLine("Wisghlist: null");
            /*
         //   System.Diagnostics.Debug.WriteLine("Count: " + wishlist.Count);
           // System.Diagnostics.Debug.WriteLine("Item1: " + wishlist.First().UserEmail);
           // System.Diagnostics.Debug.WriteLine("ccc: " + wishlist.Where(e=>e.Book != null).Count());

            foreach (var item in wishlist)
            {
                System.Diagnostics.Debug.WriteLine("BookITEM: " + item.Book.ID);
                
                System.Diagnostics.Debug.WriteLine("Book: " + item.Book.Title);
            }

    */
            return View(wishlist);
        }

       
        public ActionResult RemoveFromWishlist(int id)
        {
            WishlistItem wishlistItem = db.Wishlist.Find(id);

            if (wishlistItem != null && wishlistItem.UserEmail.Equals(User.Identity.Name))
            {
                db.Wishlist.Remove(wishlistItem);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void updateCartIcon()
        {
            var userShoppingCart = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();

            if (userShoppingCart != null && User.Identity.Name.Trim().Length > 0)
            {
                ViewBag.TotalBooksInCart = userShoppingCart.ShoppingCartItems.Count;
                AddBooksToBag(userShoppingCart);
            }
            else
            {
                ViewBag.TotalBooksInCart = 0;
                List<int> bookIDs = new List<int>();
                ViewBag.BookIDs = bookIDs;
            }
        }

        private void AddBooksToBag(ShoppingCart cart)
        {
            List<int> bookIDs = new List<int>();
            foreach (ShoppingCartItem item in cart.ShoppingCartItems)
            {
                bookIDs.Add(item.Book.ID);
            }
            ViewBag.BookIDs = bookIDs;
        }
    }
}
