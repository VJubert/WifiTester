using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WifiTester.Services
{
    public class WifiInformation
    {
        public string Name { get; set; }

        public override string ToString() => $"Name = {Name}";
    }

    public interface IWifiScanner
    {
        Task<List<WifiInformation>> ScanForWifi();
    }
}
