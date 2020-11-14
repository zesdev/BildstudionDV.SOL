using BildstudionDV.BI.Models.Jobb;
using MongoDB.Bson;

namespace BildstudionDV.BI.ViewModels
{
    public class DelJobbViewModel
    {
        public ObjectId Id { get; set; }
        public ObjectId JobbId { get; set; }
        public DelJobbStatus StatusPåJobbet { get; set; }
        public string Namn { get; set; }
        public string VemGör { get; set; }
    }
}