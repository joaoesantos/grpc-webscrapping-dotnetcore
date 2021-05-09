using System.Collections.Generic;

namespace Domain
{
    public class WebScrappingSearch
    {
        public long Id { get; set; }
        public string Url { get; set; }
        
        public List<WsRead> Reads { get; set; }
        
        public List<Subscription> Subscriptions { get; set; }
    }
}