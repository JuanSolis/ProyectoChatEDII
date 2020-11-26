using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Models;

namespace ChatApp.Hubs
{
    public class ChatHub: Hub
    {
        public int UsersOnline = 0;
        public string connectionId = "";
       
        public override Task OnConnectedAsync()
        {
           
            return base.OnConnectedAsync();
        }

        public async Task insertConnectionToUser(User user) {


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
    }
    public static class ConnectedUsers
    {
        public static List<User> online = new List<User>();
    }

}
