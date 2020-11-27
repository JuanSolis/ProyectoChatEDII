using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class RoomServices
    {
        private readonly IMongoCollection<Room> _room;
        public RoomServices(IChatAppDatabaseSettings settings)
        {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _room = database.GetCollection<Room>(settings.RoomsCollectionName);

        }
        public Room Get(string id) =>
            _room.Find<Room>(room => room.IdRoom == id).FirstOrDefault();

        public List<Room> Get() => _room.Find(room => true).ToList();

        public Room Create(Room room)
        {
            _room.InsertOne(room);
            return room;
        }
    }
}
