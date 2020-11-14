using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace BildstudionDV.BI.Context
{
    public class BildStudionDVContext : IBildStudionDVContext
    {
        public MongoClient client { get; set; }
        public IMongoDatabase database { get; set; }
        public BildStudionDVContext(string username, string password)
        {
            client = new MongoClient("mongodb+srv://" + username + ":" + password + "@cluster0.mswfg.mongodb.net/BildStudionDV?retryWrites=true&w=majority");
            database = client.GetDatabase("BildStudionDV");
        }
        public void DropDataBase()
        {
            client.DropDatabase("BildStudionDV");
        }
    }
}
