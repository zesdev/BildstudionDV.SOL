using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class Deltagare
    {
        BildStudionDVContext context;
        IMongoCollection<DeltagareModel> deltagaredb;
        public Deltagare(BildStudionDVContext _context)
        {
            context = _context;
            deltagaredb = context.database.GetCollection<DeltagareModel>("deltagare");
        }
        public void AddDeltagare(DeltagareModel model)
        {
            deltagaredb.InsertOne(model);
        }
        public void RemoveDeltagare(ObjectId Id)
        {
            deltagaredb.DeleteOne<DeltagareModel>(x => x.Id == Id);
        }
        public List<DeltagareModel> GetAllDeltagarModels()
        {
            var list = deltagaredb.Find<DeltagareModel>(x => true).ToList();
            return list;
        }
        public DeltagareModel GetDeltagare(ObjectId Id)
        {
            var deltagarn = deltagaredb.Find<DeltagareModel>(x => x.Id == Id).First();
            return deltagarn;
        }
        public void UpdateDeltagare(DeltagareModel model)
        {
            UpdateProperty("DeltagarNamn", model.Id, model.DeltagarNamn);
            UpdateProperty("Måndag", model.Id, model.Måndag.ToString());
            UpdateProperty("Tisdag", model.Id, model.Tisdag.ToString());
            UpdateProperty("Onsdag", model.Id, model.Onsdag.ToString());
            UpdateProperty("Torsdag", model.Id, model.Torsdag.ToString());
            UpdateProperty("Fredag", model.Id, model.Fredag.ToString());
            UpdateProperty("IsActive", model.Id, model.IsActive.ToString());
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<DeltagareModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<DeltagareModel>.Update.Set(property, newpropertyContent);
            deltagaredb.UpdateOne(filter, update);
        }
    }
}
