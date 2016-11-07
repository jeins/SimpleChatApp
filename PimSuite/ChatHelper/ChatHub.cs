using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

        public void Connect(string fullName, int userId)
        {
            var id = Context.ConnectionId;

            if (!isUserIdExist(userId))
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

            Clients.Caller.onConnected(userId, fullName, id);
        }

        public void SendPrivateMessage(int toUserId, string message)
        {
            var fromUser = _db.Connections.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

            var toUser = _db.Connections.FirstOrDefault(u => u.UserId == toUserId);
            var dateNow = DateTime.Now;

//            Clients.Client(fromUser.ConnectionId).sendPrivateMessage(toUser.UserId, toUser.FullName, message);
//            Clients.Client(toUser.ConnectionId).sendPrivateMessage(fromUser.UserId, fromUser.FullName, message);

            Clients.Client(toUser.ConnectionId).sendPrivateMessage(fromUser.FullName, message, dateNow, "receiver");
            Clients.Client(fromUser.ConnectionId).sendPrivateMessage(fromUser.FullName, message, dateNow, "sender");
            
            var chat = new Chats
            {
                UserId = fromUser.UserId,
                ChatToUserId = toUserId,
                Message = message,
                CreatedAt = DateTime.Now
            };
            
            _db.Chats.Add(chat);
            _db.SaveChanges();
        }

        private bool isUserIdExist(int userId)
        {
            var count = _db.Connections.Count(u => u.UserId == userId);
            if (count > 0)
                return true;
            return false;
        }
    }
}