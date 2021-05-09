using System;
using ConsoleApp.WebScrapping.Definition;

namespace ConsoleApp.WebScrapping.Implementation
{
    public class ScrappingDataIdentifier : IScrappingDataIdentifier
    {
        private readonly string _url;
        private readonly string _itemName;

        public ScrappingDataIdentifier(string url, string itemName)
        {
            _url = url;
            _itemName = itemName;
        }

        public string GetSearchUrl()
        {
            return _url;
        }

        public string GetItemName()
        {
            return _itemName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is not ScrappingDataIdentifier)
            {
                return false;
            }

            ScrappingDataIdentifier other = (ScrappingDataIdentifier) obj;

            return Equals(other);
        }

        protected bool Equals(ScrappingDataIdentifier other)
        {
            return _url == other._url && _itemName == other._itemName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_url, _itemName);
        }
    }
}