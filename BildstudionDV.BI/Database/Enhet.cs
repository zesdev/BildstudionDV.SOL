using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.Models.Inventarie;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class Enhet
    {
        BildStudionDVContext context;
        IMongoCollection<EnhetModel> enhetdb;
        Grupp grupp;
        public Enhet(BildStudionDVContext _context, Grupp _grupp)
        {
            context = _context;
            grupp = _grupp;
            enhetdb = context.database.GetCollection<EnhetModel>("enheter");
        }

        public void AddEnhet(EnhetModel model)
        {
            enhetdb.InsertOne(model);
        }
        public void RemoveEnhet(ObjectId Id)
        {
            grupp.RemoveAllGruppsInEnhet(Id);
            enhetdb.FindOneAndDelete<EnhetModel>(x => x.Id == Id);
        }
        public EnhetModel GetEnhet(ObjectId Id)
        {
            return enhetdb.Find<EnhetModel>(x => x.Id == Id).First();
        }
        public List<EnhetModel> GetAllEnheter()
        {
            return enhetdb.Find<EnhetModel>(x => true).ToList();
        }
        public void UpdateEnhet(EnhetModel model)
        {
            UpdateProperty("Namn", model.Id, model.Namn);
            UpdateProperty("ChefNamn", model.Id, model.ChefNamn);
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<EnhetModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<EnhetModel>.Update.Set(property, newpropertyContent);
            enhetdb.UpdateOne(filter, update);
        }
    }
}
