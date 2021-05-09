using System;

namespace ConsoleApp.WebScrapping.Definition
{
    public interface IScrappingDataIdentifier
    {
        string GetSearchUrl();
        string GetItemName();
    }
}