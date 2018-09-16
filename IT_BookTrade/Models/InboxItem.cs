using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class InboxItem
    {
        public string SenderName { get; set; }
        public string Msg { get; set; }
        public DateTime Date_Msg { get; set; }
        public int ChatID { get; set; }
    }
}