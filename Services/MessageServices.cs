using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class MessageServices
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageServices(IChatAppDatabaseSettings settings)
        {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _messages = database.GetCollection<Message>(settings.MessagesCollectionName);

        }

        public Message Create(Message message)
        {
            _messages.InsertOne(message);
            return message;
        }

    }
}
