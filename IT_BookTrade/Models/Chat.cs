using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public virtual List<ChatMessages> Messages { get; set; }
    }

    public class ChatMessages
    {
        public int ChatMessagesId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool PostedBy { get; set; }
        public int ChatId { get; set; }
    }
}