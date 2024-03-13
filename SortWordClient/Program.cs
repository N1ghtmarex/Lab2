using System.Net.Sockets;

namespace Client;

class SortWordClient
{
    public static void Main(string[] args)
    {
        TcpClient tcpClient = new TcpClient("185.221.162.111", 13000);

        Stream s = tcpClient.GetStream();
        StreamReader sr = new StreamReader(s);
        StreamWriter sw = new StreamWriter(s);
        sw.AutoFlush = true;

        while (true)
        {
            Console.Write("Request:  ");
            string? request = Console.ReadLine();

            sw.WriteLine(request);

            if (request == "stop")
            {
                s.Close();
                break;
            }

            Console.WriteLine("Response: " + sr.ReadLine());
        }

        tcpClient.Close();
    }
}