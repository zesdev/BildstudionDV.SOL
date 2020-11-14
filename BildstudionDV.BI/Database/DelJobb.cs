using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models.Jobb;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class DelJobb
    {
        BildStudionDVContext context;
        IMongoCollection<DelJobbModel> deljobbdb;
        public DelJobb(BildStudionDVContext _context)
        {
            context = _context;
            deljobbdb = context.database.GetCollection<DelJobbModel>("deljobbs");
        }

        public void AddDelJobb(DelJobbModel model)
        {
            deljobbdb.InsertOne(model);
        }

        public void RemoveDelJobb(ObjectId Id)
        {
            deljobbdb.FindOneAndDelete<DelJobbModel>(x => x.Id == Id);
        }

        public void RemoveAllDelJobbsInJobb(ObjectId jobbId)
        {
            var deljobbs = deljobbdb.Find<DelJobbModel>(x => x.JobbId == jobbId).ToList();
            foreach (var deljobb in deljobbs)
            {
                RemoveDelJobb(deljobb.Id);
            }
        }
        public void UpdateDelJobb(DelJobbModel model)
        {
            UpdateProperty("Namn", model.Id, model.Namn);
            UpdateProperty("VemGör", model.Id, model.VemGör);
            UpdateProperty("StatusPåJobbet", model.Id, model.StatusPåJobbet.ToString());
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<DelJobbModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<DelJobbModel>.Update.Set(property, newpropertyContent);
            deljobbdb.UpdateOne(filter, update);
        }
        public List<DelJobbModel> GetDelJobbsInJobb(ObjectId jobbId)
        {
            return deljobbdb.Find<DelJobbModel>(x => x.JobbId == jobbId).ToList();
        }
        public DelJobbModel GetDelJobb(ObjectId Id)
        {
            return deljobbdb.Find(x => x.Id == Id).First();
        }
    }
}
