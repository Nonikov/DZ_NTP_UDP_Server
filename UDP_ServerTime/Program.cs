using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NTP_Server;

namespace UDP_TimeServer
{
    public class UdpTimeServer
    {
        static string remoteAddress = "192.168.1.104";
        static int remotePort = 1000;
        static int localPort = 1001;

        static void SendTime()
        {
            using (UdpClient sender = new UdpClient())
            {
                for (; ; )
                {
                    //Get time from the NTP server
                    byte[] buffer = Encoding.ASCII.GetBytes(NTP.GetNetworkTime().ToString());
                    sender.Send(buffer, buffer.Length, remoteAddress, remotePort);
                }
            }
        }

        public static void ReceiveRequest()
        {
            using (UdpClient receiver = new UdpClient(localPort))
            {
                IPEndPoint remoteIp = null;

                for (; ; )
                {
                    byte[] buffer = receiver.Receive(ref remoteIp);
                    string message = Encoding.ASCII.GetString(buffer);
                    if (message == "time")
                    {
                        SendTime();
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The UDP_TimeServer is running");

            UdpTimeServer.ReceiveRequest();


            Console.ReadLine();
        }
    }
}
