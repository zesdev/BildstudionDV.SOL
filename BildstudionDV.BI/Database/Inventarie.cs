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
    public class Inventarie
    {
        BildStudionDVContext context;
        IMongoCollection<InventarieModel> inventariedb;
        public Inventarie(BildStudionDVContext _context)
        {
            context = _context;
            inventariedb = context.database.GetCollection<InventarieModel>("inventarier");
        }
        public void AddInventarie(InventarieModel model)
        {
            inventariedb.InsertOne(model);
        }
        public void RemoveInventarie(ObjectId Id)
        {
            inventariedb.FindOneAndDelete<InventarieModel>(x => x.Id == Id);
        }
        public void RemoveAllInventarierInGrupp(ObjectId GruppId)
        {
            var jobbInGroup = inventariedb.Find<InventarieModel>(x => x.GruppId == GruppId).ToList();
            foreach (var jobb in jobbInGroup)
            {
                RemoveInventarie(jobb.Id);
            }
        }
        public List<InventarieModel> GetListOfInventarierInGrupp(ObjectId GruppId)
        {
            var list = inventariedb.Find<InventarieModel>(x => x.GruppId == GruppId).ToList();
            return list;
        }

        public void UpdateInventarieItem(InventarieModel model)
        {
            UpdateProperty("InventarieNamn", model.Id, model.InventarieNamn);
            UpdateProperty("InventarieKommentar", model.Id, model.InventarieKommentar);
            UpdateProperty("Antal", model.Id, model.Antal);
            UpdateProperty("Fabrikat", model.Id, model.Fabrikat);
            UpdateProperty("Pris", model.Id, model.Pris);
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<InventarieModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<InventarieModel>.Update.Set(property, newpropertyContent);
            inventariedb.UpdateOne(filter, update);
        }
    }
}
