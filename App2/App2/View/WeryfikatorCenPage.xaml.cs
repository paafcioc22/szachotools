using App2.Model;
using App2.Model.ApiModel;
using App2.Services;
using Newtonsoft.Json;
using RestSharp;
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
    public partial class WeryfikatorCenPage : ContentPage
    {
        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;
        SettingsPage settingsPage;


        private RestClient _client;
        string NazwaCennika;
        int NrCennika;

        public TwrInfo twrkarty { get; set; }

        private List<string> nowy;


        public WeryfikatorCenPage()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _client = new RestClient($"http://{app.Serwer}");
            BindingContext = this;

            if (SettingsPage.OnAlfaNumeric)
                manualEAN.Keyboard = Keyboard.Default;

            settingsPage = new SettingsPage();

            if (!string.IsNullOrEmpty(app.Cennik))
            {
                var rodzaj = (SettingsPage.CzyCenaPierwsza) ? " pierwsza" : app.Cennik;
                NazwaCennika = $"cena [{rodzaj}]";
                lbl_cennik.Text = NazwaCennika;
                //NrCennika = nrcenika.Id;
            }


            if (SettingsPage.SelectedDeviceType == 1)
                SkanowanieEan(); //aparat

            //Appearing += (object sender, System.EventArgs e) => manualEAN.Focus();
        }
        private async void SkanowanieEan()
        {
            var testBt = await PermissionService.CheckAndRequestPermissionAsync(new Permissions.Camera());
            if (testBt == PermissionStatus.Granted)
            {


                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,

                    PossibleFormats = new List<ZXing.BarcodeFormat>() {

                    //ZXing.BarcodeFormat.CODE_128,
                    ZXing.BarcodeFormat.CODABAR,
                    ZXing.BarcodeFormat.CODE_39,
                    ZXing.BarcodeFormat.EAN_13
                }

                };
                if (SettingsPage.OnAlfaNumeric)
                    opts.PossibleFormats.Add(ZXing.BarcodeFormat.CODE_128);

                opts.TryHarder = true;

                zxing = new ZXingScannerView
                {

                    IsScanning = false,
                    IsTorchOn = false,
                    IsAnalyzing = false,
                    AutomationId = "zxingDefaultOverlay",//zxingScannerView
                    Opacity = 22,
                    Options = opts
                };

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
                    TopText = "Włącz latarkę",
                    BottomText = "Skanowanie rozpocznie się automatycznie",
                    ShowFlashButton = true,
                    AutomationId = "zxingDefaultOverlay",

                };

                var customOverlay = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
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

                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                        {
                            if (scanPage.IsScanning) scanPage.AutoFocus();
                            return true;
                        });
                        await Navigation.PopModalAsync();
                        await pobierztwrkod(result.Text);


                    });
                };
                await Navigation.PushModalAsync(scanPage);
            }
            else
            {
                await DisplayAlert("uwaga", "brak uprawnień do aparatu", "OK");
            }
             
        }



        public async Task pobierztwrkod(string _ean)
        {

            var app = Application.Current as App;
            //TwrInfo product = null;


            try
            {
                if (await SettingsPage.SprConn())
                {

                    if (!string.IsNullOrEmpty(_ean))
                    {
                        if (app.Cennik == "OUTLET")
                            NrCennika = 4;
                        else NrCennika = 2;

                        var karta = new TwrKodRequest()
                        {
                            Twrcenaid = NrCennika + 1,
                            Twrean = _ean
                        };

                        var request = new RestRequest("/api/gettowar")
                              .AddJsonBody(karta);

                        var response = await _client.ExecutePostAsync<List<TwrInfo>>(request);

                        twrkarty = await FilesHelper.GetCombinedTwrInfo(_ean, NrCennika, request, _client);

                        if (twrkarty != null)
                        {
                            twr_kod.Text = twrkarty.Twr_Kod;
                            lbl_twrkod.Text = twrkarty.Twr_Ean;
                            lbl_twrsymbol.Text = twrkarty.Twr_Symbol;
                            lbl_twrnazwa.Text = twrkarty.Twr_Nazwa;
                            lbl_stan.Text = twrkarty.Stan_szt + " szt";
                            lbl_twrcena.Text = (SettingsPage.CzyCenaPierwsza) ? twrkarty.Twr_Cena1.ToString() : twrkarty.Twr_Cena.ToString() + " zł";
                            lbl_twrcena30.Text = twrkarty.Twr_Cena30.ToString() + " zł";
                            img_foto.Source = FilesHelper.ConvertUrlToOtherSize(twrkarty.Twr_Url, twrkarty.Twr_Kod, FilesHelper.OtherSize.home, true);

                            if (twrkarty.Twr_Ean != _ean)
                            {
                                await DisplayAlert("Uwaga", $"nie znaleziono dokładnie tego EANu, ale kod {twrkarty.Twr_Ean} jest najblizej", "OK");
                            }

                        }
                        else
                        {
                            await DisplayAlert("Uwaga", "Nie znaleziono towaru", "OK");
                            twr_kod.Text = "";
                            lbl_twrkod.Text = "";
                            lbl_twrsymbol.Text = "";
                            lbl_twrnazwa.Text = "";
                            lbl_stan.Text = "";
                            lbl_twrcena.Text = "";
                            img_foto.Source = "";
                        }

                    }

                }
                else
                {
                    await DisplayAlert("uwaga", "brak połączenia z serwisem api", "OK");
                }
            }
            catch (Exception s)
            {
                await DisplayAlert("Uwaga", s.Message, "OK");
            }





        }
        private void ScanTwr_Clicked(object sender, EventArgs e)
        {
            SkanowanieEan();
        }

        private async void kodean_Unfocused(object sender, FocusEventArgs e)
        {
            await pobierztwrkod(manualEAN.Text);
            manualEAN.Text = "";
        }

        private async void BtnShowOther_Clicked(object sender, EventArgs e)
        {
            if (twrkarty != null)
                await Navigation.PushModalAsync(new StanyTwrInnych(twrkarty.Twr_Gidnumer.ToString()));
        }
        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            //ceny = new List<string>(); 


            try
            {
                var request = new RestRequest($"/api/gettowarycena/{twrkarty.Twr_Gidnumer}", Method.Get);
                var response = await _client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    var jsonResponse = JsonConvert.DeserializeObject<TwrCeny>(response.Content);
                    var cenyTowarow = jsonResponse;

                    await DisplayActionSheet("Ceny:", "OK", null, cenyTowarow.Result.ToArray());
                }
                else
                {
                    await DisplayAlert("info", response.Content.ToString(), "OK");
                }


            }
            catch (Exception ex)
            {
                await DisplayAlert("Uwaga", ex.Message, "OK");
            }


        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (twrkarty != null)
                if (!string.IsNullOrEmpty(twrkarty.Twr_Url))
                    Launcher.OpenAsync(twrkarty.Twr_Url.Replace("Miniatury/", "").Replace("small", "large"));
        }
        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {

            nowy = new List<string>();

            if (twrkarty != null)
            {
                string Webquery = $@"cdn.PC_WykonajSelect N' select distinct   AkN_GidNazwa ,AkN_NazwaAkcji, 
                        AkN_DataStart,AkN_DataKoniec
                        from cdn.pc_akcjeNag
                        JOIN CDN.PC_AkcjeElem ON AkN_GidNumer = Ake_AknNumer
                        where Ake_FiltrSQL like ''%{twrkarty.Twr_Kod}%''  and AkN_DataKoniec>= GETDATE() - 30
                        order by AkN_DataStart desc '";

                var dane = await App.TodoManager.GetGidAkcjeAsync(Webquery);

                if (dane.Count > 0)
                {
                    foreach (var wpis in dane)
                    {
                        nowy.Add(String.Concat(wpis.AkN_GidNazwa, "\r\n",
                        wpis.AkN_NazwaAkcji, "\r\n",
                        wpis.AkN_ZakresDat, "\r\n- - - - - - - - - - - - - - - -"));
                    }

                    await DisplayActionSheet("Aktualne Akcje:", "OK", null, nowy.ToArray());
                }
                else
                {
                    await DisplayAlert(null, "Brak aktualnych akcji dla tego produktu", "OK");
                }
            }

        }
        bool IsStringCorrect(string wprowadzona, out sbyte liczba)
        {
            bool isCorrect = false;
            sbyte ileMetek;

            if (!string.IsNullOrEmpty(wprowadzona))
            {
                var isok = sbyte.TryParse(wprowadzona, out ileMetek);
                if (isok)
                {
                    isCorrect = (ileMetek > 0 && ileMetek <= 20);
                    liczba = ileMetek;
                }
                else
                {
                    liczba = 1;
                    return isCorrect;
                }
            }
            else
            {
                liczba = 1;
                return isCorrect;
            }

            return isCorrect;
        }
        private async void btn_print_Clicked(object sender, EventArgs e)
        {
            sbyte ileMetek;
            if (twrkarty != null)
            {

                PrintSerwis printSerwis = new PrintSerwis();

                if (await printSerwis.ConnToPrinter())
                {
                    List<string> kolory = new List<string> {
                    "biały", "pomarańczowy"
                };

                    var kolor = await DisplayActionSheet("Wybierz kolor Etykiety..:", "Anuluj", null, kolory.ToArray());
                    if (kolor != "Anuluj")
                    {
                        string result = await DisplayPromptAsync("Ile metek chcesz wydrukować?", "(1-20 szt)", "OK", "Anuluj", "1", 2, Keyboard.Numeric, "1");

                        if (IsStringCorrect(result, out ileMetek))
                        {
                            await printSerwis.PrintCommand(twrkarty, kolor, ileMetek);
                        }
                        else
                        {
                            await DisplayAlert(null, "Wprowadzono błędną wartość", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert(null, "Błąd drukarki", "OK");
                }
            }
            else
            {
                await DisplayAlert("uwaga", "Nie pobrano karty towaru", "OK");
            }
        }
    }
}