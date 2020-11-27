using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Models
{
    public class Room
    {
        [BsonId]
        public string IdRoom { get; set; }
        public List<Message> chatMessages { get; set; }

    }
}
