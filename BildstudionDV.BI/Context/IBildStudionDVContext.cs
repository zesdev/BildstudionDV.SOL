using MongoDB.Driver;

namespace BildstudionDV.BI.Context
{
    public interface IBildStudionDVContext
    {
        MongoClient client { get; set; }
        IMongoDatabase database { get; set; }

        void DropDataBase();
    }
}