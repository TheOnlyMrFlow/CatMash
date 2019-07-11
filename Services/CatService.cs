using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatMash.Models;

namespace CatMash.Services
{
    public class CatService
    {
        private readonly IMongoCollection<Cat> _cats;

        public enum SortBy
        {
            elo,
            occurence
        }

        public CatService(ICatMashDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _cats = database.GetCollection<Cat>(settings.CatsCollectionName);
        }

        public List<Cat> Get() =>
            _cats.Find(cat => true).ToList();


        public List<Cat> Get(SortBy sortBy, bool decreasing = false)
        {
            var cats = _cats.Find(cat => true).ToList();

            List<Cat> ordered;
            
            switch (sortBy)
            {
                case SortBy.elo:
                    ordered = cats.OrderBy(c => c.Elo).ToList();
                    break;

                case SortBy.occurence:
                    ordered = cats.OrderBy(c => c.Occurences).ToList();
                    break;

                default:
                    ordered = cats;
                    break;

            }

            if (decreasing)
            {
                ordered.Reverse();
            }

            return ordered;
        }

        public Cat Get(string id) =>
            _cats.Find<Cat>(cat => cat.Id == id).FirstOrDefault();

        public Cat Create(Cat cat)
        {
            _cats.InsertOne(cat);
            return cat;
        }

        public void Update(string id, Cat catIn) =>
            _cats.ReplaceOne(cat => cat.Id == id, catIn);

        public void Remove(Cat catIn) =>
            _cats.DeleteOne(cat => cat.Id == catIn.Id);

        public void Remove(string id) =>
            _cats.DeleteOne(cat => cat.Id == id);
    }
}