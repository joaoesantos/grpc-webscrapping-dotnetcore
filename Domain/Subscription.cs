namespace Domain
{
    public class Subscription
    {
        public long Id { get; set; }
        public long SubscriberId { get; set; }
        public long SearchId { get; set; }
        
        public long LastReadId { get; set; }

        public WsRead LastRead { get; set; }
        public WebScrappingSearch SubscribedSearch { get; set; }
        
        public Subscriber Subscriber { get; set; }
    }
}