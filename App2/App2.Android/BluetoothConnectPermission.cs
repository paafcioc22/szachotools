using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Xamarin.Essentials.Permissions;

namespace App2.Droid
{
    public class BluetoothConnectPermission : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
                {
                    return new List<(string androidPermission, bool isRuntime)>
                    {
                        (Android.Manifest.Permission.BluetoothScan, true),
                        (Android.Manifest.Permission.BluetoothConnect, true)
                    }.ToArray();
                }
                else
                {
                    return new List<(string androidPermission, bool isRuntime)>
                    {
                        (Android.Manifest.Permission.BluetoothAdmin, true),
                        (Android.Manifest.Permission.Bluetooth, true)
                    }.ToArray();
                }
            }
        }
    }
}