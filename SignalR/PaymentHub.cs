using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Razor.Data;
using Razor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Razor.SignalR
{
    public class PaymentHub : Hub
    { 
        public AppDbContext AppDbContext {get;set;}
        public PaymentHub(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }
        private readonly static ConnectionMappings<string> _connection = new ConnectionMappings<string>();

        public async Task SendNotification(string username)
        {
            Notification notification = new Notification()
            {
                sender_id = int.Parse(username),
                receiver_id = 0,
                role_id = 1,
                created_at = DateTime.Now
            };
            AppDbContext.Notification.Add(notification);
            AppDbContext.SaveChanges();
            await Clients.All.SendAsync("GotMessage2",username);
        }
        public async Task SendConfirmation(string username,string receiver)
        {
            Notification notification = new Notification()
            {
                sender_id = int.Parse(username),
                receiver_id = int.Parse(receiver),
                role_id = 2,
                created_at = DateTime.Now
            };
            AppDbContext.Notification.Add(notification);
            AppDbContext.SaveChanges();
            await Clients.All.SendAsync("GotMessage2",username);
        }
    }

     public class ConnectionMappings<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connection =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connection.Count;
            }
        }
        public void Add(T key, string connectionId)
        {
            lock (_connection)
            {
                HashSet<string> connections;
                if (!_connection.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connection.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connection.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connection)
            {
                HashSet<string> connections;
                if (!_connection.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connection.Remove(key);
                    }
                }
            }
        }
    }
}