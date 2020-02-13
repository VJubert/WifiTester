using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreFoundation;
using CoreLocation;
using NetworkExtension;
using SystemConfiguration;
using WifiTester.Services;
using Xamarin.Essentials;

namespace WifiTester.iOS.Services
{
    // ios need require to apple :
    // https://developer.apple.com/contact/request/hotspot-helper/
    // https://stackoverflow.com/questions/9684341/iphone-get-a-list-of-all-ssids-without-private-library/52473480#52473480
    // https://developer.apple.com/documentation/networkextension/nehotspothelper/1618965-register
    // after that you can get network list when user goes to the wifi system page
    // you can ask to connect to a network if you have password and ssid
    // futhermore, you can not open general settings of device, only applications settings

    public class WifiScanner : IWifiScanner
    {
        public WifiScanner() { }

        public Task<List<WifiInformation>> ScanForWifi()
        {
            var queue = new DispatchQueue("network queue");

            var res = NEHotspotHelper.Register(null, queue, x =>
               {
                   Console.WriteLine("handling");

                   foreach (var net in x.NetworkList)
                   {
                       Console.WriteLine(net);
                   }
               });

            Console.WriteLine($"Return {res}");

            return Task.FromResult((List<WifiInformation>)null);
        }

        public async Task ShowCurrentWifiInformation()
        {
            await Geolocation.GetLocationAsync();

            //return null pour des simulateurs
            var result = CaptiveNetwork.TryGetSupportedInterfaces(out var supportedInterfaces);

            if(result == StatusCode.OK && supportedInterfaces != null)
            {
                foreach (var r in supportedInterfaces)
                {
                    Console.WriteLine(r);

                    var netResult = CaptiveNetwork.TryCopyCurrentNetworkInfo(r, out var networkInfo);
                    if(netResult == StatusCode.OK && networkInfo!=null)
                    {
                        foreach(var kvp in networkInfo)
                        {
                            Console.WriteLine($"{kvp.Key} {kvp.Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine(netResult);
                    }
                }
            } else
            {
                Console.WriteLine(result);
            }
        }
    }
}
