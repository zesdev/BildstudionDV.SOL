using BildstudionDV.BI.Models.Jobb;
using MongoDB.Bson;

namespace BildstudionDV.BI.ViewModels
{
    public class DelJobbViewModel
    {
        public int AccessId { get; set; }
        public ObjectId Id { get; set; }
        public ObjectId JobbId { get; set; }
        public string Kommentar { get; set; }
        public DelJobbStatus StatusPåJobbet { get; set; }
        public string Namn { get; set; }
        public string VemGör { get; set; }
    }
}