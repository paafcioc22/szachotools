using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using static Xamarin.Forms.Button;
using static Xamarin.Forms.Button.ButtonContentLayout;

namespace App2.View
{
    class AddTwrPage : ContentPage
    {
        private Label stan;
        private Label ean;
        private Label symbol;
        private Label nazwa;
        private Entry kodean;
        private Entry ilosc;
        private Image foto;
        private Button btn_Skanuj;
        private Button btn_Zapisz;
        private Int32 _gidnumer; 

        string twrkod;
        string stan_szt;
        string twr_url;
        string twr_nazwa;
        string twr_symbol;
        string twr_ean;


        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;


        public AddTwrPage(int gidnumer) //dodawamoe pozycji
        {
            this.Title = "Dodaj MM";
            StackLayout stackLayout_gl = new StackLayout();
            StackLayout stackLayout = new StackLayout();
            StackLayout stack_naglowek = new StackLayout();

            if (SettingsPage.SelectedDeviceType == 1)
                SkanowanieEan();

            var absoluteLayout = new AbsoluteLayout();
            var centerLabel = new Label
            {
                Text = "Dodawanie Pozycji",
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.YellowGreen
            };

            AbsoluteLayout.SetLayoutBounds(centerLabel, new Rectangle(0, 0, 1, 10));
            absoluteLayout.Children.Add(centerLabel);

            Label lbl_naglowek = new Label();
            lbl_naglowek.HorizontalOptions = LayoutOptions.CenterAndExpand;
            lbl_naglowek.VerticalOptions = LayoutOptions.Start;
            lbl_naglowek.Text = "Dodawanie pozycji";
            lbl_naglowek.FontSize = 20;
            lbl_naglowek.TextColor = Color.Bisque;
            lbl_naglowek.BackgroundColor = Color.DarkCyan;

            stack_naglowek.HorizontalOptions = LayoutOptions.FillAndExpand;
            stack_naglowek.VerticalOptions = LayoutOptions.Start;
            stack_naglowek.BackgroundColor = Color.DarkCyan;
            stack_naglowek.Children.Add(lbl_naglowek);

            stackLayout_gl.Children.Add(stack_naglowek);

            foto = new Image();
            stackLayout.Children.Add(foto);

            _gidnumer = gidnumer;

            stan = new Label();
            stan.HorizontalOptions = LayoutOptions.Center;

            nazwa = new Label();
            nazwa.HorizontalOptions = LayoutOptions.Center;

            ean = new Label();
            ean.HorizontalOptions = LayoutOptions.Center;

            symbol = new Label();
            symbol.HorizontalOptions = LayoutOptions.Center;

            stackLayout.Children.Add(stan);
            stackLayout.Children.Add(nazwa);
            stackLayout.Children.Add(ean);
            stackLayout.Children.Add(symbol);

            kodean = new Entry();
            kodean.Keyboard = Keyboard.Text;
            kodean.Placeholder = "Wpisz ręcznie EAN lub skanuj";
            kodean.Keyboard = Keyboard.Telephone;
            kodean.Unfocused += Kodean_Unfocused;
            kodean.ReturnCommand = new Command(() => ilosc.Focus());
            stackLayout.Children.Add(kodean);

            ilosc = new Entry();
            ilosc.Keyboard = Keyboard.Text;
            ilosc.Placeholder = "Wpisz Ilość";
            ilosc.Keyboard = Keyboard.Telephone;
            ilosc.Completed += (object sender, EventArgs e) =>
            {
                ZapiszPozycje();

            };
            ilosc.HorizontalOptions = LayoutOptions.Center;
            stackLayout.Children.Add(ilosc);

            btn_Skanuj = new Button();
            btn_Skanuj.Text = "Skanuj EAN";
            btn_Skanuj.Clicked += Btn_Skanuj_Clicked;
            btn_Skanuj.ImageSource = "scan48x2.png";
            btn_Skanuj.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayout.Children.Add(btn_Skanuj);

            btn_Zapisz = new Button();
            btn_Zapisz.Text = "Zapisz pozycję";
            btn_Zapisz.Clicked += Btn_Zapisz_Clicked;
            btn_Zapisz.ImageSource = "save48x2.png";
            btn_Zapisz.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayout.Children.Add(btn_Zapisz);
            //stackLayout.HorizontalOptions = LayoutOptions.Center;
            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(30, 0, 30, 0);

            stackLayout_gl.Children.Add(stackLayout);
            //stackLayout_gl.VerticalOptions= LayoutOptions.CenterAndExpand;
            absoluteLayout.Children.Add(stackLayout_gl, new Rectangle(0, 0.5, 1, 1), AbsoluteLayoutFlags.HeightProportional);

            Content = stackLayout_gl;

        }

