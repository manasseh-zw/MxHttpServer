using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var ipAddress = IPAddress.Loopback;
        int port = 8081;

        var listener = new TcpListener(ipAddress, port);
        listener.Start();

        Console.WriteLine($"listening on http://localhost:{port}");
        Console.WriteLine("waiting for a connection");

        var client = listener.AcceptTcpClient();
        Console.WriteLine("client connected");

        var stream = client.GetStream();

        var buffer = new byte[4096];

        var bytesRead = stream.Read(buffer, 0, buffer.Length);

        var requestText = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        Console.WriteLine("----- RAW REQUEST -----");
        Console.WriteLine(requestText);
        Console.WriteLine("-----------------------");

        client.Close();
        listener.Stop();
    }
}
