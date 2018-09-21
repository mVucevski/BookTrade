using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        private ApplicationDbContext usersDB = new ApplicationDbContext();

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
            var cart = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();

            if (cart != null)
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
            /* double totalCost = 0;
             foreach (Book book in shoppingCart)
             {
                 totalCost += book.Price;
             }*/
            var total = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();



            return total.TotalPrice;
        }

        //GET: Shopping Cart
        [Authorize]
        public ActionResult ShoppingCart()
        {
            var cartItems = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();

            var soldOutBooks = cartItems.ShoppingCartItems.Where(item => item.Book.Amount < 1);
            if (soldOutBooks != null)
            {
                var soldOutBooksList = soldOutBooks.ToList();
                db.ShoppingCartItems.RemoveRange(soldOutBooksList);
                cartItems.ShoppingCartItems.RemoveAll(item => item.Book.Amount < 1);
            }


            db.SaveChanges();
            ViewBag.TotalPrice = cartItems.TotalPrice;
            updateCartIcon();
            return View(cartItems.ShoppingCartItems);
        }

        //Add to cart (If on index page) //Ova preku API i ajax
        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book.Amount < 1)
            {
                return RedirectToAction("Index");
            }
            if (book == null)
            {
                return HttpNotFound();
            }

            if (!book.SellerEmail.Equals(User.Identity.Name))
            {
                AddToCart(book);
            }

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
            if (book.Amount < 1)
            {
                return RedirectToAction("Details", new { id });
            }
            if (book == null)
            {
                return HttpNotFound();
            }

            if (!book.SellerEmail.Equals(User.Identity.Name))
            {
                AddToCart(book);
                return RedirectToAction("Details", new { id });
            }




            return RedirectToAction("Index");
        }

        private void AddToCart(Book book)
        {
            //------> Add to cart function <-----
            var cart = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();

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

            if (book.SellerEmail.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index");
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

        private void ClearCart()
        {
            var cartItems = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();

            if (cartItems != null)
            {
                cartItems.TotalPrice = 0;
                var listShopingItems = db.ShoppingCartItems.Where(x => x.ShoppingCartId == cartItems.ShoppingCartId).ToList();

                foreach (var x in listShopingItems) {
                    db.ShoppingCartItems.Remove(x);
                }

                cartItems.ShoppingCartItems.Clear();
                db.SaveChanges();
                updateCartIcon();
            }
        }

        //Clear cart
        public ActionResult ClearShoppingCart()
        {
            ClearCart();

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

            ViewBag.PageName = "Search results for " + search;
            return View(books.OrderBy(x=>x.Amount==0));
        }

        // GET: Books/Bestsellers
        public ActionResult Bestsellers()
        {
            updateCartIcon();

            var books = db.Books.Where(x=>x.Amount>0).ToList().OrderByDescending(x=>x.BooksSold).OrderBy(x => x.Amount == 0);

            ViewBag.PageName = "Bestsellers";
            return View("Search", books);
        }

        // GET: Books/LatestDeals
        public ActionResult LatestDeals()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0).ToList().OrderByDescending(x => x.ID);

            ViewBag.PageName = "Latest Book Deals";
            return View("Search", books);
        }

        // GET: Books/TradeSection
        public ActionResult TradeSection()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && x.Tradeable).ToList().OrderByDescending(x => x.ID);

            ViewBag.PageName = "Trade Section";
            return View("Search", books);
        }

        // GET: Books/TradeSection
        public ActionResult SurpriseMe()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name)).ToList();

            var randomBook = books[new Random().Next(books.Count)];

            return RedirectToAction("Details", new { id = randomBook.ID });
        }

        // GET: Books/Fiction
        public ActionResult Fiction()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name) && x.Category.Contains("Fiction")).ToList();

            ViewBag.PageName = "Category Fiction";
            return View("Search", books);
        }

        // GET: Books/NonFiction
        public ActionResult NonFiction()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name) && x.Category.Contains("Non-Fiction")).ToList();

            ViewBag.PageName = "Category Non-Fiction";
            return View("Search", books);
        }

        // GET: Books/CrimeThriller
        public ActionResult CrimeThriller()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name) && (x.Category.Contains("Crime") || x.Category.Contains("Thriller"))).ToList();

            ViewBag.PageName = "Category Crime & Thriller";
            return View("Search", books);
        }

        // GET: Books/FoodDrink
        public ActionResult FoodDrink()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name) && (x.Category.Contains("Drink") || x.Category.Contains("Food") || x.Category.Contains("Cook"))).ToList();

            ViewBag.PageName = "Category Food & Drink";
            return View("Search", books);
        }

        // GET: Books/Fantasy
        public ActionResult Fantasy()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name) && x.Category.Contains("Fantasy")).ToList();

            ViewBag.PageName = "Category Fantasy";
            return View("Search", books);
        }

        // GET: Books/History
        public ActionResult History()
        {
            updateCartIcon();

            var books = db.Books.Where(x => x.Amount > 0 && !x.SellerEmail.Equals(User.Identity.Name) && x.Category.Contains("History")).ToList();

            ViewBag.PageName = "Category History";
            return View("Search", books);
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

            var seller = usersDB.Users.Where(x => x.Email.Equals(book.SellerEmail)).FirstOrDefault();
            ViewBag.SellerName = seller.FirstName + " " + seller.LastName;



            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            updateCartIcon();
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,ImagePath,ImageFile,Rating,BookAuthor,BookDescription,Description,Price,Tradeable,ISBN,Category,Language,Amount")] Book book)
        {
            updateCartIcon();
            if (ModelState.IsValid)
            {
                saveImage(book);

                book.SellerEmail = User.Identity.Name;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        private void saveImage(Book book) {
            if (book.ImageFile != null)
            {
                string filename = Path.GetFileNameWithoutExtension(book.ImageFile.FileName);
                string extension = Path.GetExtension(book.ImageFile.FileName);
                filename = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + extension;
                book.ImagePath = "~/Images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                book.ImageFile.SaveAs(filename);
            }
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            updateCartIcon();
            if (book == null)
            {
                return HttpNotFound();
            }

            if(book.SellerEmail.Equals(User.Identity.Name) || User.IsInRole("Admin"))
            {
                ViewBag.TotalBooksInCart = shoppingCart.Count;
                return View(book);
            }

            return RedirectToAction("Index");
        }


        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,ImagePath,ImageFile,Rating,BookAuthor,BookDescription,Description,Price,Tradeable,ISBN,SellerEmail,Category,Language,Amount")] Book book, string ImagePath)
        {
            System.Diagnostics.Debug.WriteLine("Slika " + ImagePath);
            book.ImagePath = ImagePath;
            updateCartIcon();

            if (ModelState.IsValid)
            {           
                saveImage(book);           
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        /*public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            updateCartIcon();
            if (book == null)
            {
                return HttpNotFound();
            }
            
            if (book.SellerEmail.Equals(User.Identity.Name) || User.IsInRole("Admin"))
            {
                return View(book);
            }

            return RedirectToAction("Index");
        }*/

        // POST: Books/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        // GET: Checkout

        public ActionResult CheckOut()
        {
            updateCartIcon();
            ViewBag.TotalCostOfCart = TotalCostOfCart();
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(OrderDetails checkoutDetails, string mm)
        {
            Order ORDER;
            if (db.Order.Count() == 0)
                ORDER = null;
            else
                ORDER = db.Order.FirstOrDefault(p => p.UserEmail.Equals(User.Identity.Name));

            if (ORDER == null) {

                ORDER = new Order()
                {
                    UserEmail = User.Identity.Name,
                    OrdersDetails = new List<OrderDetails>()
                };
                db.Order.Add(ORDER);
            }
            
            checkoutDetails.ExpDate = mm + "/" + checkoutDetails.ExpDate;

            var uniqueId = (ORDER.OrdersDetails.Count + 1).ToString();

            checkoutDetails.InvoiceID = uniqueId + "-" + randomInovice(5).ToUpper();
            /*
             * To-Do!
             * Implement discount system
             */
            checkoutDetails.DiscountPrecentage = 0;
            checkoutDetails.OrderDate = DateTime.Now;

            var shopingItems = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).ToList().First().ShoppingCartItems;
            var orderItems = new List<OrderItem>();

            foreach(var x in shopingItems)
            {
                x.Book.BooksSold += 1;
                x.Book.Amount -= 1;
                
                orderItems.Add(new OrderItem()
                {
                    BookTitle = x.Book.Title,
                    BookPrice = x.Book.Price
                });
            }

            checkoutDetails.OrderItems = orderItems;

            ORDER.OrdersDetails.Add(checkoutDetails);
            
            db.SaveChanges();
            
            ClearCart();
            updateCartIcon();
            return RedirectToAction("OrderSummary", "Books", new { id = checkoutDetails.InvoiceID });
            //System.Diagnostics.Debug.WriteLine("MMMM " + mm);
        }

        private string randomInovice(int len) {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: OrderSummary
        public ActionResult OrderSummary(string id)
        {
            
            var order = db.Order.Where(p => p.UserEmail.Equals(User.Identity.Name)).First()
                .OrdersDetails.Find(x=>x.InvoiceID.Equals(id));
               
           
            //To-Do
            //Add Total property in OrderDetails Table and Sum the prices with the discount

            if (order == null)
            {
                return RedirectToAction("Index", "Books");
            }
            else
            {
                ViewBag.TotalCost = order.OrderItems.Sum(x => x.BookPrice);
                updateCartIcon();
                return View(order);
            }

        }

        //GET: Chat (Redirect)
        public ActionResult TradeContact(string user)
        {
            Chat tmp = db.Chat.FirstOrDefault(x => (x.User2.Equals(User.Identity.Name) && x.User1.Equals(user)) || (x.User1.Equals(User.Identity.Name) && x.User2.Equals(user)));

            if(tmp == null)
            {
               tmp = db.Chat.Add(new Chat()
                {
                    User1 = User.Identity.Name,
                    User2 = user,
                    Messages = new List<ChatMessages>()
                });
                db.SaveChanges();
            }


            updateCartIcon();
            return RedirectToAction("Index", "Chat", new { Id = tmp.ChatId });
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
