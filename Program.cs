namespace GameServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string ipAddress = "127.0.0.1"; // Change this to your desired IP address
            //int port = 1337; // Change this to your desired port number

            //Server server = new Server(ipAddress, port);
            //server.Start();

            ChatServer chatServer = new ChatServer();
            chatServer.StartServer();
        }
    }
}