        //protected override bool OnBackButtonPressed()
        //{
        //    kodean.Focus();
        //    return true;
        //}


        public AddTwrPage(Model.DokMM mmka) //edycja
        {
            this.Title = "Dodaj MM";



            StackLayout stackLayout = new StackLayout();
            StackLayout stackLayout_gl = new StackLayout();
            StackLayout stack_naglowek = new StackLayout();

            Label lbl_naglowek = new Label();
            lbl_naglowek.HorizontalOptions = LayoutOptions.CenterAndExpand;
            lbl_naglowek.VerticalOptions = LayoutOptions.Start;
            lbl_naglowek.Text = "Edycja pozycji";
            lbl_naglowek.FontSize = 20;
            lbl_naglowek.TextColor = Color.Bisque;
            lbl_naglowek.BackgroundColor = Color.DarkCyan;

            stack_naglowek.HorizontalOptions = LayoutOptions.FillAndExpand;
            stack_naglowek.VerticalOptions = LayoutOptions.Start;
            stack_naglowek.BackgroundColor = Color.DarkCyan;
            stack_naglowek.Children.Add(lbl_naglowek);

            stackLayout_gl.Children.Add(stack_naglowek);

            foto = new Image();
            stackLayout.Children.Add(foto);
            _gidnumer = mmka.gidnumer;


            stan = new Label();
            stan.HorizontalOptions = LayoutOptions.Center;
            //stan.Text = "Stan";
            stackLayout.Children.Add(stan);

            ean = new Label();
            ean.HorizontalOptions = LayoutOptions.Center;

            symbol = new Label();
            symbol.HorizontalOptions = LayoutOptions.Center;

            nazwa = new Label();
            nazwa.HorizontalOptions = LayoutOptions.Center;
            stackLayout.Children.Add(nazwa);
            stackLayout.Children.Add(ean);
            stackLayout.Children.Add(symbol);

            kodean = new Entry();
            kodean.Keyboard = Keyboard.Text;
            kodean.Placeholder = "Wpisz ręcznie EAN lub skanuj";
            kodean.Text = mmka.twrkod;
            kodean.IsEnabled = false;
            kodean.Keyboard = Keyboard.Telephone;
            kodean.Unfocused += Kodean_Unfocused;
            kodean.ReturnCommand = new Command(() => ilosc.Focus());
            stackLayout.Children.Add(kodean);

            ilosc = new Entry();
            ilosc.Keyboard = Keyboard.Text;
            ilosc.Placeholder = "Wpisz Ilość";
            ilosc.Keyboard = Keyboard.Telephone;
            ilosc.Text = mmka.szt.ToString();
            ilosc.Completed += (object sender, EventArgs e) =>
            {
                EdytujPozyce();

            };
            ilosc.HorizontalOptions = LayoutOptions.Center;
            stackLayout.Children.Add(ilosc);

            btn_Skanuj = new Button();
            btn_Skanuj.Text = "Skanuj EAN";
            btn_Skanuj.Clicked += Btn_Skanuj_Clicked;
            btn_Skanuj.IsEnabled = false;
            btn_Skanuj.ImageSource = "scan48x2.png";
            btn_Skanuj.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayout.Children.Add(btn_Skanuj);

            btn_Zapisz = new Button();
            btn_Zapisz.Text = "Zapisz pozycję";
            btn_Zapisz.Clicked += Btn_Update_Clicked;
            btn_Zapisz.ImageSource = "save48x2.png";
            btn_Zapisz.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayout.Children.Add(btn_Zapisz);
            //stackLayout.HorizontalOptions = LayoutOptions.Center;
            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(30, 0, 30, 0);
            stackLayout_gl.Children.Add(stackLayout);
            Content = stackLayout_gl;
            GetDataFromTwrKod(mmka.twrkod, false);
            ilosc.Focus();
        }


