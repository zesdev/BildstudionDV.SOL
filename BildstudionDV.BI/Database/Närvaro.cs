using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models.Attendence;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class Närvaro
    {
        BildStudionDVContext context;
        IMongoCollection<AttendenceModel> närvarodb;
        public Närvaro(BildStudionDVContext _context)
        {
            context = _context;
            närvarodb = context.database.GetCollection<AttendenceModel>("närvaro");
        }

        public void AddAttendence(AttendenceModel model)
        {
            närvarodb.InsertOne(model);
        }
        public void RemoveAttendence(ObjectId Id)
        {
            närvarodb.FindOneAndDelete<AttendenceModel>(x => x.Id == Id);
        }

        public AttendenceModel GetAttendenceItem(ObjectId Id)
        {
            var attendenceitem = närvarodb.Find<AttendenceModel>(x => x.Id == Id).First();
            return attendenceitem;
        }

        public List<AttendenceModel> GetAllAttendenceItems()
        {
            var list = närvarodb.Find<AttendenceModel>(x => true).ToList();
            return list;
        }
        public void UpdateAttendence(AttendenceModel model)
        {
            UpdateProperty("Måndag", model.Id, model.Måndag.ToString());
            UpdateProperty("Tisdag", model.Id, model.Tisdag.ToString());
            UpdateProperty("Onsdag", model.Id, model.Onsdag.ToString());
            UpdateProperty("Torsdag", model.Id, model.Torsdag.ToString());
            UpdateProperty("Fredag", model.Id, model.Fredag.ToString());
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<AttendenceModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<AttendenceModel>.Update.Set(property, newpropertyContent);
            närvarodb.UpdateOne(filter, update);
        }
    }
}
