using System;
using System.Collections.Generic;
using System.Linq;
using HomeTheatre.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using HomeTheatre.Dto.Models;

namespace HomeTheatre.CompositionRoot.Hubs
{
    [Authorize]
    public class RoomHub : Hub
    {
        private readonly IRoomManager _roomManager;

        public RoomHub(IRoomManager manager)
        {
            _roomManager = manager;
        }

        [HubMethodName("JoinRoom")]
        public void JoinRoom(int roomId)
        {
            var name = Context.User.Identity.Name;
            var userAgent = Context.Request.Headers["User-Agent"];
            var connection = Context.ConnectionId;

            // Joins the room
            _roomManager.JoinRoom(name, roomId, connection, userAgent);

            // Get the connections
            var connections = _roomManager.GetConnectionsByRoom(roomId);
                //.Where(x => x.ConnectionId != connection);

            // Get unique list of users
            var users = new HashSet<string>(connections.Select(x => x.UserName));            

            // Broadcasts the users to the clients
            foreach (var conn in connections)
            {
                if (name != conn.UserName)
                {
                    Clients.Client(conn.ConnectionId).broadcastMessage($"{name} has joined the room");
                }
                Clients.Client(conn.ConnectionId).broadcastUsers(users.Select(x => x));
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var name = Context.User.Identity.Name;
            var room = _roomManager.LeaveRoom(Context.ConnectionId);

            // Get the connections
            var connections = _roomManager.GetConnectionsByRoom(room);

            // Get unique list of users
            var users = new HashSet<string>(connections.Select(x => x.UserName));

            // Broadcasts the users to the clients
            foreach (var conn in connections)
            {
                Clients.Client(conn.ConnectionId).broadcastMessage($"{name} has left the room");
                Clients.Client(conn.ConnectionId).broadcastUsers(users.Select(x => x));
            }

            return base.OnDisconnected(stopCalled);
        }

        public void Send(string message)
        {
            // Get connection ID
            var connectionId = Context.ConnectionId;
            
            // Using connection ID get room and rooms users
            var connection = _roomManager.GetConnection(connectionId);

            // Get connections by room ID
            var connections = _roomManager.GetConnectionsByRoom(connection.RoomId);

            // Broadcast
            foreach (var conn in connections)
            {
                Clients.Client(conn.ConnectionId).broadcastMessage(Context.User.Identity.Name, message);
            }
        }
    }
}