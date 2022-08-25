namespace ProxyScanSharp
{
    public static class Program
    {
        static async Task MainAsync(string[] args)
        {
            await Task.Run(() =>
            {
                List<Task> Threads = new();

                for (int i = 0; i < Convert.ToInt32(args[1]); i++)
                {
                    Console.WriteLine("Started Thread: " + i);
                    Task thread = Task.Run(() => Threading.Tasks.CheckThread(args));
                    Threads.Add(thread);
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
