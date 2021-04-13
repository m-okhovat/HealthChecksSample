using System;

namespace GloboTicket.Services.ShoppingBasket.Messages
{
    public class BasketLineMessage
    {
        public Guid BasketLineId { get; set; }
        public int Price { get; set; }
        public int TicketAmount { get; set; }
    }
}
