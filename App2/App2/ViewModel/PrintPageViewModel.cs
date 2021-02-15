//using App2.Model;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Windows.Input;
//using Xamarin.Forms;

//namespace App2.ViewModel
//{
//    public class PrintPageViewModel
//    {
//        private readonly IBlueToothService _blueToothService;

//        private IList<string> _deviceList;
//        public IList<string> DeviceList
//        {
//            get
//            {
//                if (_deviceList == null)
//                    _deviceList = new ObservableCollection<string>();
//                return _deviceList;
//            }
//            set
//            {
//                _deviceList = value;
//            }
//        }

//        private string _printMessage;
//        public string PrintMessage
//        {
//            get
//            {
//                return _printMessage;
//            }
//            set
//            {
//                _printMessage = value;
//            }
//        }

//        private string _selectedDevice;
//        public string SelectedDevice
//        {
//            get
//            {
//                return _selectedDevice;
//            }
//            set
//            {
//                _selectedDevice = value;
//            }
//        }

//        /// <summary>
//        /// Print text-message
//        /// </summary>
//        /// 
         
//        public ICommand PrintCommand => new Command(async () =>
//        {


//            PrintMessage += @"!0 200 200 210 1" + Environment.NewLine + "TEXT 4 0 30 40 Hello World" + Environment.NewLine + "FORM" + Environment.NewLine + "PRINT" + Environment.NewLine;



//            await _blueToothService.Print(SelectedDevice, PrintMessage);
//        });

//        public PrintPageViewModel()
//        {
//            _blueToothService = DependencyService.Get<IBlueToothService>();
//            BindDeviceList();
//        }

//        /// <summary>
//        /// Get Bluetooth device list with DependencyService
//        /// </summary>
//        void BindDeviceList()
//        {
//            var list = _blueToothService.GetDeviceList();
//            DeviceList.Clear();
//            foreach (var item in list)
//                DeviceList.Add(item);
//        }
//    }
//}