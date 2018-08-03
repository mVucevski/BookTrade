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

            if(wishlist == null) System.Diagnostics.Debug.WriteLine("Wisghlist: null");
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


        // GET: Wishlist/Create
        public ActionResult Create()
        {
            return View();
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
    }
}
