using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BoardServer;

class BoardServer
{
    public static void Main(string[] args)
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 13371);
        tcpListener.Start();

        TcpClient tcpClient = tcpListener.AcceptTcpClient();

        NetworkStream stream = tcpClient.GetStream();

        byte[] bytes = new byte[256];
        while (true)
        {
            int i = stream.Read(bytes, 0, bytes.Length);

            var data = Encoding.UTF8.GetString(bytes, 0, i);
            data = data.Replace("\r\n", "");

            if (data == "stop")
            {
                break;
            }

            if (data == "LIST")
            {
                using FileStream fstream = File.OpenRead("board.txt");
                byte[] buffer = new byte[fstream.Length];

                fstream.Read(buffer, 0, buffer.Length);

                var text = Encoding.UTF8.GetString(buffer) + "\r\n";

                buffer = Encoding.UTF8.GetBytes(text);
                stream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                using FileStream fstream = new FileStream("board.txt", FileMode.Append, FileAccess.Write);

                byte[] buffer = Encoding.UTF8.GetBytes(data + ";");

                fstream.Write(buffer, 0, buffer.Length);

                var response = Encoding.UTF8.GetBytes($"Message added: \"{data}\"\r\n");
                stream.Write(response, 0, response.Length);
            }
        }

        tcpClient.Close();
    }
}