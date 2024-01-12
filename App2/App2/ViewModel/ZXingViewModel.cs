using App2.OptimaAPI;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public  class ZXingViewModel : ViewModelBase
    {
        
        public Xamarin.Forms.Command<ZXing.Result> ScanCommand { get; }
        public ZXing.Mobile.MobileBarcodeScanningOptions ScannerOptions { get; private set; }
        public event EventHandler<string> ScanCompleted;


        public bool IsScanning { get; set; }
        public bool IsAnalyzing { get; set; }
        public ZXing.Result Result { get; set; }

        public ZXingViewModel()
        {
            ScanCommand = new Xamarin.Forms.Command<ZXing.Result>(HandleScanResult);
            ScannerOptions = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.CODE_39, ZXing.BarcodeFormat.EAN_13 }
            };
            IsScanning = true;
            IsAnalyzing = true;
        }

        private async Task InitiateScan()
        {
            if (await CheckCameraPermissionAsync())
            {
                // Uprawnienia przyznane, aktywuj skanowanie
                IsScanning = true;  // Zakładając, że IsScanning kontroluje skaner
                IsAnalyzing = true; // Zakładając, że IsAnalyzing kontroluje analizę obrazu
            }
            else
            {
                // Uprawnienia nie przyznane, pokaż komunikat
                await Application.Current.MainPage.DisplayAlert("Problem", "No permissions to use camera.", "ΟΚ");
            }
        }

        public void StopScanning()
        {
            IsScanning = false;
            IsAnalyzing = false;
        }


        private  void HandleScanResult(ZXing.Result result)
        {
            // Logika obsługi zeskanowanego kodu

            ScanCompleted?.Invoke(this, result.Text);
        }

        private async Task<bool> CheckCameraPermissionAsync()
        {
            var granted = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (granted != PermissionStatus.Granted)
            {
                granted = await Permissions.RequestAsync<Permissions.Camera>();
            }
            return granted == PermissionStatus.Granted;
        }

    }
}
