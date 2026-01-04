using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static async Task Main(string[] args)
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

        var body = """
            <html>
                <body>
                    <h1>Hello form mxHttp server! </h1>
                </body>
            </html>
            """;

        var bodyBytes = Encoding.UTF8.GetBytes(body);

        var responseHeaders =
            "HTTP/1.1 200 OK\r\n"
            + "Content-Type: text/html; charset=utf-8\r\n"
            + $"Content-Length: {bodyBytes.Length}\r\n"
            + "Connection: close\r\n"
            + "\r\n";

        var headerBytes = Encoding.UTF8.GetBytes(responseHeaders);

        await stream.WriteAsync(headerBytes);
        await stream.WriteAsync(bodyBytes);

        client.Close();
    }
}
