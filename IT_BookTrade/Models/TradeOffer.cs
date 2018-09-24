using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_BookTrade.Models
{
    public class TradeOffer
    {
        public int TradeOfferId { get; set; }
        public string UserSender { get; set; }
        public string UserReceiver { get; set; }
        public virtual Book SendersBook { get; set; }
        public virtual Book ReceiverBook { get; set; }
        public bool Respond { get; set; }
        public bool Accepted { get; set; }
    }
}