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
    public class BooksController : Controller
    {
        private BookContext db = new BookContext();
        public static List<Book> shoppingCart = new List<Book>();

        private void AddBooksToBag(ShoppingCart cart)
        {
            List<int> bookIDs = new List<int>();
            foreach (ShoppingCartItem item in cart.ShoppingCartItems)
            {
                bookIDs.Add(item.Book.ID);
            }
            ViewBag.BookIDs = bookIDs;
        }

        private void updateCartIcon()
        {
            var total = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList().First();

            if (total != null)
            {
                ViewBag.TotalBooksInCart = total.ShoppingCartItems.Count;
                AddBooksToBag(total);
            }
            else
            {
                ViewBag.TotalBooksInCart = 0;
            }
        }

        private void RemoveBookFromCart(int id)
        {
            //shoppingCart.Remove(db.Books.Find(id));

            for (int i = 0; i < shoppingCart.Count; ++i)
            {
                if (shoppingCart[i].ID == id)
                {
                    shoppingCart.RemoveAt(i);
                    break;
                }
            }


            //------> Remove from cart function <-----
            var cart = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList().First();

            if(cart != null)
            {
                var item = cart.ShoppingCartItems.Where(x => x.Book.ID == id).First();

                if(item != null)
                {
                    cart.TotalPrice -= item.Book.Price;
                    cart.ShoppingCartItems.Remove(item);
                    db.ShoppingCartItems.Remove(item);

                    db.SaveChanges();
                }
            }

            //------> Remove from cart function <-----
            updateCartIcon();
        }

        private double TotalCostOfCart()
        {
            double totalCost = 0;
            foreach (Book book in shoppingCart)
            {
                totalCost += book.Price;
            }
            return totalCost;
        }

        //GET: Shopping Cart
        [Authorize]
        public ActionResult ShoppingCart()
        {
            // ViewBag.TotalBooksInCart = shoppingCart.Count;
            // ViewBag.TotalCostOfCart = TotalCostOfCart();

            

            var cartItems = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList().First();
            ViewBag.TotalPrice = cartItems.TotalPrice;
            updateCartIcon();
            return View(cartItems.ShoppingCartItems);
        }

        //Add to cart (If on index page)
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

            AddToCart(book);

            //------> Add to cart function <-----
            return RedirectToAction("Index");
        }

        //Add to cart (If on details page)
        public ActionResult AddToCartDetails(int? id)
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

            AddToCart(book);


            return RedirectToAction("Details", new { id });
        }

        private void AddToCart(Book book)
        {
            //------> Add to cart function <-----
            var cart = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList().First();

            if (cart == null)
            {

                cart = new ShoppingCart()
                {
                    UserEmail = User.Identity.Name,
                    TotalPrice = 0,
                    DiscountCode = "",
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
                db.ShoppingCart.Add(cart);
            }

            if (!cart.ShoppingCartItems.Exists(x => x.Book.ID == book.ID))
            {
                cart.ShoppingCartItems.Add(new ShoppingCartItem()
                {
                    Book = book,
                    ShoppingCartId = cart.ShoppingCartId
                });

                cart.TotalPrice += book.Price;
            }
            

            db.SaveChanges();
            updateCartIcon();
            //------> Add to cart function <-----
        }

        //Add to wishlist (If on details page)
        [Authorize]
        public ActionResult AddToWishlistDetails(int? id)
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

            var wishlistItem = db.Wishlist.Where(x => x.UserEmail.Equals(User.Identity.Name) && x.Book.ID == id).FirstOrDefault();

            if (wishlistItem == null)
            {
                db.Wishlist.Add(new WishlistItem() {
                    DateAdded = DateTime.Now,
                    Book = book,
                    UserEmail = User.Identity.Name
                });
                db.SaveChanges();
            }
    
            return RedirectToAction("Details", new { id });
        }

        //Remove from cart (If on shopping cart page)
        public ActionResult RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemoveBookFromCart((int)id);

            return RedirectToAction("ShoppingCart");
        }

        //Remove from cart (If on index page)
        public ActionResult RemoveFromCartIndex(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemoveBookFromCart((int)id);

            return RedirectToAction("Index");
        }

        //Remove from cart (If on details page)
        public ActionResult RemoveFromCartDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemoveBookFromCart((int)id);

            return RedirectToAction("Details", new { id });
        }

        //Clear cart
        public ActionResult ClearShoppingCart()
        {
            //shoppingCart.Clear();

            var cartItems = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList().First();

            if(cartItems != null)
            {
                cartItems.TotalPrice = 0;
                cartItems.ShoppingCartItems.Clear();
                db.ShoppingCartItems.Where(x => x.ShoppingCartId == cartItems.ShoppingCartId).ForEachAsync(x => db.ShoppingCartItems.Remove(x));
                db.SaveChanges();
                updateCartIcon();
            }

            return RedirectToAction("ShoppingCart");
        }

        // GET: Books
        public ActionResult Index()
        {
            updateCartIcon();
            return View(db.Books.ToList());
        }

       

        // GET: Books/Search
        public ActionResult Search(string search)
        {
            updateCartIcon();

            var tmpBooks = db.Books.ToList();
            var books = new List<Book>();

            if (!String.IsNullOrEmpty(search))
            {
                books.AddRange(tmpBooks.Where(s => s.Title.ToLower().Contains(search.ToLower())).ToList());              
                books.AddRange(tmpBooks.Where(s => s.ISBN != null && s.ISBN.Equals(search)).ToList());
                books.AddRange(tmpBooks.Where(s => s.BookAuthor.ToLower().Contains(search.ToLower())).ToList());
                books.Distinct();
            }
            else
            {
                books = db.Books.ToList();
            }

            return View(books);
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

            updateCartIcon();
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
        public ActionResult Create([Bind(Include = "ID,Title,ImageURL,Rating,BookAuthor,BookDescription,Description,Price,Tradeable,ISBN")] Book book)
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
        public ActionResult Edit([Bind(Include = "ID,Title,ImageURL,Rating,BookAuthor,BookDescription,Description,Price,Tradeable,ISBN")] Book book)
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

        // GET: Checkout
        public ActionResult CheckOut()
        {
            ViewBag.TotalBooksInCart = shoppingCart.Count;
            ViewBag.TotalCostOfCart = TotalCostOfCart();
            return View();
        }

        // GET: OrderSummary
        public ActionResult OrderSummary()
        {
            List<Book> books = new List<Book>();
            foreach (Book book in shoppingCart)
            {
                books.Add(book);
            }
            ViewBag.TotalCostOfCart = TotalCostOfCart();
            ClearShoppingCart();
            ViewBag.TotalBooksInCart = shoppingCart.Count();
            return View(books);
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
