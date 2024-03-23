using System.Net.Sockets;
using System.Net;
using System.Text;
using GameServer.Enums;

namespace GameServer
{
    public class ChatServer
    {
        static Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        string serverIp = "127.0.0.1"; // Change this to your desired IP address
        int serverPort = 3133; // Change this to your desired port number

        // Start the server
        TcpListener server;

        public ChatServer()
        {
            server = new TcpListener(IPAddress.Parse(serverIp), serverPort);
        }
        public void StartServer()
        {
            server.Start();
            Console.WriteLine("Server started. Listening for incoming connections fc  
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                // Start a new thread to handle each client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }
        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            string clientId = Guid.NewGuid().ToString(); // Generate a unique ID for the client
            clients.Add(clientId, client);

            NetworkStream stream = client.GetStream();

            try
            {
                while (true)
                {
                    // Read data from the client
                    byte[] data = new byte[1024];
                    int bytesRead = stream.Read(data, 0, data.Length);
                    string message = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Console.WriteLine("Received from client " + clientId + ": " + message);

                    // Broadcast the message to all other clients
                    BroadcastMessage(message, clientId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client " + clientId + " disconnected: " + ex.Message);
                clients.Remove(clientId);
                client.Close();
            }
        }

        static void BroadcastMessage(string message, string senderId)
        {
            foreach (var clientId in clients.Keys)
            {
                if (clientId != senderId)
                {
                    TcpClient client = clients[clientId];
                    NetworkStream stream = client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }

                if (clientId == senderId)
                {
                    TcpClient client = clients[clientId];
                    NetworkStream stream = client.GetStream();
                    byte[] data = { (byte)MessageStatus.Success };
                    stream.Write(data, 0, data.Length);
                }
            }
        }
    }
}
