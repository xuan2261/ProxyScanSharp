using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using System.Net;
using System.Net.Sockets;

namespace ProxyScanSharp
{
    static class Program
    {
        private static readonly ReaderWriterLock LogLock = new();

        static void WriteProxy(string filename, string proxy)
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
        
        static bool PortCheck(string proxy, int port, int timeout)
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
        static async Task<bool> CheckProxy(string proxy, int port, int timeout, string url)
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

        static async Task CheckThread(string[] args)
        {
            while (true)
            {
                try
                {
                    var randomizer = RandomizerFactory.GetRandomizer(new FieldOptionsIPv4Address());
                    var ip = randomizer.Generate();

                    if (!string.IsNullOrEmpty(ip))
                    {
                        foreach(string port in args[0].Split(','))
                        {
                            if (PortCheck(ip, Convert.ToInt32(port), Convert.ToInt32(args[3])))
                            {
                                if (await CheckProxy(ip, Convert.ToInt32(port), Convert.ToInt32(args[4]), args[5]))
                                {
                                    WriteProxy(args[2], ip + ":" + port);
                                    break;
                                }
                            }
                        }
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
            }
        }
        static async Task MainAsync(string[] args)
        {
            await Task.Run(() =>
            {
                List<Task> Threads = new();

                for (int i = 0; i < Convert.ToInt32(args[1]); i++)
                {
                    Console.WriteLine("Started Thread: " + args[1]);
                    Threads.Add(CheckThread(args));
                }

                Task.WaitAll(Threads.ToArray());
            });
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid amount of threads");
                return;
            }
            if (args.Length != 6)
            {
                Console.WriteLine("Invalid amount of arguments <ports> <threads> <filename> <port_timeout_ms> <proxy_timeout_seconds> <url_to_check>");
                return;
            }
            MainAsync(args).Wait();
            Console.ReadLine();
        }
    }
}
