using GroupChat.Net.IO;
using System.Net.Sockets;

namespace GroupChat.Server.Model
{
    public class Client
    {
        public string? Username { get; set; }
        public string? UID { get; set; }
        public TcpClient? ClientSocket { get; set; }
        PacketReader packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = packetReader.ReadOpCode();
            UID = packetReader.ReadMessage();
            Username = packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with this information:\nUID: {UID}\nUsername: {Username}\n");
            Process();
        }

        private void Process()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var opcode = packetReader.ReadOpCode();
                        switch (opcode)
                        {
                            case 5:
                                var msg = packetReader.ReadMessage();
                                Console.WriteLine($"[{DateTime.Now}]: Message recived! {msg}\n");
                                Program.BroadcastMessage(msg);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"UID \"{UID}\" has disconnected!\n");
                        Program.BroadcastDisConnection(UID!);
                        ClientSocket?.Close();
                        break;
                    }
                }
            });
        }
    }
}