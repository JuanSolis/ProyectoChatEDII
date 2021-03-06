﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        public UserService(IChatAppDatabaseSettings settings) {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);

        }

        public List<User> Get() => _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByUsername(string username) =>
            _users.Find<User>(user => user.Username == username).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public async void Update(User userToUpdate, User updatedUser)
        {
            await _users.ReplaceOneAsync(r => r.Id.Equals(userToUpdate.Id), updatedUser, new UpdateOptions { IsUpsert = true });
        }

    }
}
