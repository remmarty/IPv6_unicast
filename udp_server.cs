using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

int port = 8000;

if (args.Length > 0) port = int.Parse(args[0]);

Console.WriteLine("UDP receiver - echo server");
UdpClient udpServer = new UdpClient(AddressFamily.InterNetworkV6);
udpServer.Client.DualMode = true;
udpServer.Client.Bind(new IPEndPoint(IPAddress.IPv6Any, port)); 
Console.WriteLine($"UDP receiver started at port {port}");

while (true) {
    IPEndPoint remoteEP = new IPEndPoint(IPAddress.IPv6Any, 0);
    byte[] buffer = udpServer.Receive(ref remoteEP);
    string data = Encoding.UTF8.GetString(buffer);
    Console.WriteLine($" Received from: {remoteEP} data: {data}");
    udpServer.Send(buffer, buffer.Length, remoteEP);  
    Console.WriteLine($" Sent echo response to: {remoteEP}");
}
udpServer.Close();
Console.WriteLine("Close UDP socket");
