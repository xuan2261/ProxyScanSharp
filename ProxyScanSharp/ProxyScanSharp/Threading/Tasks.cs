using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace ProxyScanSharp.Threading
{
    public class Tasks
    {
        public static async Task CheckThread(string[] args)
        {
            while (true)
            {
                try
                {
                    var randomizer = RandomizerFactory.GetRandomizer(new FieldOptionsIPv4Address());
                    var ip = randomizer.Generate();

                    if (!string.IsNullOrEmpty(ip))
                    {
                        if (Extensions.Extensions.CheckIPValid(ip))
                        {
                            foreach (string port in args[0].Split(','))
                            {
                                if (Extensions.Extensions.PortCheck(ip, Convert.ToInt32(port), Convert.ToInt32(args[3])))
                                {
                                    if (await Extensions.Extensions.CheckProxy(ip, Convert.ToInt32(port), Convert.ToInt32(args[4]), args[5]))
                                    {
                                        Extensions.Extensions.WriteProxy(args[2], ip + ":" + port);
                                        break;
                                    }
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
    }
}
