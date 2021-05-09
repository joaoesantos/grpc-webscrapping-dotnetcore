using System;

namespace ConsoleApp.WebScrapping.Definition
{
    public interface IScrappingData
    {
        int GetHash();
        string GetNotificationMessage();
        string GetData();
        string GetName();
        long GetId();
        IScrappingDataIdentifier GetIdentifier();
    }
}