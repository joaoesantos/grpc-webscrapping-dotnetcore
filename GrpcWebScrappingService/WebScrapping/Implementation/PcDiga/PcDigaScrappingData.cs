using System;
using ConsoleApp.WebScrapping.Definition;
using Newtonsoft.Json;

namespace ConsoleApp.WebScrapping.Implementation.PcDiga
{
    public class PcDigaScrappingData : IScrappingData
    {
        private bool _inStock;
        private string _name;
        private double _price;
        private string _data;
        private ScrappingDataIdentifier _scrappingDataIdentifier;
        private long _id;

        public PcDigaScrappingData(string url, bool inStock, string name, double price, long id)
        {
            _inStock = inStock;
            _name = name;
            _price = price;
            _data = $"{{name:{_name}, stock:{_inStock}, price:{_price}}}";
            _scrappingDataIdentifier = new ScrappingDataIdentifier(url, name);
            _id = id;
        }

        public int GetHash()
        {
            return HashCode.Combine(_inStock, _name, _price);
        }

        public string GetNotificationMessage()
        {
            throw new System.NotImplementedException();
        }

        public string GetData()
        {
            return _data;
        }

        public string GetName()
        {
            return _name;
        }

        public long GetId()
        {
            return _id;
        }

        public IScrappingDataIdentifier GetIdentifier()
        {
            return _scrappingDataIdentifier;
        }
    }
}