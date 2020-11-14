using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models.Jobb;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class Jobb
    {
        BildStudionDVContext context;
        IMongoCollection<JobbModel> jobbdb;
        DelJobb delJobb;
        public Jobb(BildStudionDVContext _context, DelJobb _delJobb)
        {
            context = _context;
            jobbdb = context.database.GetCollection<JobbModel>("jobbs");
            delJobb = _delJobb;
        }
        public JobbModel GetJobb(ObjectId Id)
        {
            return jobbdb.Find<JobbModel>(x => x.Id == Id).First();
        }
        public List<JobbModel> GetAllJobbs()
        {
            return jobbdb.Find<JobbModel>(x => true).ToList();
        }
        public void AddJobb(JobbModel model)
        {
            jobbdb.InsertOne(model);
        }
        public void RemoveJobb(ObjectId Id)
        {
            delJobb.RemoveAllDelJobbsInJobb(Id);
            jobbdb.FindOneAndDelete<JobbModel>(x => x.Id == Id);
        }
        public void RemoveAllJobbsFörKund(ObjectId kundId)
        {
            var jobbsInKund = jobbdb.Find<JobbModel>(x => x.KundId == kundId).ToList();
            foreach (var jobb in jobbsInKund)
            {
                RemoveJobb(jobb.Id);
            }
        }
        public void UpdateJobb(JobbModel model)
        {
            UpdateProperty("Title", model.Id, model.Title);
            UpdateProperty("TypAvJobb", model.Id, model.TypAvJobb.ToString());
            UpdateProperty("TypAvPrioritet", model.Id, model.TypAvPrioritet.ToString());
            UpdateProperty("StatusPåJobbet", model.Id, model.StatusPåJobbet.ToString());
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<JobbModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<JobbModel>.Update.Set(property, newpropertyContent);
            jobbdb.UpdateOne(filter, update);
        }
    }
}
