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
    public class ChatController : Controller
    {
        private BookContext db = new BookContext();
        private ApplicationDbContext usersDB = new ApplicationDbContext();

        // GET: Chat
        public ActionResult Index(int Id)
        {
            var tmp = db.Chat.FirstOrDefault(x => x.ChatId == Id);
            updateCartIcon();
            if (tmp != null)
            {
                if(!(tmp.User1.Equals(User.Identity.Name) || tmp.User2.Equals(User.Identity.Name)) || User.Identity.Name.Length < 1){
                    return RedirectToAction("Index", "Books", new { });
                }
                ApplicationUser contactedUser;

                if (User.Identity.Name.Equals(tmp.User1))
                {
                    //false = logiraniot user e prv
                    ViewBag.LoggedInUser = false;
                    contactedUser = usersDB.Users.Where(x => x.Email.Equals(tmp.User2)).FirstOrDefault();
                }
                else
                {
                    ViewBag.LoggedInUser = true;
                    contactedUser = usersDB.Users.Where(x => x.Email.Equals(tmp.User1)).FirstOrDefault();
                }

                ViewBag.UserName = contactedUser.FirstName + " " + contactedUser.LastName;
                return View(tmp);
            }
            else
            {
                return RedirectToAction("Index", "Books", new { });
            }
        }

        private Boolean whoPostedFirst(string email)
        {
            return !User.Identity.Name.Equals(email);
        }

        [HttpPost]
        public ActionResult SendMsg(int ChatId, string Msg)
        {
            //System.Diagnostics.Debug.WriteLine("ChatID " + ChatId);
            //System.Diagnostics.Debug.WriteLine("Msg " + Msg);

            updateCartIcon();
            var chat = db.Chat.Find(ChatId);
            if(chat != null)
            {
                chat.Messages.Add(new ChatMessages() {
                    Message = Msg,
                    Date = DateTime.Now,
                    PostedBy = whoPostedFirst(chat.User1)
                });
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Chat", new { Id = ChatId });
        }

        // GET: Inbox
        [Authorize]
        public ActionResult Inbox()
        {
            List<InboxItem> inboxList = new List<InboxItem>();
            var chatsCheck = db.Chat.Where(x => x.User1.Equals(User.Identity.Name) || x.User2.Equals(User.Identity.Name));
            List<Chat> chats = null;
            updateCartIcon();

            if (chatsCheck.Any())
            {
                chats = chatsCheck.ToList();


                string msg;
                ApplicationUser sender;

                if (chats != null)
                {
                    foreach (var c in chats)
                    {
                        if (c.Messages.Any())
                        {
                            var lastMsg = c.Messages.Last();
                            if (lastMsg != null)
                            {
                                if (!lastMsg.PostedBy)
                                {
                                    if (c.User1.Equals(User.Identity.Name))
                                    {
                                        msg = "You: " + lastMsg.Message;
                                        sender = usersDB.Users.Where(x => x.Email.Equals(c.User2)).FirstOrDefault();
                                    }
                                    else
                                    {
                                        msg = lastMsg.Message;
                                        sender = usersDB.Users.Where(x => x.Email.Equals(c.User1)).FirstOrDefault();
                                    }
                                }
                                else
                                {
                                    if (c.User2.Equals(User.Identity.Name))
                                    {
                                        msg = "You: " + lastMsg.Message;
                                        sender = usersDB.Users.Where(x => x.Email.Equals(c.User1)).FirstOrDefault();
                                    }
                                    else
                                    {
                                        msg = lastMsg.Message;
                                        sender = usersDB.Users.Where(x => x.Email.Equals(c.User2)).FirstOrDefault();
                                    }
                                }
                                inboxList.Add(new InboxItem()
                                {
                                    Msg = msg,
                                    Date_Msg = lastMsg.Date,
                                    SenderName = sender.FirstName + " " + sender.LastName,
                                    ChatID = lastMsg.ChatId
                                });
                            }
                        }
                    }
                }


                if (inboxList.Any())
                {
                    inboxList = inboxList.OrderByDescending(x => x.Date_Msg).ToList();
                }
            }

            return View(inboxList);
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
