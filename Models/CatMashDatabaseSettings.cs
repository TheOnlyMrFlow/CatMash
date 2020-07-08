using System;

namespace CatMash.Models
{
    public class CatMashDatabaseSettings : ICatMashDatabaseSettings
    {
        public string CatsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICatMashDatabaseSettings
    {
        string CatsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}


