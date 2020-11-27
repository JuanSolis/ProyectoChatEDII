using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class ChatAppDatabaseSettings: IChatAppDatabaseSettings
    {

        public string UsersCollectionName { get; set; }
        public string MessagesCollectionName { get; set; }
        public string RoomsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
    }

    public interface IChatAppDatabaseSettings
    {
        string MessagesCollectionName { get; set; }
        string RoomsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
