using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;

namespace WifiTester.Droid.Services
{
    public class PermissionHelper
    {
        public interface IPermissionHandlingActivity
        {
            Task<Permission[]> WaitForPermission(int code);
        }

        internal static class PermissionContainer
        {
            public static readonly Dictionary<int, TaskCompletionSource<Permission[]>> _permissions = new Dictionary<int, TaskCompletionSource<Permission[]>>();

            public static Task<Permission[]> WaitForPermission(int code)
            {

                TaskCompletionSource<Permission[]> result = new TaskCompletionSource<Permission[]>();

                if (_permissions.TryAdd(code, result))
                {
                    return result.Task;
                }

                if (_permissions.TryGetValue(code, out result))
                {
                    return result.Task;
                }

                return null;
            }

            public static void OnResult(int requestCode, Permission[] grantResults)
            {
                if (_permissions.TryGetValue(requestCode, out var result))
                {
                    result.TrySetResult(grantResults);

                    _permissions.Remove(requestCode);
                }
            }
        }
    }
}
