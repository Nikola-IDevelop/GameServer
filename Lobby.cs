using System.Net.Sockets;

namespace GameServer
{
    public class Lobby
    {
        public string LobbyId { get; private set; }
        public List<TcpClient> Clients { get; private set; }

        public Lobby(string lobbyId)
        {
            LobbyId = lobbyId;
            Clients = new List<TcpClient>();
        }

        public void AddClient(TcpClient client)
        {
            Clients.Add(client);
        }

        public void RemoveClient(TcpClient client)
        {
            Clients.Remove(client);
        }
    }
}
