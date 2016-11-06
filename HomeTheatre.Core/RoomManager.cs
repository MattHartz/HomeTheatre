using HomeTheatre.DataAccess;
using HomeTheatre.Dto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace HomeTheatre.Core
{
    public interface IRoomManager
    {
        int CreateRoom(Room model);
        void DeleteRoom(string name);
        int JoinRoom(string userName, int room, string connectionId, string userAgent = null);
        int LeaveRoom(string connectionId);
        bool DoesRoomExist(int roomId);
        Connection GetConnection(string connectionId);
        List<Connection> GetConnectionsByRoom(int roomId);
    }

    public class RoomManager : IRoomManager
    {
        private readonly RoomContext _roomContext;
        private readonly ApplicationDbContext _userContext;

        public RoomManager(RoomContext rContext, ApplicationDbContext userContext)
        {
            _roomContext = rContext;
            _userContext = userContext;
        }

        public int CreateRoom(Room model)
        {
            _roomContext.Rooms.Add(model);
            _roomContext.SaveChanges();

            return _roomContext.Rooms.First(x => x.Name == model.Name).Id;
        }

        public void DeleteRoom(string name)
        {
            var room = _roomContext.Rooms.First(x => x.Name == name);

            _roomContext.Rooms.Remove(room);
            _roomContext.SaveChanges();
        }

        public int JoinRoom(string userName, int roomId, string connectionId, string userAgent = null)
        {
            // Validate that the user actually exists for safe keeping
            var user = _userContext.Users.First(x => x.UserName == userName);

            if (user == null)
            {
                throw new Exception("User does not exist.  wtf?");
            }

            // Create connection
            var connection = new Connection()
            {
                Connected = true,
                RoomId = roomId,
                UserName = user.UserName,
                UserAgent = userAgent,
                ConnectionId = connectionId,
                DateAdded = DateTime.Now
            };

            // Add connection
            _roomContext.Connections.Add(connection);
            _roomContext.SaveChanges();

            // Return count of users in room
            return _roomContext.Rooms.Count(x => x.Id == roomId);
        }

        public int LeaveRoom(string connectionId)
        {
            // Get Connection
            var connection = _roomContext.Connections.First(x => x.ConnectionId == connectionId);

            // Get room
            var roomId = connection.RoomId;

            // Close connection
            _roomContext.Connections.Remove(connection);
            _roomContext.SaveChanges();

            // Return the roomId
            return roomId;
        }

        public bool DoesRoomExist(int roomId)
        {
            return _roomContext.Rooms.Any(x => x.Id == roomId);
        }


        public Connection GetConnection(string connectionId)
        {
            var connection = _roomContext.Connections
                .First(x => x.ConnectionId == connectionId);

            return connection;
        }

        public List<Connection> GetConnectionsByRoom(int roomId)
        {
            var connections = _roomContext.Connections
                .Where(x => x.RoomId == roomId)
                .ToList();

            return connections;
        }

    }
}