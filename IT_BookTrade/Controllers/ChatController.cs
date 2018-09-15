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
            /*string email = "user1@user.com";

            Chat tmp = db.Chat.FirstOrDefault(x => (x.User2.Equals(User.Identity.Name) && x.User1.Equals(email)) || (x.User1.Equals(User.Identity.Name) && x.User2.Equals(email)));

            if(tmp == null)
            {
                 db.Chat.Add(new Chat() {
                    User1 = User.Identity.Name,
                    User2 = email,
                    Messages = new List<ChatMessages>()
                });
                db.SaveChanges();
            }



            newMsg(tmp.ChatId, "Hello", false);

            return View(db.Chat.ToList()); */

            var tmp = db.Chat.FirstOrDefault(x => x.ChatId == Id);
            if (tmp != null)
            {
                if(!(tmp.User1.Equals(User.Identity.Name) || tmp.User2.Equals(User.Identity.Name))){
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


        private void newMsg(int id, string msg, bool by) {

            db.Chat.Find(id).Messages.Add(new ChatMessages() {
                Message = msg,
                Date = DateTime.Now,
                PostedBy = by
            });

            db.SaveChanges();
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
