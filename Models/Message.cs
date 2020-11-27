using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Models
{
    public class Message
    {
        [BsonId]
        public string Id { get; set; }

        public string SenderUser { get; set; }

        public string ReceiverUser { get; set; }

        public string Type { get; set; }

        public string room { get; set; }


    }
}
