using System.Collections.Generic;

namespace Domain
{
    public class Subscriber
    {
        public long Id { get; set; }
        public string Username { get; set; }

        public List<Subscription> Subscriptions { get; set; }
    }
}