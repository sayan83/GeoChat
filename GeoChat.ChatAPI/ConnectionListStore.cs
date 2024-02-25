using System.Net.WebSockets;

namespace GeoChat.ChatAPI;

public class ConnectionListStore
{
    public Dictionary<string, WebSocket> ActiveConnections {get; set;}
    public static ConnectionListStore activeConnections = new ConnectionListStore();
    public ConnectionListStore() {
        ActiveConnections = new Dictionary<string, WebSocket>();
    }

    public IEnumerable<string> GetActiveUsers() {
        return ActiveConnections.Keys.ToList();
    }
}
