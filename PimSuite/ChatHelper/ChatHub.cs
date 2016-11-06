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

            if (!ifConnectionIdExist(id))
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

            var curConnectedUser = _db.Connections.FirstOrDefault(u => u.ConnectionId == id);

            Clients.Caller.onConnected(curConnectedUser.UserId.ToString(), curConnectedUser.FullName, curConnectedUser.ConnectionId);
        }

        public void SendPrivateMessage(int toUserId, string message)
        {
            var fromUser =
                _db.Connections.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

            var toUser = _db.Connections.FirstOrDefault(u => u.UserId == toUserId);

            Clients.Client(toUser.ConnectionId).sendPrivateMessage(fromUser.UserId, fromUser.FullName, message);
            Clients.Client(fromUser.ConnectionId).sendPrivateMessage(toUser.UserId, toUser.FullName, message);

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

        private bool ifConnectionIdExist(string connectionId)
        {
            var count = _db.Connections.Count(u => u.ConnectionId == connectionId);
            if (count > 0)
                return true;
            return false;
        }
    }
}