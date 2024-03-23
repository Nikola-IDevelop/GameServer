using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class Server
    {
        private TcpListener listener;

        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            listener = new TcpListener(ip, port);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Server started. Listening for incoming connections...");

            // Start accepting client connections asynchronously
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                // Create a new thread to handle client communication
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    // Convert the received bytes to a string
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + dataReceived);

                    // Echo the message back to the client
                    byte[] dataToSend = Encoding.UTF8.GetBytes(dataReceived);
                    stream.Write(dataToSend, 0, dataToSend.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Clean up resources
                stream.Close();
                client.Close();
                Console.WriteLine("Client disconnected.");
            }
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}
