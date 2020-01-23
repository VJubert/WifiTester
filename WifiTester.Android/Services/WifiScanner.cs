using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Runtime;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using WifiTester.Services;
using static WifiTester.Droid.Services.PermissionHelper;

namespace WifiTester.Droid.Services
{
    // From : https://developer.android.com/guide/topics/connectivity/wifi-scan

    public class WifiScanner : IWifiScanner
    {
        public WifiScanner()
        {
        }

        public async Task<List<WifiInformation>> ScanForWifi()
        {
            var appContext = Application.Context.ApplicationContext;
            ConnectivityManager manager = (ConnectivityManager)appContext.GetSystemService(Context.ConnectivityService);
            var wifiManager = (WifiManager)appContext.GetSystemService(Context.WifiService);

            var activity = CrossCurrentActivity.Current.Activity;

            //if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted &&
            //    ContextCompat.CheckSelfPermission(activity, Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted &&
            //    ContextCompat.CheckSelfPermission(activity, Manifest.Permission.AccessWifiState) != Android.Content.PM.Permission.Granted &&
            //    ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ChangeWifiState) != Android.Content.PM.Permission.Granted)
            //{
            var permissions = PermissionContainer.WaitForPermission(9002);

            ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessWifiState, Manifest.Permission.ChangeWifiState }, 9002);

            var result = await permissions;

            Console.WriteLine($"Permission request result : {result}");
            //}

            var tcs = new TaskCompletionSource<List<WifiInformation>>();

            activity.RegisterReceiver(new WifiReceiver(tcs), new IntentFilter(WifiManager.ScanResultsAvailableAction));

            var ret = wifiManager.StartScan();

            Console.WriteLine($"Start scan results : {ret}");

            return await tcs.Task;
        }

        public class WifiReceiver : BroadcastReceiver
        {
            private TaskCompletionSource<List<WifiInformation>> _tcs;

            public WifiReceiver(TaskCompletionSource<List<WifiInformation>> tcs)
            {
                _tcs = tcs;
            }

            protected WifiReceiver(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
            {
            }

            public override void OnReceive(Context context, Intent intent)
            {
                var wifi = context.GetSystemService(Context.WifiService) as WifiManager;

                var results = wifi.ScanResults;

                _tcs.SetResult(results.Select(x => new WifiInformation
                {
                    Name = x.Ssid
                }).ToList());

                CrossCurrentActivity.Current.Activity.UnregisterReceiver(this);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _tcs = null;
                }
                base.Dispose(disposing);
            }
        }
    }
}
