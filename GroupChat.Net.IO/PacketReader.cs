using System.Net.Sockets;
using System.Text;

namespace GroupChat.Net.IO
{
    public class PacketReader : BinaryReader
    {
        NetworkStream _networkStream;
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
        }

        public int ReadOpCode()
        {
            return _networkStream.ReadByte();
        }

        public string ReadMessage()
        {
            byte[] bytes = new byte[ReadInt32()];
            _networkStream.Read(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}