using App2.Model;
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
        private IList<CennikClass> idceny;
        private CennikClass nrcenika;
        string NazwaCennika;
        int NrCennika;
        SqlConnection connection;
        public TwrKarty twrkarty { get; set; }
        List<string> ceny;
        private List<string> nowy;


        public WeryfikatorCenPage()
        {
            InitializeComponent();
            var app = Application.Current as App;

            BindingContext = this;

            if (SettingsPage.OnAlfaNumeric)
                manualEAN.Keyboard = Keyboard.Default;

            settingsPage = new SettingsPage();
            idceny = settingsPage.cennikClasses;
            if (idceny != null)
            {
                nrcenika = idceny[app.Cennik];
                var rodzaj = (SettingsPage.CzyCenaPierwsza) ? " pierwsza" : nrcenika.RodzajCeny;
                NazwaCennika = $"cena [{rodzaj}]";
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
            //if (SettingsPage.SprConn())
            //{
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
            // }
            //else
            //{
            //  await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            //            }
        }
        public async void pobierztwrkod(string _ean)
        {
            string Webquery = "";
            var app = Application.Current as App;
            //SettingsPage settingsPage = new SettingsPage();
            //var idceny = settingsPage.cennikClasses;
            //var nrcenika = idceny[app.Cennik];
            twrkarty = null;
            try
            {
                if (SettingsPage.SprConn())
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
                            Webquery = $"cdn.pc_pobierztwr '{_ean}', {NrCennika}";
                            var dane = await App.TodoManager.PobierzTwrAsync(Webquery);

                            twrkarty = new TwrKarty
                            {

                                twr_kod = Convert.ToString(rs["twr_kod"]),
                                twr_gidnumer = Convert.ToString(rs["twr_gidnumer"]),
                                stan_szt = Convert.ToString(rs["ilosc"]),
                                twr_url = Convert.ToString(rs["twr_url"]),
                                twr_nazwa = Convert.ToString(rs["twr_nazwa"]),
                                twr_symbol = Convert.ToString(rs["twr_symbol"]),
                                twr_ean = Convert.ToString(rs["twr_ean"]),
                                twr_cena = Convert.ToString(rs["cena"]).Replace(",", "."),
                                twr_cena1 = (dane.Count > 0) ? dane[0].cena1 : ""
                            };
                            // DisplayAlert("Zeskanowany Kod ", twrkod, "OK");
                        }
                        else
                        {
                            // idceny = settingsPage.cennikClasses;
                            //var nrcenika = idceny[app.Cennik];
                            if (nrcenika.RodzajCeny == "OUTLET")
                                NrCennika = 4;
                            else NrCennika = 2;
                            Webquery = $"cdn.pc_pobierztwr '{_ean}', {NrCennika}";
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
                                    twr_cena1 = dane[0].cena1
                                };

                            }

                            DependencyService.Get<Model.IAppVersionProvider>().ShowShort("Brak stanów lub nie istnieje!");
                        }
                        rs.Close();
                        rs.Dispose();
                        connection.Close();
                    }


                }
                else
                {
                    //await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
                    DependencyService.Get<Model.IAppVersionProvider>().ShowShort("Brak połączenia z optimą!");

                    //idceny = settingsPage.cennikClasses;
                    //var nrcenika = idceny[app.Cennik];
                    if (nrcenika.RodzajCeny == "OUTLET")
                        NrCennika = 4;
                    else NrCennika = 2;
                    Webquery = $"cdn.pc_pobierztwr '{_ean}', {NrCennika}";
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
                            twr_cena1 = dane[0].cena1
                        };

                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Uwaga", "Nie znaleziono towaru..", "OK");
            }

            if (twrkarty != null)
            {
                twr_kod.Text = twrkarty.twr_kod;
                lbl_twrkod.Text = twrkarty.twr_ean;
                lbl_twrsymbol.Text = twrkarty.twr_symbol;
                lbl_twrnazwa.Text = twrkarty.twr_nazwa;
                lbl_stan.Text = twrkarty.stan_szt + " szt";
                lbl_twrcena.Text = (SettingsPage.CzyCenaPierwsza) ? twrkarty.twr_cena1 : twrkarty.twr_cena + " zł";
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
            if (twrkarty != null)
                await Navigation.PushModalAsync(new StanyTwrInnych(twrkarty.twr_gidnumer));
        }
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
            if(twrkarty!=null)
            if(!string.IsNullOrEmpty(twrkarty.twr_url))
            Launcher.OpenAsync(twrkarty.twr_url.Replace("Miniatury/", "").Replace("small","large"));
        }        
        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {

            nowy = new List<string>();

            if(twrkarty != null)
            {
                string Webquery = $@"cdn.PC_WykonajSelect N' select distinct   AkN_GidNazwa ,AkN_NazwaAkcji, 
                        AkN_DataStart,AkN_DataKoniec
                        from cdn.pc_akcjeNag
                        JOIN CDN.PC_AkcjeElem ON AkN_GidNumer = Ake_AknNumer
                        where Ake_FiltrSQL like ''%{twrkarty.twr_kod}%''  and AkN_DataKoniec>= GETDATE() - 30
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
                if (isok )
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
            if (twrkarty!=null)
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