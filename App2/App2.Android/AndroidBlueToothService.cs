
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

[assembly: Xamarin.Forms.Dependency(typeof(AndroidBlueToothService))]

namespace App2.Droid
{
    class AndroidBlueToothService : IBlueToothService
    {
        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// BondedDevices returns BluetoothDevice collection anyway I need to take just device name.
        /// </summary>
        /// <returns></returns>
        public bool isBluetoothEnabled()
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
             
            return mBluetoothAdapter.IsEnabled;

        }

        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// We need to find Bluetooth Device with selected device name.
        /// Now, we use BluetoothSocket class with most common UUID
        /// Try to connect BluetoothSocket then convert your text-message to bytearray
        /// Last step write your bytearray by way of bluetoothSocket
        /// </summary>
        /// <param name="deviceName">Selected deviceName</param>
        /// <param name="text">My printed text-message</param>
        /// <returns></returns>

    }
}