using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PimSuite.Models;

namespace PimSuite.ChatHelper
{
    public class ChatHub : Hub
    {
        private PimSuiteDatabaseEntities _db;
        public ChatHub()
        {
            _db = new PimSuiteDatabaseEntities();
        }

        public void SendMessage(int fromUserId, string message)
        {
            var user = _db.Connections.FirstOrDefault(u => u.UserId == fromUserId);
            var date = DateTime.Now.ToString("g");
            
            Clients.Others.addNewMessageFrom(user.FullName, message, date, "receiver");
            Clients.Client(Context.ConnectionId).addUserMessage(user.FullName, message, date, "sender");
            
            var chat = new Chats
            {
                UserId = fromUserId,
                Message = message,
                CreatedAt = DateTime.Now
            };

            _db.Chats.Add(chat);
            _db.SaveChanges();
        }

        public void Connect(string fullName, int userId)
        {
            var id = Context.ConnectionId;
            var users = _db.Connections.Where(u=>u.UserId != userId).Select(u => u.FullName);

            if (!IsUserIdExist(userId))
            {
                var connection = new Connections
                {
                    ConnectionId = id,
                    UserId = userId,
                    FullName = fullName
                };
                _db.Connections.Add(connection);
                _db.SaveChanges();
            }
            else
            {
                var connection = _db.Connections.SingleOrDefault(u => u.UserId == userId);
                if (connection != null)
                {
                    connection.ConnectionId = id;
                    _db.SaveChanges();
                }
            }

            Clients.Caller.onConnected(users);
            Clients.Caller.loadChatHistory(GetChatHistory(userId));
            Clients.AllExcept(id).onNewUserConnected(userId, fullName);
        }

        private List<string[]> GetChatHistory(int userId)
        {
            var chatsHistory = new List<string[]>();
            var tmpChatsHistory = _db.Chats.Select(c => new { c.UserId ,c.Users.FirstName, c.Users.LastName, c.Message, c.CreatedAt }).ToList();

            foreach (var chat in tmpChatsHistory)
            {
                var senderOrReceiver = (chat.UserId == userId) ? "sender" : "receiver";
                var arr = new string[]
                {
                    chat.FirstName + " " + chat.LastName,
                    chat.Message,
                    chat.CreatedAt.ToString("g"),
                    senderOrReceiver
                };

                chatsHistory.Add(arr);
            }

            return chatsHistory;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connection = _db.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (connection != null)
            {
                var userId = connection.UserId;
                var fullName = connection.FullName;

                _db.Connections.Remove(connection);
                _db.SaveChanges();

                Clients.All.onUserDisconnected(userId);
            }

            return base.OnDisconnected(stopCalled);
        }

        private bool IsUserIdExist(int userId)
        {
            var count = _db.Connections.Count(u => u.UserId == userId);
            if (count > 0)
                return true;
            return false;
        }
    }
}