        public AddTwrPage(Model.DokMM mmka, string CzyFoto = null)
        {
            this.Title = "Dodaj MM";
            StackLayout stackLayout_gl = new StackLayout();
            StackLayout stackLayout = new StackLayout();
            StackLayout stack_naglowek = new StackLayout();

            var absoluteLayout = new AbsoluteLayout();
            var centerLabel = new Label
            {
                Text = " Zdjecie"
               ,
                HorizontalOptions = LayoutOptions.StartAndExpand
               ,
                BackgroundColor = Color.YellowGreen
            };

            foto = new Image();
            stackLayout.Children.Add(foto);
            stackLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            stackLayout_gl.VerticalOptions = LayoutOptions.FillAndExpand;
            stackLayout_gl.Children.Add(stackLayout);
            //absoluteLayout.Children.Add(stackLayout_gl, new Rectangle(1, 1, 1, 1), AbsoluteLayoutFlags.HeightProportional);
            Content = stackLayout_gl;
            GetDataFromTwrKod(mmka.twrkod, true);


        }

        private void Kodean_Unfocused(object sender, FocusEventArgs e)
        {
            pobierztwrkod(kodean.Text);
        } 

        private void EdytujPozyce()
        {
            if (ilosc.Text != null && kodean.Text != null)
            {
                if (Int32.Parse(ilosc.Text) > Int32.Parse(stan_szt) && Int32.Parse(ilosc.Text) == 0)
                {
                    DisplayAlert(null, "Wpisana ilość przekracza stan ", "OK");
                }
                else
                {
                    Model.DokMM dokMM = new Model.DokMM();
                    dokMM.gidnumer = _gidnumer;
                    dokMM.twrkod = kodean.Text;
                    dokMM.szt = Convert.ToInt32(ilosc.Text);
                    dokMM.UpdateElement(dokMM);
                    //Model.DokMM.dokElementy.GetEnumerator();// (usunMM);
                    dokMM.getElementy(_gidnumer);
                    Navigation.PopModalAsync();
                }
            }
            else
            {
                DisplayAlert("Uwaga", "Nie uzupełniono wszystkich pól!", "OK");
            }
        }

