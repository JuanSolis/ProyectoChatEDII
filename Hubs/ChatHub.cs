using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Models;
using ChatApp.Services;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public int UsersOnline = 0;
        public string connectionId = "";

        private readonly UserService _userService;

        public ChatHub(UserService userService)
        {
            _userService = userService;
        }

        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }

        public async Task insertConnectionToUser(User user)
        {


            bool contains = ConnectedUsers.online.Any(p => p.Username == user.Username);


            if (!contains)
            {

                ConnectedUsers.online.Add(user);
            }
            


            await Clients.All.SendAsync("onlineFriends", ConnectedUsers.online);
        }

        public List<User> onlineFriends(List<User> usersOnline)
        {

            return usersOnline;
        }


        public async Task CreateRoom(string roomName)
        {
            // Create and save chat room in database


            await Clients.All.SendAsync("addChatRoom");
        }
    }
        public static class ConnectedUsers
    {
        public static List<User> online = new List<User>();
    }

}
