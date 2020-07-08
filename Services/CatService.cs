using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Cat> Get() =>
            _cats.Find(cat => true).ToEnumerable();


        public IEnumerable<Cat> Get(SortBy sortBy, bool decreasing = false)
        {
            var cats = _cats.Find(cat => true).ToEnumerable();

            List<Cat> ordered;
            
            switch (sortBy)
            {
                case SortBy.elo:
                    cats = cats.OrderBy(c => c.Elo);
                    break;

                case SortBy.occurence:
                    cats = cats.OrderBy(c => c.Occurences);
                    break;

                default:
                    break;
            }

            if (decreasing)
            {
                cats = cats.Reverse();
            }

            return cats;
        }

        public Cat Get(string id) =>
            _cats.Find<Cat>(cat => cat.Id == id).FirstOrDefault();

        public Cat Create(Cat cat)
        {
            cat.Elo = 1000;
            cat.Occurences = 0;
            _cats.InsertOne(cat);
            return cat;
        }

        public void Update(string id, Cat catIn) =>
            _cats.ReplaceOne(cat => cat.Id == id, catIn);

        public void Remove(Cat catIn) =>
            _cats.DeleteOne(cat => cat.Id == catIn.Id);

        public void Remove(string id) =>
            _cats.DeleteOne(cat => cat.Id == id);

        public void ResetElo() =>
            _cats.UpdateMany(cat => true, Builders<Cat>.Update.Set("elo", 1000));

        public void ResetOccurence() =>
            _cats.UpdateMany(cat => true, Builders<Cat>.Update.Set("occurences", 0));

        public Cat[] GetRandomMatch()
        {
            var ran = new Random();
            return this.Get(SortBy.occurence).OrderBy(x => ran.NextDouble()).Take(2).ToArray();
        }

        public void SaveMatchResult(Cat winner, Cat loser)
        {
            int delta = EloService.CalculateDelta(winner.Elo, loser.Elo);

            winner.Elo += delta;
            loser.Elo -= delta;
            winner.Occurences++;
            loser.Occurences++;

            this.Update(winner.Id, winner);
            this.Update(loser.Id, loser);
        }

    }
}