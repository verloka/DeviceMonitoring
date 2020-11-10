using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Threading;

namespace DeviceMonitoring
{
    class Program
    {
        static Timer Timer;
        static int Delay;
        static Dictionary<long, short> Timeline;
        static Dictionary<int, string> Devices;
        static int DeviceNumber;
        static int BatchSize;
        static string FileName;

        static void Main(string[] args)
        {
            Delay = args.GetIntArgument("Delay", 100, "-d");
            BatchSize = args.GetIntArgument("Batch size", 100, "-b");
            FileName = args.GetStringArgument("Output name", string.Empty, "-f");

            Console.WriteLine("Load devices...");

            Devices = LoadDeviceList();

            Console.Write("\nSelect the device: ");
            var str = Console.ReadLine();

            if (int.TryParse(str, out DeviceNumber) && Devices.ContainsKey(DeviceNumber))
            {
                Console.Clear();
                Console.WriteLine($"Selected device: {Devices[DeviceNumber]}");

                if(string.IsNullOrWhiteSpace(FileName))
                    FileName = Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), $"data[{DateTime.Now.ToShortDateString()}].csv");
                
                Console.WriteLine($"Data will be save in the file: {FileName}\n");

                Timeline = new Dictionary<long, short>();
                Timer = new Timer(TimerElapsed);
                Timer.Change(Delay, Timeout.Infinite);
            }
            else
            {
                Console.WriteLine("Device not found...");
            }

            Console.WriteLine(Timer == null ? "Press any key to exit..." : "Monitoring, press any key to exit...");
            Console.Read();
        }

        static Dictionary<int, string> LoadDeviceList()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            int i = 1;

            ManagementObjectSearcher deviceList = new ManagementObjectSearcher("Select Name from Win32_PnPEntity");
            if (deviceList != null)
            {
                foreach (ManagementObject device in deviceList.Get())
                    try
                    {
                        string name = device.GetPropertyValue("Name").ToString();
                        Console.WriteLine($"{i}. Device: {name}");
                        dic.Add(i, name);
                        i++;
                    }
                    catch { }

                deviceList.Dispose();
            }

            return dic;
        }

        static bool Isworking(string Name)
        {
            ManagementObjectSearcher deviceList = new ManagementObjectSearcher($"Select Status from Win32_PnPEntity where Name='{Name}'");
            if (deviceList != null)
            {
                foreach (ManagementObject device in deviceList.Get())
                    try
                    {
                        string status = device.GetPropertyValue("Status").ToString();
                        bool working = ((status == "OK") || (status == "Degraded") || (status == "Pred Fail"));
                        return working;
                    }
                    catch { }

                deviceList.Dispose();
            }

            return false;
        }

        static void TimerElapsed(object state)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);

            Timeline.Add(DateTime.Now.Ticks, (short)(Isworking(Devices[DeviceNumber]) ? 0 : 1));

            if(Timeline.Count >= BatchSize)
            {
                using StreamWriter sw = new StreamWriter(File.OpenWrite(FileName));
                foreach (var line in Timeline)
                    sw.WriteLine($"{new DateTime(line.Key):o};{line.Value}");
            }

            Timer.Change(Delay, Timeout.Infinite);
        }
    }
}
