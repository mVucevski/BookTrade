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
    public class ChatAPIController : ApiController
    {
        private BookContext db = new BookContext();

        // GET: api/ChatAPI
        /* public IQueryable<ChatMessages> GetChatMessages()
         {
             return db.ChatMessages;
         } */

        // GET: api/ChatAPI/5
        [Route("api/ChatAPI/{ChatId}/{MsgId}")]
        [ResponseType(typeof(ChatMessages))]
        public IHttpActionResult GetChatMessages(int ChatId, int MsgId)
        {

            ChatMessages newMsg = null;
            var poraki = db.ChatMessages.Where(x => x.ChatId == ChatId);
            if (poraki != null)
            {
                var list = poraki.ToList();

                for(int z = 0; z<list.Count;z++)
                {
                    if (list[z].ChatMessagesId == MsgId)
                    {
                        if(z+1 != list.Count)
                        {
                            newMsg = list[z + 1];
                        }
                    }
                }

            }
         
            if (newMsg == null)
            {
                return Ok();
            }

            return Ok(newMsg);
        }

        // PUT: api/ChatAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChatMessages(int id, ChatMessages chatMessages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chatMessages.ChatMessagesId)
            {
                return BadRequest();
            }

            db.Entry(chatMessages).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessagesExists(id))
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

        private Boolean whoPostedFirst(string email)
        {
            return !User.Identity.Name.Equals(email);
        }
        
        // POST
        public void Post(ChatMessagesData msgData)
        {
           // System.Diagnostics.Debug.WriteLine("ChatId1111111111111111111111 " + msgData.Message);

            var chat = db.Chat.Find(msgData.ChatId);
            ChatMessages poraka = new ChatMessages()
            {
                Message = msgData.Message,
                Date = DateTime.Now,
                PostedBy = whoPostedFirst(chat.User1)
            };

            if (chat != null)
            {
                chat.Messages.Add(poraka);
                db.SaveChanges();
            }
        }


        // DELETE: api/ChatAPI/5
        [ResponseType(typeof(ChatMessages))]
        public IHttpActionResult DeleteChatMessages(int id)
        {
            ChatMessages chatMessages = db.ChatMessages.Find(id);
            if (chatMessages == null)
            {
                return NotFound();
            }

            db.ChatMessages.Remove(chatMessages);
            db.SaveChanges();

            return Ok(chatMessages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatMessagesExists(int id)
        {
            return db.ChatMessages.Count(e => e.ChatMessagesId == id) > 0;
        }
    }
}