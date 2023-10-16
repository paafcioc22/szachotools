
using Android.Bluetooth;
//using BlueToothPrinter.DependencyServices;
using App2.Droid;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2.Model;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidBlueToothService))]

namespace App2.Droid
{
    public class AndroidBlueToothService : IBluetoothPermissionService
    {
        public async Task<bool> CheckAndRequestBluetoothPermissionAsync()
        {
            // Tutaj umieść logikę sprawdzania i żądania uprawnień
            var status = await Permissions.CheckStatusAsync<BluetoothConnectPermission>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<BluetoothConnectPermission>();
            }

            return status == PermissionStatus.Granted;
        }
    }
}