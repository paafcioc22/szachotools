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
            //ScanCommand = new Xamarin.Forms.Command<ZXing.Result>(HandleScanResult);
            ScanCommand = new Xamarin.Forms.Command<ZXing.Result>(result =>
            {
                // Przekształć ZXing.Result na string i wywołaj zdarzenie ScanCompleted
                HandleScanResult(result.Text);
            });
            ScannerOptions = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.CODE_39, ZXing.BarcodeFormat.EAN_13, ZXing.BarcodeFormat.QR_CODE }
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


        public  void HandleScanResult(string result)
        {
            // Logika obsługi zeskanowanego kodu

            Device.BeginInvokeOnMainThread(() =>
            {
                if (result != null && !string.IsNullOrEmpty(result))
                {
                    // Zatrzymaj skanowanie, ponieważ mamy wynik
                    StopScanning();

                    // Wywołanie zdarzenia z wynikiem skanowania
                    ScanCompleted?.Invoke(this, result);
                }
            });

            //ScanCompleted?.Invoke(this, result.Text);
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
