using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

var tcpClient = new TcpClient(AddressFamily.InterNetworkV6);
string host = "fe80::2ddf:912d:95fc:702d%13";
int port = 8000;
string msg = "Hello there!";
int BUFSIZE = 10000;
char[] buf = new char[BUFSIZE];

if (args.Length > 0) host = args[0];
if (args.Length > 1) port = int.Parse(args[1]);
if (args.Length > 2) msg = args[2];

IPAddress[] addr = Dns.GetHostAddresses(host);
TcpClient tcp = new TcpClient(addr[0].AddressFamily);
tcp.Client.ReceiveTimeout = 3000;
tcp.Connect(addr[0], port);
Console.WriteLine($"Connected to: {addr[0]}");

var streamWriter = new StreamWriter(tcp.GetStream(), Encoding.ASCII);
var streamReader = new StreamReader(tcp.GetStream(), Encoding.UTF8);
streamWriter.Write(msg);
streamWriter.Flush();
Console.WriteLine($"Sent to: {addr[0]} data: {msg} {msg.Length}");
Thread.Sleep(100);

int len = streamReader.Read(buf, 0, BUFSIZE);
Console.WriteLine($"Received response length: {len}");

string response = new string(buf, 0, len);
Console.WriteLine(response);

tcp.Close();

Console.WriteLine("Closed connection");