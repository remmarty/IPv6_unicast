using System.Net;
using System.Net.Sockets;
using System.Text;


string host = "localhost";
int port = 8000;
string msg = "Test message " + DateTime.Now.ToString();

if (args.Length > 0) host = args[0];
if (args.Length > 1) port = int.Parse(args[1]);
if (args.Length > 2) msg = args[2];

Console.WriteLine("UDP sender");
IPAddress[] addr = Dns.GetHostAddresses(host);
var remoteEP = new IPEndPoint(addr[0], port);
var udpClient = new UdpClient(addr[0].AddressFamily);
udpClient.Client.ReceiveTimeout = 3000;

try {
    udpClient.Connect(remoteEP);
    byte[] buf = Encoding.UTF8.GetBytes(msg);
    udpClient.Send(buf, buf.Length);
    Console.WriteLine($"Sent to: {remoteEP} data: {msg}");
    byte[] bufin = udpClient.Receive(ref remoteEP);
    if (bufin.Length > 0) {
        string response = Encoding.UTF8.GetString(bufin);
        Console.WriteLine($"Response from: {remoteEP} data: {response}");
    }
}

catch {
    Console.WriteLine("No response");
}
udpClient.Close();
Console.WriteLine("Closed UDP socket");