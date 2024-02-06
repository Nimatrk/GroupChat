using GroupChat.Net.IO;
using GroupChat.UI.MVVM.Model;
using System.Net.Sockets;

namespace GroupChat.UI.Net
{
    public class Server
    {
        TcpClient? _client;
        public PacketReader PacketReader { get; set; }
        public event Action ConnectedEvent;
        public event Action DisconnectedEvent;
        public event Action ReciveMessageEvent;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(User user)
        {
            if (!_client!.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrWhiteSpace(user.Username))
                {
                    PacketBuilder packetBuilder = new PacketBuilder();
                    packetBuilder.WriteOpCode(0);
                    packetBuilder.WriteMessage(user.UID!);
                    packetBuilder.WriteMessage(user.Username!);
                    _client.Client.Send(packetBuilder.GetBytes());
                }

                ProcessPackets();
            }
        }

        public void SendMessageToServer(string message)
        {
            if (_client!.Connected)
            {
                PacketBuilder packetBuilder = new PacketBuilder();
                packetBuilder.WriteOpCode(5);
                packetBuilder.WriteMessage(message);
                _client.Client.Send(packetBuilder.GetBytes());
            }
        }

        private void ProcessPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadOpCode();
                    switch (opcode)
                    {
                        case 1:
                            ConnectedEvent?.Invoke(); break;
                        case 5:
                            ReciveMessageEvent?.Invoke(); break;
                        case 10:
                            DisconnectedEvent?.Invoke(); break;
                    }
                }
            });
        }
    }
}