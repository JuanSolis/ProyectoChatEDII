using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Models;
using ChatApp.Services;
using Diffie_Hellman;
using SDES;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public int UsersOnline = 0;
        public string connectionId = "";

        private readonly UserService _userService;
        private readonly RoomServices _roomService;
        private readonly MessageServices _messageService;
        public ChatHub(UserService userService, RoomServices roomService, MessageServices messageService)
        {
            _userService = userService;
            _roomService = roomService;
            _messageService = messageService;
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
            DiffieHellmanKey diffie = new DiffieHellmanKey();
            diffie.DiffleHellmanKey();

            from.n =  diffie.generarNum1();

            _userService.Update(from, from);

            to.n = diffie.generarNum2();

            _userService.Update(to, to);
    
            
            // Create and save chat room in database
            List<Room> rooms = _roomService.Get();
            Room roomFound = _roomService.Get(from.Id + to.Id);
            Room roomBidirectional = _roomService.Get(to.Id + from.Id);

            if (roomBidirectional != null)
            {
                await Clients.Caller.SendAsync("getCurrentRoom", roomBidirectional, to, from);
            }
            else {
                if (roomFound == null)
                {
                    Room newRoom = new Room();
                    newRoom.IdRoom = from.Id + to.Id;
                    newRoom.chatMessages = new List<Message>();
                    _roomService.Create(newRoom);
                    await Clients.Caller.SendAsync("getCurrentRoom", newRoom, to, from);
                }
                else {
                    await Clients.Caller.SendAsync("getCurrentRoom", roomFound, to, from);
                }
            }
            
        }

        public async Task SendMessage(Message message) {

            DiffieHellmanKey diffie = new DiffieHellmanKey();

            diffie.DiffleHellmanKey();

            CifradoSDES sdesClass = new CifradoSDES();

            User userEntry = _userService.GetByUsername(message.SenderUser);

            string cypher = sdesClass.Cifrar(message.content, diffie.ObtenerLlave(userEntry.n));

            message.content = cypher;

            _messageService.Create(message);

            var room = _roomService.Get(message.room);
            room.chatMessages.Add(message);

             _roomService.Update(room, room);

            ////Caller
            await Clients.Group(message.room).SendAsync("Messages", room.chatMessages);
        }

        public List<Message> DechyperMessage(List<Message> messageList)
        {
            if (messageList.Count > 0)
            {
                
                foreach (Message msg in messageList)
                {
                    DiffieHellmanKey diffie = new DiffieHellmanKey();

                    diffie.DiffleHellmanKey();

                    CifradoSDES sdesClass = new CifradoSDES();
                    User userEntry = _userService.GetByUsername(messageList[0].SenderUser);
                    string decypher = "";
                    decypher = sdesClass.Descifrar(msg.content, diffie.ObtenerLlave(userEntry.n));
                    msg.content = decypher;
                }

                //_messageService.Create(message);

                //var room = _roomService.Get(message.room);
                //room.chatMessages.Add(message);

                //_roomService.Update(room, room);
            }

            return messageList;
        }

        public async Task GetRoomMessages(string idRoom) {

            var room = _roomService.Get(idRoom);
            //Caller
            //Clients.Caller.SendAsync("GetMessages", room.chatMessages);
            await Clients.Group(idRoom).SendAsync("Messages", room.chatMessages);
        }


        public async Task JoinRoom(Room lastRoom, Room room)
        {

            // Join to new chat room
            if (lastRoom != null)
            {
                if (lastRoom.IdRoom != room.IdRoom)
                {
                    await Leave(lastRoom.IdRoom);
                    await Groups.AddToGroupAsync(Context.ConnectionId, room.IdRoom);
                    await Clients.Caller.SendAsync("getLastRoom", room);
                }else
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, room.IdRoom);
                    await Clients.Caller.SendAsync("getLastRoom", room);

                }
            }
            else {
                await Groups.AddToGroupAsync(Context.ConnectionId, room.IdRoom);
                await Clients.Caller.SendAsync("getLastRoom", room);
            }
           
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

    }
        public static class ConnectedUsers
    {
        public static List<User> online = new List<User>();
    }

}
