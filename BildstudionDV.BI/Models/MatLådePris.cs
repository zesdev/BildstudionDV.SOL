using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models
{
    class MatLådePris
    {
        public ObjectId Id { get; set; }
        public int Pris { get; set; }
    }
}
