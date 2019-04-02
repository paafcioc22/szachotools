using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace App2
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]


    //public partial class ListaMMP : ContentPage
    //{
    //    ZXingScannerPage scanPage;
    //    ZXingScannerView zxing;
    //    //ZXingBarcodeImageView barcode;○
    //    ZXing.Mobile.MobileBarcodeScanningOptions opts;

    //    public ListaMMP(string EAN_mmka, int gidnumer)
    //    {
    //        InitializeComponent();
    //        Model.PrzyjmijMMClass.ListOfTwrOnMM.Clear();
    //        Model.PrzyjmijMMClass przyjmijMMClass = new Model.PrzyjmijMMClass();

    //        przyjmijMMClass.GetlistMMElements(EAN_mmka, gidnumer);
    //        tytul.Text = przyjmijMMClass.GetNrDokMmp;// mmp;

    //        MyListView.ItemsSource = Model.PrzyjmijMMClass.GetMmkas();
    //    }

    //    async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
    //    {
    //        if (e.Item == null)
    //            return;
    //        var mmka = e.Item as Model.PrzyjmijMMClass;


    //        PromptResult prr = await UserDialogs.Instance.PromptAsync
    //            (
    //                new PromptConfig
    //                {
    //                    InputType = InputType.Number,
    //                    Message = "Podaj Ilośc:  ",
    //                    OkText = "Zatwierdź",
    //                    CancelText = "Anuluj",
    //                    Title = "Pytanie..",
    //                    IsCancellable = true,
    //                }
    //            );
    //        //await DisplayAlert("info", prr.Text, "ok");
    //        if (prr.Text != "")
    //        {
    //            mmka.ilosc_OK = Convert.ToInt16(prr.Text);
    //        }


    //        //MyListView.ItemsSource = Model.PrzyjmijMMClass.GetMmkas();
    //        //MyListView.EndRefresh();

    //        ((ListView)sender).SelectedItem = null;
    //    }


    //    private async void Btn_Skanuj_twr_clicked(object sender, EventArgs e)
    //    {
    //        Model.PrzyjmijMMClass mn = new Model.PrzyjmijMMClass();
    //        if (View.SettingsPage.SprConn())
    //        {
    //            opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
    //            {
    //                AutoRotate = false,
    //                PossibleFormats = new List<ZXing.BarcodeFormat>() {
    //            //ZXing.BarcodeFormat.EAN_8,
    //            ZXing.BarcodeFormat.EAN_13,
    //            //ZXing.BarcodeFormat.CODE_128,
    //            //ZXing.BarcodeFormat.CODABAR,
    //            //ZXing.BarcodeFormat.CODE_39,

    //            }

    //            };

    //            opts.TryHarder = true;

    //            zxing = new ZXingScannerView
    //            {
    //                //HorizontalOptions = LayoutOptions.Center,
    //                //VerticalOptions = LayoutOptions.Center,
    //                IsScanning = false,
    //                IsTorchOn = false,
    //                IsAnalyzing = false,
    //                AutomationId = "zxingDefaultOverlay",//zxingScannerView
    //                Opacity = 22,
    //                Options = opts
    //            };

    //            var torch = new Switch
    //            {
    //                //Text = "ON/OFF Latarka"

    //            };
    //            torch.Toggled += delegate
    //            {
    //                scanPage.ToggleTorch();
    //            };
    //            var grid = new Grid
    //            {
    //                VerticalOptions = LayoutOptions.Center,
    //                HorizontalOptions = LayoutOptions.Center,

    //            };
    //            var Overlay = new ZXingDefaultOverlay
    //            {
    //                TopText = "Przyłóż telefon do kodu",
    //                BottomText = "Skanowanie rozpocznie się automatycznie",
    //                ShowFlashButton = true,
    //                AutomationId = "zxingDefaultOverlay",
    //            };
    //            //customOverlay.Children.Add(torch);
    //            grid.Children.Add(Overlay);
    //            Overlay.Children.Add(torch);
    //            Overlay.BindingContext = Overlay;

    //            scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
    //            {
    //                DefaultOverlayTopText = "Zeskanuj kod ",
    //                //DefaultOverlayBottomText = " Skanuj kod ";
    //                DefaultOverlayShowFlashButton = true
    //            };


    //            scanPage.OnScanResult += (result) =>
    //            {
    //                scanPage.IsScanning = false;
    //                scanPage.AutoFocus();
    //                Device.BeginInvokeOnMainThread(() =>
    //                {
    //                    Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
    //                    {
    //                        if (scanPage.IsScanning) scanPage.AutoFocus(); return true;
    //                    });
    //                    Navigation.PopModalAsync();
    //                    mn.pobierztwrkod(result.Text);
    //                    DisplayAlert("Zeskanowany Kod ", result.Text, "OK");
    //                    szukaj.Text = mn.twrkod;

    //                });
    //            };
    //            await Navigation.PushModalAsync(scanPage);
    //        }
    //        else
    //        {
    //            await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

    //        }

    //    }

    //    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    //    {
    //        //Model.PrzyjmijMMClass przyjmijMMClass = new Model.PrzyjmijMMClass();
    //        MyListView.ItemsSource = Model.PrzyjmijMMClass.GetMmkas(e.NewTextValue);
    //    }
    //}
}
