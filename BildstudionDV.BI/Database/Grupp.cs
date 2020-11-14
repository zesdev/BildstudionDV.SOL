using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models.Inventarie;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class Grupp
    {
        BildStudionDVContext context;
        IMongoCollection<GruppModel> gruppdb;
        Inventarie inventarie;
        public Grupp(BildStudionDVContext _context, Inventarie _inventarie)
        {
            context = _context;
            inventarie = _inventarie;
            gruppdb = context.database.GetCollection<GruppModel>("grupper");
        }
        public void AddGrupp(GruppModel model)
        {
            gruppdb.InsertOne(model);
        }
        public void RemoveGrupp(ObjectId Id)
        {
            inventarie.RemoveAllInventarierInGrupp(Id);
            gruppdb.FindOneAndDelete<GruppModel>(x => x.Id == Id);
        }
        public List<GruppModel> GetAllGruppsInEnhet(ObjectId enhetId)
        {
            var list = gruppdb.Find<GruppModel>(x => x.EnhetId == enhetId).ToList();
            return list;
        }
        public GruppModel GetGrupp(ObjectId Id)
        {
            return gruppdb.Find<GruppModel>(x => x.Id == Id).First();
        }
        public void RemoveAllGruppsInEnhet(ObjectId Id)
        {
            var list = GetAllGruppsInEnhet(Id);
            foreach (var item in list)
            {
                RemoveGrupp(item.Id);
            }
        }
        public void UpdateGruppModel(GruppModel model)
        {
            UpdateProperty("GruppNamn", model.Id, model.GruppNamn);
            UpdateProperty("EnhetId", model.Id, model.EnhetId);
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, ObjectId EnhetId)
        {
            var filter = Builders<GruppModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<GruppModel>.Update.Set(property, EnhetId);
            gruppdb.UpdateOne(filter, update);
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<GruppModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<GruppModel>.Update.Set(property, newpropertyContent);
            gruppdb.UpdateOne(filter, update);
        }
    }
}
