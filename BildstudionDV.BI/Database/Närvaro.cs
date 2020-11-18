using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models.Attendence;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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

        internal void RemoveAllAttendenceForDeltagare(ObjectId deltagarId)
        {
            var attendenceModels = närvarodb.Find<AttendenceModel>(x => x.DeltagarIdInQuestion == deltagarId).ToList();
            foreach (var model in attendenceModels)
            {
                RemoveAttendence(model.Id);
            }
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

        internal List<AttendenceModel> GetAttendenceItemsFörDeltagare(ObjectId deltagarId)
        {
            return närvarodb.Find<AttendenceModel>(x => x.DeltagarIdInQuestion == deltagarId).ToList();
        }

        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<AttendenceModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<AttendenceModel>.Update.Set(property, newpropertyContent);
            närvarodb.UpdateOne(filter, update);
        }

        internal List<AttendenceModel> GetAttendenceForDate(DateTime date)
        {
            var list = närvarodb.Find<AttendenceModel>(x => true).ToList();
            date = date.AddHours(-1);
            var sortedList = list.Where(x => x.DateConcerning.DayOfYear == date.DayOfYear && x.DateConcerning.Year == date.Year).ToList();
            
            return sortedList;
        }
    }
}
