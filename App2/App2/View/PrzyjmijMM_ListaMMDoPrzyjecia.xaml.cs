using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Xamarin.Essentials;
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
            BindingContext = this;
            ToggleScreenLock();
        }

         
            public void ToggleScreenLock()
            {
                DeviceDisplay.KeepScreenOn = !DeviceDisplay.KeepScreenOn;
            }


        protected override bool OnBackButtonPressed()
        {
            ToggleScreenLock();
            return base.OnBackButtonPressed();
        }


        protected async override void OnAppearing()
        {

            PrzyjmijMMClass = new Model.PrzyjmijMMClass();
            //PobierzListe();
            await PrzyjmijMMClass.getListMM(View.SettingsPage.IsBuforOff);

            //BindingContext = Model.PrzyjmijMMClass.ListaMMDoPrzyjcia;
            BindingContext = this;
            ListaMMek.ItemsSource = Model.PrzyjmijMMClass.ListaMMDoPrzyjcia;

            base.OnAppearing();

        }

        private BindableProperty IsSearchingProperty =
            BindableProperty.Create("IsSearching", typeof(bool), typeof(PrzyjmijMM_ListaMMDoPrzyjecia), false);
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
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

            IsSearching = true;
                    var mm = e.Item as Model.PrzyjmijMMClass;
             

            
            ((ListView)sender).SelectedItem = null;
            _userTapped = false;


            var delay = await Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Device.BeginInvokeOnMainThread(async () =>
                {

                    if (e.Item == null)
                        return;

                    if (_userTapped)
                        return;

                    _userTapped = true;

                    await Navigation.PushModalAsync(new PrzyjmijMM_ListaElementowMM(  mm.GIDdokumentuMM)); 

                    IsSearching = false;

                });
                return 0;
            });

             
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
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                    EANKodMM = result.Text;
                    int? MMgidNumer = 0;
                    przyjmijMMClass = new Model.PrzyjmijMMClass();
                    MMgidNumer= await przyjmijMMClass.ReturnGidNumerFromEANMM(EANKodMM);

                    if (MMgidNumer == 0 )
                    {
                        await DisplayAlert("Uwaga", "Brak MM w systemie", "OK");
                        return;
                    }
                    else {
                        await Navigation.PushModalAsync(new PrzyjmijMM_ListaElementowMM((int)MMgidNumer)); 
                    } 
                 
                });
            };
            await Navigation.PushModalAsync(scanPage);
        }
    }
}