        private void Btn_Update_Clicked(object sender, EventArgs e)
        {
            EdytujPozyce();
        }
        private async void ZapiszPozycje()
        {
            if (ilosc.Text != null && kodean.Text != null)
            {
                if ((Int32.Parse(ilosc.Text) > Int32.Parse(stan_szt)) || Int32.Parse(ilosc.Text) == 0)
                {
                    await DisplayAlert(null, "Wpisana ilość przekracza stan ", "OK");
                }
                else
                {
                    Model.DokMM dokMM = new Model.DokMM();
                    dokMM.gidnumer = _gidnumer;
                    dokMM.twrkod = kodean.Text;
                    dokMM.szt = Convert.ToInt32(ilosc.Text);

                    if (dokMM.ExistsOtherDocs(dokMM))
                        await DisplayAlert("Ostrzeżenie", "Dodawany towar znajduje się już na innej MM", "OK");

                    int IleIstnieje = dokMM.SaveElement(dokMM);



                    if (IleIstnieje > 0)
                    {
                        var odp = await DisplayAlert("UWAGA!", "Dodawany kod już znajduje się na liście. Chcesz zsumować ilości?", "TAK", "NIE");
                        if (odp)
                        {
                            int suma = Int32.Parse(ilosc.Text) + IleIstnieje;
                            if (suma > Int32.Parse(stan_szt))
                            {
                                await DisplayAlert(null, "Łączna ilość przekracza stan ", "OK");
                                return;
                            }
                            else
                            {
                                //Model.DokMM dokMM = new Model.DokMM();
                                dokMM.gidnumer = _gidnumer;
                                dokMM.twrkod = kodean.Text;
                                dokMM.szt = suma;// Convert.ToInt32(ilosc.Text);
                                dokMM.UpdateElement(dokMM);
                                dokMM.getElementy(_gidnumer);
                            }
                        }
                        else
                        {

                            await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
                        }
                    }
                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie uzupełniono wszystkich pól!", "OK");
            }
        }
        public async void ZapiszPozycje(int mmGidnumer, string twrKod, int ilosc, int stan_szt)
        {
            if ((ilosc) > (stan_szt) && (ilosc) == 0)
            {
                await DisplayAlert(null, "Wpisana ilość przekracza stan ", "OK");
            }
            else
            {
                Model.DokMM dokMM = new Model.DokMM();
                dokMM.gidnumer = mmGidnumer;
                dokMM.twrkod = twrKod;
                dokMM.szt = (ilosc);

                int IleIstnieje = dokMM.SaveElement(dokMM);

                if (IleIstnieje > 0)
                {
                    var odp = await DisplayAlert("UWAGA!", "Dodawany kod już znajduje się na liście. Chcesz zsumować ilości?", "TAK", "NIE");
                    if (odp)
                    {
                        int suma = (ilosc) + IleIstnieje;
                        if (suma > (stan_szt))
                        {
                            await DisplayAlert(null, "Łączna ilość przekracza stan ", "OK");
                            return;
                        }
                        else
                        {
                            //Model.DokMM dokMM = new Model.DokMM();
                            dokMM.gidnumer = mmGidnumer;
                            dokMM.twrkod = twrKod;
                            dokMM.szt = suma;// Convert.ToInt32(ilosc.Text);
                            dokMM.UpdateElement(dokMM);
                            dokMM.getElementy(_gidnumer);
                        }
                    }
                    else
                    {

                        await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
                    }
                }
                await Navigation.PopModalAsync();
            }

        }
        private void Btn_Zapisz_Clicked(object sender, EventArgs e)
        {
            ZapiszPozycje();

        } 

        //public bool SprConn() //Third way, slightly slower than Method 1
        //{

        //    var connStr = new SqlConnectionStringBuilder
        //    {
        //        DataSource = View.SettingsPage._serwer,
        //        InitialCatalog = View.SettingsPage._database,
        //        UserID = View.SettingsPage._uid,
        //        Password = View.SettingsPage._pwd,
        //        ConnectTimeout = 3
        //    }.ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            //DisplayAlert("Connected", "Połączono z siecia", "OK");
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
        //            return false;
        //        }
        //    }
        //}

        public async void SkanowanieEan()
        { 

            if (SettingsPage.SprConn())
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>()
                    {
                        ZXing.BarcodeFormat.EAN_13,
                        ZXing.BarcodeFormat.CODE_39,
                    },
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
                //customOverlay.Children.Add(torch);


                var customOverlay = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
                };
                //customOverlay.Children.Add(btn_Manual);


                //  grid.Children.Add(customOverlay); //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
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
              
                        ilosc.Focus();
                         
                    });
                };
                await Navigation.PushModalAsync(scanPage);
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            }
        }
         
        private void Btn_Skanuj_Clicked(object sender, EventArgs e)
        {
            SkanowanieEan();
        }
          
        public async void pobierztwrkod(string _ean)
        {
            var app = Application.Current as App;
            if (SettingsPage.SprConn())
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    SqlConnection connection = new SqlConnection
                    {
                        ConnectionString = "SERVER=" + app.Serwer +
                ";DATABASE=" + app.BazaProd +
                ";TRUSTED_CONNECTION=No;UID=" + app.User +
                ";PWD=" + app.Password
                    };
                    connection.Open();
                    command.CommandText = "Select twr_kod, twr_nazwa, Twr_NumerKat twr_symbol, cast(twc_wartosc as decimal(5,2))cena " +
                        ",cast(sum(TwZ_Ilosc) as int)ilosc, twr_url,twr_ean " +
                        "from cdn.towary " +
                        "join cdn.TwrCeny on Twr_twrid = TwC_Twrid and TwC_TwrLp = 2 " +
                        "join cdn.TwrZasoby on Twr_twrid = TwZ_TwrId " +
                        "where twr_ean='" + _ean + "'" +
                        "group by twr_kod, twr_nazwa, Twr_NumerKat,twc_wartosc, twr_url,twr_ean";


                    SqlCommand query = new SqlCommand(command.CommandText, connection);
                    SqlDataReader rs;
                    rs = query.ExecuteReader();
                    if (rs.Read())
                    {
                        twrkod = Convert.ToString(rs["twr_kod"]);
                        stan_szt = Convert.ToString(rs["ilosc"]);
                        twr_url = Convert.ToString(rs["twr_url"]);
                        twr_nazwa = Convert.ToString(rs["twr_nazwa"]);
                        twr_symbol = Convert.ToString(rs["twr_symbol"]);
                        twr_ean = Convert.ToString(rs["twr_ean"]);

                        // DisplayAlert("Zeskanowany Kod ", twrkod, "OK");
                    }
                    else
                    {

                        await DisplayAlert("Uwaga", "Kod nie istnieje!", "OK");
                    }
                    rs.Close();
                    rs.Dispose();
                    connection.Close();

                }
                catch (Exception exception)
                {
                    await DisplayAlert("Uwaga", exception.Message, "OK");
                }
            }
            else
            {
                string Webquery = "cdn.pc_pobierztwr '" + _ean + "'";
                var dane = await App.TodoManager.PobierzTwrAsync(Webquery);
                if (dane.Count > 0)
                {

                    twrkod = dane[0].twrkod;
                    twr_url = dane[0].url;
                    twr_nazwa = dane[0].nazwa;
                    twr_ean = dane[0].ean;
                    //twr_cena = dane[0].cena;
                } 
         
            }
       
            kodean.Text = twrkod;
            ean.Text = twr_ean;
            symbol.Text = twr_symbol;
            nazwa.Text = twr_nazwa;
            stan.Text = "Stan : " + stan_szt;
            foto.Source = twr_url;
        }
         
        public void GetDataFromTwrKod(string _twrkod, bool CzyFoto)
        {
            var app = Application.Current as App;
            if (SettingsPage.SprConn())
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    SqlConnection connection = new SqlConnection
                    {
                        ConnectionString = "SERVER=" + app.Serwer +
                        ";DATABASE=" + app.BazaProd +
                        ";TRUSTED_CONNECTION=No;UID=" + app.User +
                        ";PWD=" + app.Password
                    };
                    connection.Open();
                    command.CommandText = "Select twr_kod, twr_nazwa, Twr_NumerKat twr_symbol, cast(twc_wartosc as decimal(5,2))cena " +
                        ",cast(sum(TwZ_Ilosc) as int)ilosc, twr_url,twr_ean " +
                        "from cdn.towary " +
                        "join cdn.TwrCeny on Twr_twrid = TwC_Twrid and TwC_TwrLp = 2 " +
                        "join cdn.TwrZasoby on Twr_twrid = TwZ_TwrId " +
                        "where twr_kod='" + _twrkod + "'" +
                        "group by twr_kod, twr_nazwa, Twr_NumerKat,twc_wartosc, twr_url,twr_ean";


                    SqlCommand query = new SqlCommand(command.CommandText, connection);
                    SqlDataReader rs;
                    rs = query.ExecuteReader();
                    if (rs.Read())
                    {
                        twrkod = Convert.ToString(rs["twr_kod"]);
                        stan_szt = Convert.ToString(rs["ilosc"]);
                        twr_url = Convert.ToString(rs["twr_url"]);
                        twr_nazwa = Convert.ToString(rs["twr_nazwa"]);
                        twr_symbol = Convert.ToString(rs["twr_symbol"]);
                        twr_ean = Convert.ToString(rs["twr_ean"]);
                    }
                    else
                    {
                        DisplayAlert("Uwaga", "Kod nie istnieje!", "OK");


                    }
                    rs.Close();
                    rs.Dispose();
                    connection.Close();

                }
                catch (Exception exception)
                {
                    DisplayAlert("Uwaga", exception.Message, "OK");
                }
            }
            else
            {
                DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");

                //return twrkod;
                //kodean.Text = twrkod;
                if (CzyFoto)
                {
                    foto.Source = twr_url;
                }
                else
                {
                    ean.Text = twr_ean;
                    symbol.Text = twr_symbol;
                    nazwa.Text = twr_nazwa;
                    foto.Source = twr_url;
                    stan.Text = "Stan : " + stan_szt;
                    ilosc.Focus();

                }
            }
        }
    }
}
