using System.Text;

namespace GroupChat.Net.IO
{
    public class PacketBuilder
    {
        MemoryStream _memoryStream;

        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _memoryStream.WriteByte(opcode);
        }

        public void WriteMessage(string message)
        {
            _memoryStream.Write(BitConverter.GetBytes(message.Length));
            _memoryStream.Write(Encoding.ASCII.GetBytes(message));
        }

        public byte[] GetBytes()
        {
            return _memoryStream.ToArray();
        }
    }
}