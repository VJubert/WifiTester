using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreFoundation;
using NetworkExtension;
using WifiTester.Services;

namespace WifiTester.iOS.Services
{
    public class WifiScanner : IWifiScanner
    {
        public WifiScanner()
        {
        }

        public Task<List<WifiInformation>> ScanForWifi()
        {
            var res = NEHotspotHelper.Register(null, DispatchQueue.DefaultGlobalQueue, x =>
               {
                   Console.WriteLine("handling");

                   foreach (var net in x.NetworkList)
                   {
                       Console.WriteLine(net);
                   }
               });

            Console.WriteLine($"Results {res}");

            return Task.FromResult((List<WifiInformation>)null);
        }
    }
}
