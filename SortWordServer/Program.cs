using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SortWordServer;

class SortWordServer
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

            var words = data.Split(' ');
            Array.Sort(words);

            var set = new HashSet<string>(words);
            words = set.ToArray();

            string wordsString = string.Join(" ", words);
            wordsString += "\r\n";

            byte[] wordsBytes = Encoding.UTF8.GetBytes(wordsString);

            stream.Write(wordsBytes, 0, wordsBytes.Length);
        }

        tcpClient.Close();
    }
}