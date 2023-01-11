using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

int port = 8000;
if (args.Length > 0) port = int.Parse(args[0]);

int BUFSIZE = 1500;
char[] buf = new char[BUFSIZE];

TcpListener tcp = new TcpListener(IPAddress.IPv6Any, port);
tcp.Server.DualMode = true;
tcp.Start();
Console.WriteLine($"TCP server started at port {port}");

while (true)
{
    TcpClient client = tcp.AcceptTcpClient();
    var remoteEP = client.Client.RemoteEndPoint;
    var streamReader = new StreamReader(client.GetStream(), Encoding.UTF8);
    var streamWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);

    {
        int size = streamReader.Read(buf, 0, BUFSIZE);
        if (size > 0)
        {
            string data = new String(buf, 0, size);
            Console.WriteLine($" Received from: {remoteEP} data: {data}");
            streamWriter.Write(data);
            streamWriter.Flush();
            Console.WriteLine(" Sent echo response");
        }
    }

    client.Close();
    Console.WriteLine("Closed client connection");
}
// warning due to the above while(true)
tcp.Stop();
Console.WriteLine("Closed TCP server");