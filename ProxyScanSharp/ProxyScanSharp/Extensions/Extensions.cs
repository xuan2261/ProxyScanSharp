using System.Net;
using System.Net.Sockets;

namespace ProxyScanSharp.Extensions
{
    public static class Extensions
    {
        public static bool CheckIPValid(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }

        private static readonly ReaderWriterLock LogLock = new();

        public static void WriteProxy(string filename, string proxy)
        {
            try
            {
                lock (LogLock)
                {
                    LogLock.AcquireReaderLock(int.MaxValue);

                    using var stream = new StreamWriter(filename, true);
                    stream.WriteLine(proxy);
                }
            }
            finally
            {
                LogLock.ReleaseReaderLock();
            }
        }

        public static bool PortCheck(string proxy, int port, int timeout)
        {
            try
            {
                using TcpClient tcpClient = new();
                if (tcpClient.ConnectAsync(proxy, port).Wait(TimeSpan.FromMilliseconds(timeout)))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Port Open] " + proxy + ":" + port);
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Port Closed] " + proxy + ":" + port);
                    return false;
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Port Closed] " + proxy + ":" + port);
                return false;
            }
        }

        public static async Task<bool> CheckProxy(string proxy, int port, int timeout, string url)
        {
            try
            {
                using HttpClient HttpClient = new(new HttpClientHandler { Proxy = new WebProxy(proxy, port) });
                HttpClient.Timeout = TimeSpan.FromSeconds(timeout);
                await HttpClient.GetAsync(url);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Valid] " + proxy + ":" + port);
                return true;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Invalid] " + proxy + ":" + port);
                return false;
            }
        }
    }
}
