using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models.Jobb;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class Kund
    {
        BildStudionDVContext context;
        IMongoCollection<KundModel> kunddb;
        Jobb jobb;
        public Kund(BildStudionDVContext _context, Jobb _jobb)
        {
            context = _context;
            kunddb = context.database.GetCollection<KundModel>("kunder");
            jobb = _jobb;
        }

        public void AddKund(KundModel model)
        {
            kunddb.InsertOne(model);
        }
        public void RemoveKund(ObjectId Id)
        {
            jobb.RemoveAllJobbsFörKund(Id);
            kunddb.FindOneAndDelete<KundModel>(x => x.Id == Id);
        }

        public KundModel GetKund(ObjectId Id)
        {
            return kunddb.Find<KundModel>(x => x.Id == Id).First();
        }
        public List<KundModel> GetAllKunder()
        {
            return kunddb.Find<KundModel>(x => true).ToList();
        }
        public void UpdateKund(KundModel model)
        {
            UpdateProperty("KundNamn", model.Id, model.KundNamn);
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<KundModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<KundModel>.Update.Set(property, newpropertyContent);
            kunddb.UpdateOne(filter, update);
        }

    }
}
