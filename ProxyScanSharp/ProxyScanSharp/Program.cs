using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using System.Net;
using System.Net.Sockets;

namespace ProxyScanSharp
{
    static class Program
    {
        private static readonly ReaderWriterLock LogLock = new();

        static void WriteProxy(string proxy)
        {
            try
            {
                lock (LogLock)
                {
                    LogLock.AcquireReaderLock(int.MaxValue);

                    using (var stream = new StreamWriter("Valid.txt", true))
                    {
                        stream.WriteLine(proxy);
                    }
                }
            }
            finally
            {
                LogLock.ReleaseReaderLock();
            }
        }
        
        static async Task<bool> PortCheck(string proxy, int port)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    if (tcpClient.ConnectAsync(proxy, port).Wait(TimeSpan.FromMilliseconds(200)))
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Port Open] " + proxy + ":" + port);
                    return true;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Port Closed] " + proxy + ":" + port);
                    return false;
                }
            }
        }
        static async Task<bool> CheckProxy(string proxy, int port)
        {
            using (HttpClient HttpClient = new HttpClient(new HttpClientHandler { Proxy = new WebProxy(proxy, port)}))
            {
                try
                {
                    HttpClient.Timeout = TimeSpan.FromSeconds(2);
                    await HttpClient.GetAsync("https://ipv4.icanhazip.com");
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

        static async Task CheckThread(string[] args)
        {
            while (true)
            {
                var randomizer = RandomizerFactory.GetRandomizer(new FieldOptionsIPv4Address());
                var proxy = randomizer.Generate();
                try
                {
                    if (proxy != null)
                    {
                        if (await PortCheck(proxy, Convert.ToInt32(args[0])))
                        {
                            if (await CheckProxy(proxy, Convert.ToInt32(args[0])))
                            {
                                WriteProxy(proxy + ":" + Convert.ToInt32(args[0]));
                            }
                        }
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
                catch (Exception ex)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
            }
        }
        static async Task MainAsync(string[] args)
        {
            await Task.Run(() =>
            {
                List<Task> Threads = new List<Task>();

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
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid amount of threads");
                return;
            }
            MainAsync(args).Wait();
            Console.ReadLine();
        }
    }
}
