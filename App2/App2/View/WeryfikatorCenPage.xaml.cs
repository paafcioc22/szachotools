using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        string NazwaCennika;
        int NrCennika;
        SqlConnection connection;
         
        public WeryfikatorCenPage()
        {
            InitializeComponent();
            var app = Application.Current as App;

             

            SettingsPage settingsPage = new SettingsPage();
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
            else 
            Appearing += (object sender, System.EventArgs e) => manualEAN.Focus();
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
                            if (scanPage.IsScanning) scanPage.AutoFocus(); return true;
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

        string twrkod;
        string twrgidnumer;
        string stan_szt;
        string twr_url;
        string twr_nazwa;
        string twr_symbol;
        string twr_ean;
        string twr_cena;

        public async void pobierztwrkod(string _ean)
        {
            var app = Application.Current as App;
            //SettingsPage settingsPage = new SettingsPage();
            //var idceny = settingsPage.cennikClasses;
            //var nrcenika = idceny[app.Cennik];

            if (SettingsPage.SprConn())
            {
                try
                {

                    if (!string.IsNullOrEmpty(_ean))
                    {
                        SqlCommand command = new SqlCommand();

                        connection.Open();
                        command.CommandText = "Select twr_gidnumer,twr_kod, twr_nazwa, Twr_NumerKat twr_symbol, cast(twc_wartosc as decimal(5,2))cena " +
                            ",cast(sum(TwZ_Ilosc) as int)ilosc, twr_url,twr_ean " +
                            "from cdn.towary " +
                            "join cdn.TwrCeny on Twr_twrid = TwC_Twrid and TwC_TwCNumer =  " + NrCennika +
                            " join cdn.TwrZasoby on Twr_twrid = TwZ_TwrId " +
                            "where twr_ean='" + _ean + "'" +
                            "group by twr_gidnumer,twr_kod, twr_nazwa, Twr_NumerKat,twc_wartosc, twr_url,twr_ean";


                        SqlCommand query = new SqlCommand(command.CommandText, connection);
                        SqlDataReader rs;
                        rs = query.ExecuteReader();
                        if (rs.Read())
                        {
                            twrkod = Convert.ToString(rs["twr_kod"]);
                            twrgidnumer = Convert.ToString(rs["twr_gidnumer"]);
                            stan_szt = Convert.ToString(rs["ilosc"]);
                            twr_url = Convert.ToString(rs["twr_url"]);
                            twr_nazwa = Convert.ToString(rs["twr_nazwa"]);
                            twr_symbol = Convert.ToString(rs["twr_symbol"]);
                            twr_ean = Convert.ToString(rs["twr_ean"]);
                            twr_cena = Convert.ToString(rs["cena"]);

                            // DisplayAlert("Zeskanowany Kod ", twrkod, "OK");
                        }
                        else
                        {
                            string Webquery = "cdn.pc_pobierztwr '" + _ean + "'";
                            var dane = await App.TodoManager.PobierzTwrAsync(Webquery);
                            twrgidnumer = dane[0].TwrGidnumer.ToString();
                            twrkod = dane[0].twrkod;
                            twr_url = dane[0].url;
                            twr_nazwa = dane[0].nazwa;
                            twr_ean = dane[0].ean;
                            twr_cena = dane[0].cena;

                            await DisplayAlert("Uwaga", "Brak stanów lub nie istnieje!", "OK");
                        }
                        rs.Close();
                        rs.Dispose();
                        connection.Close(); 
                    }

                }
                catch (Exception exception)
                {
                    await DisplayAlert("Uwaga", exception.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
            }
            //return twrkod; lbl_twrkod.Text = "Kod : " + twrkod;


            twr_kod.Text = twrkod;
            lbl_twrkod.Text = twr_ean;
            lbl_twrsymbol.Text = twr_symbol;
            lbl_twrnazwa.Text = twr_nazwa;
            lbl_stan.Text = stan_szt + " szt";
            lbl_twrcena.Text = twr_cena + " zł";
            img_foto.Source = twr_url;

            
         //   img_foto.GestureRecognizers.Add(tapGestureRecognizer);

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
            if (twrgidnumer != null)
                await Navigation.PushModalAsync(new StanyTwrInnych(twrgidnumer));
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
                 and Twr_GIDNumer={twrgidnumer}";

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
            Launcher.OpenAsync(twr_url.Replace("Miniatury/", "")) ;
        }

        //private List<string> nowy;
        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            nowy = new List<string>();
            string Webquery = $@"cdn.PC_WykonajSelect N' select distinct   AkN_GidNazwa ,AkN_NazwaAkcji,

    AkN_DataStart,AkN_DataKoniec
 from cdn.pc_akcjeNag
    INNER JOIN CDN.PC_AkcjeElem ON AkN_GidNumer = Ake_AknNumer
 where Ake_FiltrSQL like ''%{twrkod}%''  and AkN_DataKoniec>= GETDATE() - 10
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
    }
}