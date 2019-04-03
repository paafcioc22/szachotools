using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrzyjmijMM_ListaMMDoPrzyjecia : ContentPage
    {
        Model.PrzyjmijMMClass PrzyjmijMMClass;  
        public   PrzyjmijMM_ListaMMDoPrzyjecia()
        {
            InitializeComponent();
            this.BindingContext = this;
           
        }



        protected async override void OnAppearing()
        {

            PrzyjmijMMClass = new Model.PrzyjmijMMClass();
            //PobierzListe();
            await PrzyjmijMMClass.getListMM();

            BindingContext = Model.PrzyjmijMMClass.ListaMMDoPrzyjcia;
            ListaMMek.ItemsSource = Model.PrzyjmijMMClass.ListaMMDoPrzyjcia;

            base.OnAppearing();

        }


        //private async  void PobierzListe()
        //{

        //    try
        //    {

        //       await PrzyjmijMMClass.getListMM();
        //    }
        //    catch (Exception s)
        //    {

        //      await  DisplayAlert(null, s.Message, "OK");
        //    }

        //}

        bool _userTapped;
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IsBusy = true;
            if (e.Item == null)
                return;

                if (_userTapped)
                    return;

                _userTapped = true;
                var mm = e.Item as Model.PrzyjmijMMClass;
             
                    await Navigation.PushModalAsync(new PrzyjmijMM_ListaElementowMM(null, mm.GIDdokumentuMM));
               
                IsBusy = false;
              
                ((ListView)sender).SelectedItem = null;
            _userTapped = false;
        }

        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        string EANKodMM;
        Model.PrzyjmijMMClass przyjmijMMClass;
       

        private async void Btn_Skanuj_Clicked(object sender, EventArgs e)
        {
            opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
            {
                AutoRotate = false,
                PossibleFormats = new List<ZXing.BarcodeFormat>() {

                ZXing.BarcodeFormat.CODE_128,
                ZXing.BarcodeFormat.CODABAR,
                ZXing.BarcodeFormat.CODE_39,

                }

            };

            opts.TryHarder = true;
            var torch = new Switch
            {

            };
            torch.Toggled += delegate
            {
                scanPage.ToggleTorch();
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,

            };

            var Overlay = new ZXingDefaultOverlay
            {
                TopText = "Przyłóż telefon do kodu",
                BottomText = "Skanowanie rozpocznie się automatycznie",
                ShowFlashButton = true,
                AutomationId = "zxingDefaultOverlay",
            };

            grid.Children.Add(Overlay);
            Overlay.Children.Add(torch);
            Overlay.BindingContext = Overlay;

            scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
            {
                DefaultOverlayTopText = "Zeskanuj kod ",
                //DefaultOverlayBottomText = " Skanuj kod ";
                DefaultOverlayShowFlashButton = true
            };

           
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                scanPage.AutoFocus();
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopModalAsync();
                    EANKodMM = result.Text;
                    int? MMgidNumer = null;
                    przyjmijMMClass = new Model.PrzyjmijMMClass();
                    przyjmijMMClass.ReturnGidNumerFromEANMM(EANKodMM, out MMgidNumer);

                    if (MMgidNumer == 0 || MMgidNumer == null)
                    {
                        DisplayAlert("Uwaga", "Brak MM w systemie", "OK");
                        return;
                    }
                    else {
                        Navigation.PushModalAsync(new PrzyjmijMM_ListaElementowMM(EANKodMM, (int)MMgidNumer)); 
                    }
                    //DisplayAlert("Zeskanowany Kod ", gidnumerMM, "OK");


                   // Navigation.PushModalAsync(new ListaMMP(nrdokumentuMM, 0));
                 
                });
            };
            await Navigation.PushModalAsync(scanPage);
        }
    }
}
