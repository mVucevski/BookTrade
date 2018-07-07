using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IT_BookTrade_1.Models;

namespace IT_BookTrade_1.Controllers
{
    public class BooksController : Controller
    {
        private BookContext db = new BookContext();
        public static List<Book> shoppingCart = new List<Book>();

        private void AddBooksToBag()
        {
            List<int> bookIDs = new List<int>();
            foreach (Book book in shoppingCart)
            {
                bookIDs.Add(book.ID);
            }
            ViewBag.BookIDs = bookIDs;          
        }

        public ActionResult ShoppingCart()
        {
            ViewBag.TotalBooksInCart = shoppingCart.Count;
            int totalCost = 0;
            foreach(Book book in shoppingCart)
            {
                totalCost += book.Price;
            }
            ViewBag.TotalCostOfCart = totalCost;
            return View(shoppingCart);
        }
        
        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            shoppingCart.Add(book);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //shoppingCart.Remove(db.Books.Find(id));

            for (int i=0; i<shoppingCart.Count; ++i)
            {
                if (shoppingCart[i].ID == id)
                {
                    shoppingCart.RemoveAt(i);
                    break;
                }
            }

            return RedirectToAction("ShoppingCart");
        }

        public ActionResult ClearShoppingCart()
        {
            shoppingCart.Clear();
            return RedirectToAction("ShoppingCart");
        }

        // GET: Books
        public ActionResult Index()
        {
            AddBooksToBag();
            ViewBag.TotalBooksInCart = shoppingCart.Count;
            return View(db.Books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            AddBooksToBag();
            ViewBag.TotalBooksInCart = shoppingCart.Count;
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.TotalBooksInCart = shoppingCart.Count;
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ImageURL,Rating,BookAuthor,BookDescription,Description,Price,Tradeable")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.TotalBooksInCart = shoppingCart.Count;
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ImageURL,Rating,BookAuthor,BookDescription,Description,Price,Tradeable")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
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
