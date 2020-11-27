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
        private readonly RoomServices _roomService;

        public ChatHub(UserService userService, RoomServices roomService)
        {
            _userService = userService;
            _roomService = roomService;
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


        public async Task CreateRoom(User from, User to)
        {
            // Create and save chat room in database
            List<Room> rooms = _roomService.Get();
            Room roomFound = _roomService.Get(from.Id + to.Id);
            Room roomBidirectional = _roomService.Get(to.Id + from.Id);

            if (roomBidirectional != null)
            {
                await Clients.Caller.SendAsync("getCurrentRoom", roomBidirectional, to);
            }
            else {
                if (roomFound == null)
                {
                    Room newRoom = new Room();
                    newRoom.IdRoom = from.Id + to.Id;
                    _roomService.Create(newRoom);
                    await Clients.Caller.SendAsync("getCurrentRoom", newRoom, to);
                }
                else {
                    await Clients.Caller.SendAsync("getCurrentRoom", roomFound, to);
                }
            }
            
        }
    }
        public static class ConnectedUsers
    {
        public static List<User> online = new List<User>();
    }

}
