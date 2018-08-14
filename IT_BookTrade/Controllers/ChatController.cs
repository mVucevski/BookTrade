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

        // GET: Chat
        public ActionResult Index()
        {
            string email = "user1@user.com";

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

            return View(db.Chat.ToList());
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
