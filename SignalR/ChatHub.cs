using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Razor.Data;
using Razor.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Razor.SignalR
{
    public class ChatHub : Hub
    {
        public AppDbContext AppDbContext {get;set;}

        private readonly static ConnectionMapping<string> _connections = 
            new ConnectionMapping<string>();

        public ChatHub(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }
        public async Task SendMessage(string sender,string receiver,string message,string connectionId)
        {   
            Chat chat = new Chat
            {
                sender_id = int.Parse(sender),
                receiver_id = int.Parse(receiver),
                message = message,
                created_at = DateTime.Now,
            };
            AppDbContext.Chat.Add(chat);
            _connections.Add(sender, connectionId);
            await Clients.Client(connectionId).SendAsync("GotMessage",sender,receiver,message);
            foreach (var conId in _connections.GetConnections(receiver))
            {
                await Clients.Client(conId).SendAsync("GotMessage",sender,receiver,message);
            }
        }

        public string AddId(string sender)
        {
            string name = sender;
            _connections.Add(name, Context.ConnectionId);

            return name;
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            var value = await Task.FromResult(0);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }

    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
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
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}