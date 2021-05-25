﻿using App2.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
        string NazwaCennika;
        int NrCennika;
        SqlConnection connection;
        public TwrKarty  twrkarty  { get; set; }


        public WeryfikatorCenPage()
        {
            InitializeComponent();
            var app = Application.Current as App;

            BindingContext = this;

            settingsPage = new SettingsPage();
            var idceny = settingsPage.cennikClasses;
            if (idceny != null)
            {
                var nrcenika = idceny[app.Cennik];
                NazwaCennika = "cena [" + nrcenika.RodzajCeny + "]";
                lbl_cennik.Text = NazwaCennika;
                NrCennika = nrcenika.Id;
            }
            connection = new SqlConnection
            {
                ConnectionString = "SERVER=" + app.Serwer +
                ";DATABASE=" + app.BazaProd +
                ";TRUSTED_CONNECTION=No;UID=" + app.User +
                ";PWD=" + app.Password
            };

            if (SettingsPage.SelectedDeviceType == 1)
                SkanowanieEan(); //aparat
             
            //Appearing += (object sender, System.EventArgs e) => manualEAN.Focus();
        }



        private async void SkanowanieEan()
        {
            if (SettingsPage.SprConn())
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>() {

                //ZXing.BarcodeFormat.CODE_128,
                //ZXing.BarcodeFormat.CODABAR,
                ZXing.BarcodeFormat.CODE_39,
                ZXing.BarcodeFormat.EAN_13
                }

                };

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
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                        {
                            if (scanPage.IsScanning) scanPage.AutoFocus();
                            return true;
                        });
                        Navigation.PopModalAsync();
                        pobierztwrkod(result.Text);


                    });
                };
                await Navigation.PushModalAsync(scanPage);
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            }
        }

     

        public async void pobierztwrkod(string _ean)
        {
            var app = Application.Current as App;
            //SettingsPage settingsPage = new SettingsPage();
            //var idceny = settingsPage.cennikClasses;
            //var nrcenika = idceny[app.Cennik];
            twrkarty = null;
            if (SettingsPage.SprConn())
            {
                try
                {

                    if (!string.IsNullOrEmpty(_ean))
                    {
                        SqlCommand command = new SqlCommand();

                        connection.Open();
                        command.CommandText = $@"Select twr_gidnumer,twr_kod, twr_nazwa, Twr_NumerKat twr_symbol, cast(twc_wartosc as decimal(5,2))cena  
                            ,cast(sum(TwZ_Ilosc) as int)ilosc, twr_url,twr_ean 
                            from cdn.towary 
                            join cdn.TwrCeny on Twr_twrid = TwC_Twrid and TwC_TwCNumer =   {NrCennika }
                             join cdn.TwrZasoby on Twr_twrid = TwZ_TwrId  
                            where twr_ean='{_ean}'
                            group by twr_gidnumer,twr_kod, twr_nazwa, Twr_NumerKat,twc_wartosc, twr_url,twr_ean";

                        
                        SqlCommand query = new SqlCommand(command.CommandText, connection);
                        SqlDataReader rs;
                        rs = query.ExecuteReader();
                        if (rs.Read())
                        {
                            twrkarty = new TwrKarty
                            {
                                 
                                twr_kod = Convert.ToString(rs["twr_kod"]),
                                twr_gidnumer = Convert.ToString(rs["twr_gidnumer"]),
                                stan_szt = Convert.ToString(rs["ilosc"]),
                                twr_url = Convert.ToString(rs["twr_url"]),
                                twr_nazwa = Convert.ToString(rs["twr_nazwa"]),
                                twr_symbol = Convert.ToString(rs["twr_symbol"]),
                                twr_ean = Convert.ToString(rs["twr_ean"]),
                                twr_cena = Convert.ToString(rs["cena"]).Replace(",","."),
                            };
                            // DisplayAlert("Zeskanowany Kod ", twrkod, "OK");
                        }
                        else
                        {
                            var idceny = settingsPage.cennikClasses;
                            var nrcenika = idceny[app.Cennik];
                            if (nrcenika.RodzajCeny == "OUTLET")
                                NrCennika = 4;
                            else NrCennika = 2;
                            string Webquery = $"cdn.pc_pobierztwr '{_ean}', {NrCennika}";
                            var dane = await App.TodoManager.PobierzTwrAsync(Webquery);
                            if (dane.Count > 0)
                            {
                               
                                twrkarty = new TwrKarty
                                {
                                    twr_gidnumer = dane[0].TwrGidnumer.ToString(),
                                    twr_kod = dane[0].twrkod,
                                    twr_url = dane[0].url,
                                    twr_nazwa = dane[0].nazwa,
                                    twr_ean = dane[0].ean,
                                    twr_cena = dane[0].cena,
                                    twr_symbol = dane[0].symbol,    
                                    twr_cena1=dane[0].cena1
                                };

                            } 
                             
                            DependencyService.Get<Model.IAppVersionProvider>().ShowShort("Brak stanów lub nie istnieje!");
                        }
                        rs.Close();
                        rs.Dispose();
                        connection.Close(); 
                    }

                }
                catch (Exception )
                {
                    await DisplayAlert("Uwaga","Nie znaleziono towaru..", "OK");
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
            }

            if (twrkarty != null)
            {
                twr_kod.Text = twrkarty.twr_kod;
                lbl_twrkod.Text = twrkarty.twr_ean;
                lbl_twrsymbol.Text = twrkarty.twr_symbol;
                lbl_twrnazwa.Text = twrkarty.twr_nazwa;
                lbl_stan.Text = twrkarty.stan_szt + " szt";
                lbl_twrcena.Text = twrkarty.twr_cena + " zł";
                img_foto.Source = twrkarty.twr_url;
            }
            else
            {
                twr_kod.Text = "";
                lbl_twrkod.Text = "";
                lbl_twrsymbol.Text = "";
                lbl_twrnazwa.Text = "";
                lbl_stan.Text = "";
                lbl_twrcena.Text = "";
                img_foto.Source = "";
            }
           



            connection.Close();

        }


        

        private void ScanTwr_Clicked(object sender, EventArgs e)
        {
            SkanowanieEan();
        }

        private void kodean_Unfocused(object sender, FocusEventArgs e)
        {
            pobierztwrkod(manualEAN.Text);
            manualEAN.Text = "";
        }



        private async void BtnShowOther_Clicked(object sender, EventArgs e)
        {
            if (twrkarty.twr_gidnumer != null)
                await Navigation.PushModalAsync(new StanyTwrInnych(twrkarty.twr_gidnumer));
        }

        List<string> ceny;
        private List<string> nowy;

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ceny = new List<string>();
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = $@"SELECT DfC_Nazwa +' : '+cast(CAST(TwC_Wartosc AS DECimal(8,2)) as varchar)+' zł' ceny FROM   cdn.towary 
                join cdn.TwrCeny on Twr_twrid = TwC_Twrid  
                join  CDN.DefCeny on TwC_TwCNumer=DfC_lp
                where DfC_Nieaktywna = 0
                 and Twr_GIDNumer={twrkarty.twr_gidnumer}";

                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                while (rs.Read())
                {
                    ceny.Add(Convert.ToString(rs["ceny"]));
                }
                DisplayActionSheet("Ceny:", "OK", null, ceny.ToArray());

                rs.Close();
                rs.Dispose();


            }
            catch (Exception ex)
            {
                DisplayAlert("Uwaga", ex.Message, "OK");
            }
             
            connection.Close();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Launcher.OpenAsync(twrkarty.twr_url.Replace("Miniatury/", "")) ;
        }

        //private List<string> nowy;
        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            nowy = new List<string>();
            string Webquery = $@"cdn.PC_WykonajSelect N' select distinct   AkN_GidNazwa ,AkN_NazwaAkcji,

    AkN_DataStart,AkN_DataKoniec
 from cdn.pc_akcjeNag
    INNER JOIN CDN.PC_AkcjeElem ON AkN_GidNumer = Ake_AknNumer
 where Ake_FiltrSQL like ''%{twrkarty.twr_kod}%''  and AkN_DataKoniec>= GETDATE() - 30
 order by AkN_DataStart desc   '";
            var dane = await App.TodoManager.GetGidAkcjeAsync(Webquery);

            foreach (var wpis in dane)
            {
                nowy.Add(String.Concat(wpis.AkN_GidNazwa, "\r\n", 
                    wpis.AkN_NazwaAkcji, "\r\n", 
                    wpis.AkN_ZakresDat, "\r\n- - - - - - - - - - - - - - - -"));
            }

            await DisplayActionSheet("Aktualne Akcje:", "OK", null, nowy.ToArray());
            //and AkN_DataKoniec>= GETDATE() - 10
        }

        private async void btn_print_Clicked(object sender, EventArgs e)
        {
           
            
            
            PrintSerwis printSerwis = new PrintSerwis();
            //if (twrkarty != null & !string.IsNullOrEmpty(twrkarty.stan_szt))
           // {
                if (await printSerwis.ConnToPrinter())
                {
                    List<string> kolory = new List<string> {
                    "biały", "pomarańczowy"
                };
                    //var kolor = await DisplayAlert(null, "Wybierz kolor Etykiety..", "Tak", "Nie");
                    var kolor = await DisplayActionSheet("Wybierz kolor Etykiety..:", "Anuluj", null, kolory.ToArray());
                    if (kolor != "Anuluj")
                    {
                        string result = await DisplayPromptAsync("Ile metek chcesz wydrukować?", "", "OK", "Anuluj", "1", 1, Keyboard.Numeric);
                        if (result != null)
                            await printSerwis.PrintCommand(twrkarty, kolor, result);
                    }

                }
                else
                {
                    await DisplayAlert(null, "Błąd drukarki", "OK");
                }
           // }
           // else
            //{
              //  await DisplayAlert("Uwaga", "Brak na stanie - nie wydrukuję", "OK");
           // }
           


        }
    }
}