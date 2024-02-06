using GroupChat.Net.IO;
using GroupChat.Server.Model;
using System.Net;
using System.Net.Sockets;

namespace GroupChat.Server
{
    public class Program
    {
        static List<Client> clients = new List<Client>();
        static TcpListener? _listener;

        static void Main(string[] args)
        {
            _listener = new TcpListener(IPAddress.Parse("0.0.0.0"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                clients.Add(client);

                BroadcastConnection();
            }
        }

        private static void BroadcastConnection()
        {
            clients.ForEach(client =>
            {
                clients.ForEach(c =>
                {
                    PacketBuilder broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(c.UID!);
                    broadcastPacket.WriteMessage(c.Username!);
                    client.ClientSocket!.Client.Send(broadcastPacket.GetBytes());
                });
            });
        }

        public static void BroadcastDisConnection(string uid)
        {
            Client client = clients.Where(c => c.UID == uid).FirstOrDefault()!;
            clients.Remove(client);
            clients.ForEach(c =>
            {
                PacketBuilder packetBuilder = new PacketBuilder();
                packetBuilder.WriteOpCode(10);
                packetBuilder.WriteMessage(uid);
                c.ClientSocket?.Client.Send(packetBuilder.GetBytes());
            });
            BroadcastMessage($"User \"{uid}\" disconnected!");
        }

        public static void BroadcastMessage(string message)
        {
            clients.ForEach(c =>
            {
                PacketBuilder packetBuilder = new PacketBuilder();
                packetBuilder.WriteOpCode(5);
                packetBuilder.WriteMessage(message);
                c.ClientSocket?.Client.Send(packetBuilder.GetBytes());
            });
        }
    }
}