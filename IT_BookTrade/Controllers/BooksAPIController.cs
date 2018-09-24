using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using IT_BookTrade.Models;

namespace IT_BookTrade.Controllers
{
    public class BooksAPIController : ApiController
    {
        private BookContext db = new BookContext();
        private ApplicationDbContext usersDB = new ApplicationDbContext();

        // GET: api/BooksAPI
        public IQueryable<Book> GetBooks()
        {
            return db.Books;
        }

        // GET: api/BooksAPI/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult GetBook(int option, int id)
        {
            Book book = db.Books.Find(id);
            if (option == 0)
            {
                AddToCart(book);

                return Ok(book);
            }
            else if (option == 1)
            {
                RemoveBookFromCart(id);

                return Ok(book);
            }

            return BadRequest();

        }

        // GET: api/BooksAPI/id/id
        [Route("api/BooksAPI/{book1}/{book2}")]
        public IHttpActionResult GetTrade(int book1, int book2)
        {
            System.Diagnostics.Debug.WriteLine("book1 " + book1 + " book2 " + book2);

            Book b1 = db.Books.Find(book1);
            Book b2 = db.Books.Find(book2);

            if (db.TradeOffers.Any(x => x.SendersBook.ID == book1 && x.ReceiverBook.ID == book2 && x.Respond == false))
            {
                return Ok();
            }
            else if (db.TradeOffers.Any(x => x.SendersBook.ID == book2 && x.ReceiverBook.ID == book1 && x.Respond == false))
            {
                return Ok();
            }

            if (!(b1.Tradeable && b2.Tradeable))
            {
                return Ok();
            }

            TradeOffer offer = new TradeOffer()
            {
                UserSender = b1.SellerEmail,
                UserReceiver = b2.SellerEmail,
                SendersBook = b1,
                ReceiverBook = b2,
                Respond = false,
                Accepted = false
            };

            db.TradeOffers.Add(offer);
            db.SaveChanges();

            return Ok();
        }

        // PUT: api/BooksAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.ID)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BooksAPI
        [ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.ID }, book);
        }

        // DELETE: api/BooksAPI/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        //TOGGLE api/BooksAPI/5
        public IHttpActionResult AddToCartBook(int id)
        {
            Book book = db.Books.Find(id);

            if (!book.SellerEmail.Equals(User.Identity.Name))
            {
                AddToCart(book);
            }

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.ID == id) > 0;
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
            //updateCartIcon();
            //------> Add to cart function <-----
        }

        private void RemoveBookFromCart(int id)
        {
            //shoppingCart.Remove(db.Books.Find(id));

            /*for (int i = 0; i < shoppingCart.Count; ++i)
            {
                if (shoppingCart[i].ID == id)
                {
                    shoppingCart.RemoveAt(i);
                    break;
                }
            }*/


            //------> Remove from cart function <-----
            var cart = db.ShoppingCart.Where(x => x.UserEmail.Equals(User.Identity.Name)).FirstOrDefault();

            if (cart != null)
            {
                var item = cart.ShoppingCartItems.Where(x => x.Book.ID == id).First();

                if (item != null)
                {
                    cart.TotalPrice -= item.Book.Price;
                    cart.ShoppingCartItems.Remove(item);
                    db.ShoppingCartItems.Remove(item);

                    db.SaveChanges();
                }
            }

            //------> Remove from cart function <-----
            //updateCartIcon();
        }
    }
}