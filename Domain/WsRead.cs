using System;

namespace Domain
{
    public class WsRead
    {
        public long Id { get; set; }
        
        public DateTime ReadDateTime { get; set; }
        
        public string Data { get; set; }
        
        public long SearchId { get; set; }
        
        public string Name { get; set; }
        
        public WebScrappingSearch WebScrappingSearch { get; set; }
    }
}