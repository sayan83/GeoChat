using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace GeoChat.ChatAPI;

public class ConnectionListStore
{
    public ConcurrentDictionary<string, WebSocket> ActiveConnections {get; set;}
    public static ConnectionListStore activeConnections = new ConnectionListStore();
    public ConnectionListStore() {
        ActiveConnections = new ConcurrentDictionary<string, WebSocket>();
    }

    public IEnumerable<string> GetActiveUsers() {
        return ActiveConnections.Keys.ToList();
    }
}
