﻿using Microsoft.AppCenter.Crashes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using static Xamarin.Forms.Button;
using static Xamarin.Forms.Button.ButtonContentLayout;

namespace App2.View
{
    class RaportLista_AddTwrKod : ContentPage
    {

        private Label lbl_stan;
        private Label lbl_twrkod;
        private Label lbl_ean;
        private Label lbl_symbol;
        private Label lbl_nazwa;
        private Label lbl_cena;
        private Entry entry_kodean;
        private Entry entry_ilosc;
        private Image img_foto;
        //private Button btn_Skanuj;
        private Button btn_AddEanPrefix;
        private Button btn_Zapisz;
        private Int32 _gidnumer;
        private SQLiteAsyncConnection _connection;
        private Model.PrzyjmijMMClass _MMElement;
        private Model.AkcjeNagElem _akcja;
        string skanean;
        ZXingDefaultOverlay overlay;
        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;


        string twrkod;
        string stan_szt;
        string twr_url;
        string twr_nazwa;
        string twr_symbol;
        string twr_ean;
        string twr_cena;


        public RaportLista_AddTwrKod(int gidnumer) : base() //dodawamoe pozycji
        {
          
            if (SettingsPage.SelectedDeviceType == 1)
            {
                WidokAparat(gidnumer);
            }
            else
            {
                WidokSkaner(gidnumer);
            }
             

        }

        private void WidokSkaner(int gidnumer)
        {
            
            StackLayout stack_dane = new StackLayout();
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            var scrollView = new ScrollView();

            _gidnumer = gidnumer;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();

            NavigationPage.SetHasNavigationBar(this, false); 
     
            Label lbl_naglowek = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Dodawanie pozycji (raport przyjęć)",
                FontSize = 20,
                TextColor = Color.Bisque,
                BackgroundColor = Color.DarkCyan
            };
            AbsoluteLayout.SetLayoutBounds(lbl_naglowek, new Rectangle(0, 0, 1, .1));
            AbsoluteLayout.SetLayoutFlags(lbl_naglowek, AbsoluteLayoutFlags.All); 

            img_foto = new Image();
            img_foto.Aspect = Aspect.AspectFill;
            TapGestureRecognizer tapGesture= new TapGestureRecognizer();

            //todo : proba naprawy błędu
            tapGesture.Tapped += async (s, e) =>
            {
                try
                {
                    await Launcher.OpenAsync(twr_url.Replace("Miniatury/", "")); 
                }
                catch (Exception x)
                {
                    var properties = new Dictionary<string, string>
                    {
                        { "_gidnumer", _gidnumer.ToString() },
                        { "foto", twr_url}
                    };
                    Crashes.TrackError(x, properties);

                }
                finally
                {
                    await Launcher.OpenAsync(twr_url);
                }
            };

            img_foto.GestureRecognizers.Add(tapGesture);
            AbsoluteLayout.SetLayoutBounds(img_foto, new Rectangle(0, 0.1, 1, .5));
            AbsoluteLayout.SetLayoutFlags(img_foto, AbsoluteLayoutFlags.All);

             

            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;

            lbl_nazwa = new Label();
            lbl_nazwa.HorizontalOptions = LayoutOptions.Center;
            
            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;

            lbl_symbol = new Label();
            lbl_symbol.HorizontalOptions = LayoutOptions.Center;

            lbl_cena = new Label();
            lbl_cena.HorizontalOptions = LayoutOptions.Center;


            entry_kodean = new Entry()
            {
                //Keyboard = Keyboard.Text,
                Placeholder = "Wpisz EAN/kod ręcznie lub skanuj",
                Keyboard = Keyboard.Plain,
                //ReturnCommand = new Command(() => entry_ilosc.Focus()) 
            };
            entry_kodean.Unfocused += Kodean_Unfocused;
             
            entry_ilosc = new Entry()
            {
                Placeholder = "Wpisz Ilość",
                Keyboard = Keyboard.Telephone,
                HorizontalOptions = LayoutOptions.Center,
                ReturnType = ReturnType.Go,
                HorizontalTextAlignment=TextAlignment.Center,
            };

            entry_ilosc.Completed += (object sender, EventArgs e) =>
            {
                Zapisz();
            };           

            btn_Zapisz = new Button()
            {
                Text = "Zapisz pozycję",
                ImageSource = "save48x2.png",
                ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10) ,
                BorderColor = Color.DarkCyan,
                BorderWidth = 2,
                CornerRadius = 10,
            };
            btn_Zapisz.Clicked += Btn_Zapisz_Clicked;
            AbsoluteLayout.SetLayoutBounds(btn_Zapisz, new Rectangle(0.5, 1, .9, 50));
            AbsoluteLayout.SetLayoutFlags(btn_Zapisz, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional); 

            btn_AddEanPrefix = new Button()
            {
                Text = "+201000",
                BorderColor = Color.DarkCyan,
                BorderWidth = 2,
                CornerRadius = 20,
            };
            btn_AddEanPrefix.Clicked += Btn_AddEanPrefix_Clicked;
            AbsoluteLayout.SetLayoutBounds(btn_AddEanPrefix, new Rectangle(1, .82, .25, 50));
            AbsoluteLayout.SetLayoutFlags(btn_AddEanPrefix, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional); 
             

            stack_dane.Children.Add(lbl_nazwa);
            stack_dane.Children.Add(lbl_ean);
            stack_dane.Children.Add(lbl_cena);
            stack_dane.Children.Add(lbl_symbol);
            stack_dane.Children.Add(entry_ilosc);
            stack_dane.Children.Add(entry_kodean);
            AbsoluteLayout.SetLayoutBounds(stack_dane, new Rectangle(0, 1, 1, .45));
            AbsoluteLayout.SetLayoutFlags(stack_dane, AbsoluteLayoutFlags.All);
                        

            absoluteLayout.Children.Add(img_foto);
            absoluteLayout.Children.Add(lbl_naglowek); 
            absoluteLayout.Children.Add(stack_dane);
            absoluteLayout.Children.Add(btn_Zapisz);
            absoluteLayout.Children.Add(btn_AddEanPrefix);

            Content = absoluteLayout; 

            Appearing += (object sender, System.EventArgs e) => entry_kodean.Focus();
        }

        private void WidokAparat(int gidnumer)
        {
             
            SkanowanieEan();

            StackLayout stack_dane = new StackLayout();
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            var scrollView = new ScrollView();

            _gidnumer = gidnumer;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();

            NavigationPage.SetHasNavigationBar(this, false);



            Label lbl_naglowek = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Dodawanie pozycji (raport przyjęć)",
                FontSize = 20,
                TextColor = Color.Bisque,
                BackgroundColor = Color.DarkCyan
            };
            AbsoluteLayout.SetLayoutBounds(lbl_naglowek, new Rectangle(0, 0, 1, .1));
            AbsoluteLayout.SetLayoutFlags(lbl_naglowek, AbsoluteLayoutFlags.All);




            img_foto = new Image();
            img_foto.Aspect = Aspect.AspectFill;
            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                try
                {
                    await Launcher.OpenAsync(twr_url.Replace("Miniatury/", ""));
                }
                catch (Exception x)
                {
                    var properties = new Dictionary<string, string>
                    {
                        { "_gidnumer", _gidnumer.ToString() },
                        { "foto", twr_url}
                    };
                    Crashes.TrackError(x, properties);

                }
                finally
                {
                    await Launcher.OpenAsync(twr_url);
                }
            };
            img_foto.GestureRecognizers.Add(tapGesture);
            AbsoluteLayout.SetLayoutBounds(img_foto, new Rectangle(0, 0.1, 1, .5));
            AbsoluteLayout.SetLayoutFlags(img_foto, AbsoluteLayoutFlags.All);



            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;

            lbl_nazwa = new Label();
            lbl_nazwa.HorizontalOptions = LayoutOptions.Center;

            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;

            lbl_symbol = new Label();
            lbl_symbol.HorizontalOptions = LayoutOptions.Center;

            lbl_cena = new Label();
            lbl_cena.HorizontalOptions = LayoutOptions.Center;


            entry_kodean = new Entry()
            {
                //Keyboard = Keyboard.Text,
                Placeholder = "Wpisz EAN/kod ręcznie lub skanuj",
                Keyboard = Keyboard.Plain,
                //ReturnCommand = new Command(() => entry_ilosc.Focus())
            };
            entry_kodean.Unfocused += Kodean_Unfocused;

            entry_ilosc = new Entry()
            {
                Placeholder = "Wpisz Ilość",
                Keyboard = Keyboard.Telephone,
                HorizontalOptions = LayoutOptions.Center,
                ReturnType = ReturnType.Go,
                HorizontalTextAlignment = TextAlignment.Center,

            };
            entry_ilosc.Completed += (object sender, EventArgs e) =>
            {
                Zapisz();
            };
            //entry_ilosc.Keyboard = Keyboard.Text;



            btn_Zapisz = new Button()
            {
                Text = "Zapisz pozycję",
                ImageSource = "save48x2.png",
                ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10),
                BorderColor = Color.DarkCyan,
                BorderWidth = 2,
                CornerRadius = 10,
            };
            btn_Zapisz.Clicked += Btn_Zapisz_Clicked;
            AbsoluteLayout.SetLayoutBounds(btn_Zapisz, new Rectangle(0.5, 1, .9, 50));
            AbsoluteLayout.SetLayoutFlags(btn_Zapisz, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);



            btn_AddEanPrefix = new Button()
            {
                Text = "+201000",
                BorderColor = Color.DarkCyan,
                BorderWidth = 2,
                CornerRadius = 20,
            };
            btn_AddEanPrefix.Clicked += Btn_AddEanPrefix_Clicked;
            AbsoluteLayout.SetLayoutBounds(btn_AddEanPrefix, new Rectangle(1, .8, .25, 50));
            AbsoluteLayout.SetLayoutFlags(btn_AddEanPrefix, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);



            stack_dane.Children.Add(lbl_nazwa);
            stack_dane.Children.Add(lbl_ean);
            stack_dane.Children.Add(lbl_cena);
            stack_dane.Children.Add(lbl_symbol);
            stack_dane.Children.Add(entry_ilosc);
            stack_dane.Children.Add(entry_kodean);
            AbsoluteLayout.SetLayoutBounds(stack_dane, new Rectangle(0, 1, 1, .45));
            AbsoluteLayout.SetLayoutFlags(stack_dane, AbsoluteLayoutFlags.All);




            //stackLayout.VerticalOptions = LayoutOptions.EndAndExpand; //Center
            //stackLayout.Padding = new Thickness(15, 0, 15, 0);

            absoluteLayout.Children.Add(img_foto);
            absoluteLayout.Children.Add(lbl_naglowek);
            absoluteLayout.Children.Add(stack_dane);
            absoluteLayout.Children.Add(btn_Zapisz);
            absoluteLayout.Children.Add(btn_AddEanPrefix);

            Content = absoluteLayout;

             
        }

        private void Btn_AddEanPrefix_Clicked(object sender, EventArgs e)
        {

            UstawFocus();
        }

        void UstawFocus()
        {
            entry_kodean.Focused += (sender, args) =>
            {
                entry_kodean.Keyboard = Keyboard.Telephone;
                entry_kodean.Text = "201000";


            };
            entry_kodean.Focus();



        }

        public RaportLista_AddTwrKod(Model.PrzyjmijMMClass mmka) //edycja
        {
            this.Title = "Dodaj MM";

            _MMElement = mmka;


            NavigationPage.SetHasNavigationBar(this, false);

            StackLayout stackLayout = new StackLayout();
            StackLayout stackLayout_gl = new StackLayout();
            StackLayout stack_naglowek = new StackLayout();

            Label lbl_naglowek = new Label();
            lbl_naglowek.HorizontalOptions = LayoutOptions.CenterAndExpand;
            lbl_naglowek.VerticalOptions = LayoutOptions.Start;
            lbl_naglowek.Text = "Szczegóły pozycji";
            lbl_naglowek.FontSize = 20;
            lbl_naglowek.TextColor = Color.Bisque;
            lbl_naglowek.BackgroundColor = Color.DarkCyan;

            stack_naglowek.HorizontalOptions = LayoutOptions.FillAndExpand;
            stack_naglowek.VerticalOptions = LayoutOptions.Start;
            stack_naglowek.BackgroundColor = Color.DarkCyan;
            stack_naglowek.Children.Add(lbl_naglowek);

            stackLayout_gl.Children.Add(stack_naglowek);



            img_foto = new Image();
            img_foto.Source = mmka.url.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(mmka.url.Replace("Miniatury/", ""));
            };
            img_foto.GestureRecognizers.Add(tapGestureRecognizer);
            stackLayout.Children.Add(img_foto);
            //_gidnumer = mmka.gi;


            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;
            lbl_stan.Text = "Ilość : " + mmka.ilosc + " szt";

            stackLayout.Children.Add(lbl_stan);

            lbl_twrkod = new Label();
            lbl_twrkod.HorizontalOptions = LayoutOptions.Center;
            lbl_twrkod.Text = "Kod towaru : " + mmka.twrkod;

            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;
            lbl_ean.Text = "Ean : " + mmka.ean;

            entry_kodean = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Ean : " + mmka.ean,

            };

            lbl_symbol = new Label();
            lbl_symbol.HorizontalOptions = LayoutOptions.Center;
            lbl_symbol.Text = "Symbol : " + mmka.symbol;

            lbl_nazwa = new Label();
            lbl_nazwa.HorizontalOptions = LayoutOptions.Center;
            lbl_nazwa.Text = "Nazwa : " + mmka.nazwa;

            lbl_cena = new Label();
            lbl_cena.HorizontalOptions = LayoutOptions.Center;
            lbl_cena.Text = "Cena : " + mmka.cena + " Zł";


            Button open_url = new Button();
            open_url.Text = "Otwórz zdjęcie";
            open_url.CornerRadius = 15;

            open_url.Clicked += Open_url_Clicked;
            //open_url.BackgroundColor = Color.FromHex("#3CB371");
            //open_url.VerticalOptions = LayoutOptions.EndAndExpand;
            //open_url.Margin = new Thickness(15, 0, 15, 5);


            stackLayout.Children.Add(lbl_twrkod);
            stackLayout.Children.Add(lbl_nazwa);
            //stackLayout.Children.Add(lbl_ean);
            stackLayout.Children.Add(entry_kodean);
            stackLayout.Children.Add(lbl_symbol);
            stackLayout.Children.Add(lbl_cena);


            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(15, 0, 15, 0);
            stackLayout.Spacing = 8;
            stackLayout_gl.Children.Add(stackLayout);
            //stackLayout_gl.Children.Add(open_url);

            Content = stackLayout_gl;
            // GetDataFromTwrKod(mmka.twrkod);
            //entry_ilosc.Focus();
        }

        
        public RaportLista_AddTwrKod(Model.AkcjeNagElem akcje) //edycja
        {
            this.Title = "Dodaj MM";

            _akcja = akcje;

            ile = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile;

            NavigationPage.SetHasNavigationBar(this, false);

            StackLayout stackLayout = new StackLayout();
            StackLayout stackLayout_gl = new StackLayout();
            StackLayout stack_naglowek = new StackLayout();

            Label lbl_naglowek = new Label();
            lbl_naglowek.HorizontalOptions = LayoutOptions.CenterAndExpand;
            lbl_naglowek.VerticalOptions = LayoutOptions.Start;
            lbl_naglowek.Text = "Szczegóły pozycji";
            lbl_naglowek.FontSize = 20;
            lbl_naglowek.TextColor = Color.Bisque;
            lbl_naglowek.BackgroundColor = Color.DarkCyan;

            stack_naglowek.HorizontalOptions = LayoutOptions.FillAndExpand;
            stack_naglowek.VerticalOptions = LayoutOptions.Start;
            stack_naglowek.BackgroundColor = Color.DarkCyan;
            stack_naglowek.Children.Add(lbl_naglowek);

            stackLayout_gl.Children.Add(stack_naglowek);



            img_foto = new Image();
            img_foto.Source = akcje.TwrUrl.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(akcje.TwrUrl.Replace("Miniatury/", ""));
            };
            img_foto.GestureRecognizers.Add(tapGestureRecognizer);
            stackLayout.Children.Add(img_foto);
            //_gidnumer = mmka.gi;


            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;
            lbl_stan.Text = "Stan : " + akcje.TwrStan + " szt";
            lbl_stan.FontAttributes = FontAttributes.Bold;

            stackLayout.Children.Add(lbl_stan);

            lbl_twrkod = new Label();
            lbl_twrkod.HorizontalOptions = LayoutOptions.Center;
            lbl_twrkod.Text = "Kod towaru : " + akcje.TwrKod;

            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;
            lbl_ean.Text = akcje.TwrEan;

            entry_kodean = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                Text = akcje.TwrSkan == 0 ? "" : akcje.TwrSkan.ToString(),
                WidthRequest = 60,
                IsEnabled = false
            };

            lbl_symbol = new Label();
            lbl_symbol.HorizontalOptions = LayoutOptions.Center;
            lbl_symbol.Text = "Symbol : " + akcje.TwrSymbol;

            lbl_nazwa = new Label();
            lbl_nazwa.HorizontalOptions = LayoutOptions.Center;
            lbl_nazwa.Text = "Nazwa : " + akcje.TwrNazwa;

            lbl_cena = new Label();
            lbl_cena.HorizontalOptions = LayoutOptions.Center;
            lbl_cena.Text = "Cena : " + akcje.TwrCena + " Zł";


            Button open_url = new Button();
            open_url.Text = "Zacznij skanowanie";
            open_url.CornerRadius = 15;

            //open_url.Clicked += Open_url_Clicked;
            overlay = new ZXingDefaultOverlay
            {
                TopText = $"Skanowany : {akcje.TwrKod}",
                BottomText = $"Zeskanowanych szt : {ile}",
                AutomationId = "zxingDefaultOverlay",
               
                
            };

            var torch = new Switch
            {
            };

            torch.Toggled += delegate
            {
                scanPage.ToggleTorch();
            };

            overlay.Children.Add(torch);
            open_url.Clicked += async delegate {
                scanPage = new ZXingScannerPage(
                    new ZXing.Mobile.MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 3000 }, overlay);
                    scanPage.DefaultOverlayShowFlashButton = true;
                    scanPage.OnScanResult += (result) =>
                    Device.BeginInvokeOnMainThread(() => { 
                    skanean = result.Text;

                        if (skanean == lbl_ean.Text)
                        {
                            ile += 1;
                            overlay.BottomText = $"Zeskanowanych szt : {ile}";
                            DisplayAlert(null, $"Zeskanowanych szt : {ile}", "OK");

                            entry_kodean.Text = ile.ToString();

                        }
                        else {

                            DisplayAlert(null,"Probujesz zeskanować inny model..", "OK");
                        }
                    });
                await Navigation.PushModalAsync(scanPage);
            };
            //open_url.BackgroundColor = Color.FromHex("#3CB371");
            open_url.VerticalOptions = LayoutOptions.EndAndExpand;
            //open_url.Margin = new Thickness(15, 0, 15, 5);


            stackLayout.Children.Add(lbl_twrkod);
            stackLayout.Children.Add(lbl_nazwa);
            stackLayout.Children.Add(lbl_ean);
            stackLayout.Children.Add(entry_kodean);
            stackLayout.Children.Add(lbl_symbol);
            stackLayout.Children.Add(lbl_cena);


            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(15, 0, 15, 0);
            stackLayout.Spacing = 8;
            stackLayout_gl.Children.Add(stackLayout);
            stackLayout_gl.Children.Add(open_url);

            Content = stackLayout_gl;
            // GetDataFromTwrKod(mmka.twrkod);
            //entry_ilosc.Focus();
        }


        int ile=0 ;
        private void Open_url_Clicked(object sender, EventArgs e)
        {
            //Device.OpenUri(new Uri(_MMElement.url.Replace("Miniatury/", "")));
            _akcja.TwrSkan = Convert.ToInt32(entry_kodean.Text);
            Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            if(ile>0)
            _akcja.TwrSkan = ile;

            return base.OnBackButtonPressed();
        }

        private void Kodean_Unfocused(object sender, FocusEventArgs e)
        {
            if(!string.IsNullOrEmpty(entry_kodean.Text))
            pobierztwrkod(entry_kodean.Text);
            if(!string.IsNullOrEmpty(twrkod))
                entry_ilosc.Focus();
        }

        private void Btn_Update_Clicked(object sender, EventArgs e)
        {
            if (entry_ilosc.Text != null && entry_kodean.Text != null)
            {
                if (Int32.Parse(entry_ilosc.Text) > Int32.Parse(stan_szt))
                {
                    DisplayAlert(null, "Wpisana ilość przekracza stan {}", "OK");
                }
                else
                {
                    Model.DokMM dokMM = new Model.DokMM();
                    dokMM.gidnumer = _gidnumer;
                    dokMM.twrkod = entry_kodean.Text;
                    dokMM.szt = Convert.ToInt32(entry_ilosc.Text);
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
               

        public async void Zapisz()
        {

            if (!string.IsNullOrEmpty(entry_ilosc.Text) && !string.IsNullOrEmpty(entry_kodean.Text) && !string.IsNullOrEmpty(twr_nazwa))
            {

                try
                {
                    Model.RaportListaMM dokMM = new Model.RaportListaMM();
                    var listaZMM = Model.PrzyjmijMMLista.przyjmijMMListas.Where(n => n.GIDdokumentuMM == _gidnumer).ToList();
                    dokMM.twrkod = entry_kodean.Text;
                    dokMM.ilosc_OK = Convert.ToInt16(entry_ilosc.Text);
                    dokMM.nazwa = lbl_nazwa.Text;
                    int maxid = await _connection.ExecuteScalarAsync<int>("select ifnull(max(IdElement),0) maxid from RaportListaMM where GIDdokumentuMM =?", _gidnumer);


                    var wynik = await _connection.QueryAsync<Model.RaportListaMM>("select * from RaportListaMM where GIDdokumentuMM = ? and twrkod=?", _gidnumer, entry_kodean.Text);
                    if (wynik.Count > 0)
                    {
                        var wpis = wynik[0];

                        int suma = wynik[0].ilosc_OK + Convert.ToInt16(entry_ilosc.Text);

                        await DisplayAlert("Uwaga", String.Format("Kod {0} już jest na liście : {1} szt - pozycja zostanie zaktualizowana, razem : {2}", dokMM.twrkod, wpis.ilosc_OK,suma), "OK");
                        wpis.ilosc_OK = suma;
                        await _connection.UpdateAsync(wpis);
                    }
                    else
                    {
                        dokMM.IdElement = (maxid) + 1;
                        dokMM.GIDdokumentuMM = _gidnumer;
                        dokMM.DatadokumentuMM = listaZMM[0].DatadokumentuMM;
                        dokMM.nrdokumentuMM = listaZMM[0].nrdokumentuMM;
                        dokMM.XLGIDMM = listaZMM[0].XLGIDMM;
                        //dokMM.nazwa = 
                        // dokMM.RaportListaMMs.Add(dokMM);
                        Model.RaportListaMM.RaportListaMMs.Add(dokMM);
                        await _connection.InsertAsync(dokMM);
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert(null, ex.Message, "OK");
                    var prop = new Dictionary<string, string>
                    {
                        { "operator", View.StartPage.user},
                        
                    };
                    Crashes.TrackError(ex, prop);
                }


                #region MyRegion
                // await Navigation.PushModalAsync(new RaportListaElementow(dokMM));  
                // int IleIstnieje = dokMM.SaveElement(dokMM);

                //if (IleIstnieje > 0)
                //{
                //    var odp = await DisplayAlert("UWAGA!", "Dodawany kod już znajduje się na liście. Chcesz zsumować ilości?", "TAK", "NIE");
                //    if (odp)
                //    {
                //        int suma = Int32.Parse(entry_ilosc.Text) + IleIstnieje;
                //        if (suma > Int32.Parse(stan_szt))
                //        {
                //            await DisplayAlert(null, "Łączna ilość przekracza stan ", "OK");
                //            return;
                //        }
                //        else
                //        {
                //            //Model.DokMM dokMM = new Model.DokMM();

                //            dokMM.twrkod = entry_kodean.Text;
                //            dokMM.szt = suma;// Convert.ToInt32(ilosc.Text);
                //            dokMM.UpdateElement(dokMM);
                //            dokMM.getElementy(_gidnumer);
                //        }
                //    }
                //    else
                //    {

                //        await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
                //    }
                //} 
                #endregion


                // OnDoPush(); 
                await Navigation.PopModalAsync();
                // SkanowanieEan();  
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie uzupełniono wszystkich pól!", "OK");
            }
        }
        private void Btn_Zapisz_Clicked(object sender, EventArgs e)
        {

            Zapisz();
            Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
            {
                if (SettingsPage.SelectedDeviceType == 1)
                {
                    SkanowanieEan();
                }
                 
                
                return false;
            });
        } 

        private async void SkanowanieEan()
        {
            if (SettingsPage.SprConn())
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {

                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>() {
                //ZXing.BarcodeFormat.EAN_8,
                ZXing.BarcodeFormat.EAN_13,
                //ZXing.BarcodeFormat.CODE_128,
                //ZXing.BarcodeFormat.CODABAR,
                ZXing.BarcodeFormat.CODE_39,
                },
                    // CameraResolutionSelector = availableResolutions => {

                    //     foreach (var ar in availableResolutions)
                    //     {
                    //         Console.WriteLine("Resolution: " + ar.Width + "x" + ar.Height);
                    //     }
                    //     return availableResolutions[0];
                    // },
                    //DelayBetweenContinuousScans=3000

                };

                opts.TryHarder = true;

                //zxing = new ZXingScannerView
                //{

                //    IsScanning = false,
                //    IsTorchOn = true,
                //    IsAnalyzing = false,
                //    AutomationId = "zxingDefaultOverlay",//zxingScannerView
                //    Opacity = 22,
                //    Options = opts
                //};

                var torch = new Switch
                {

                };

                torch.Toggled += delegate
                {
                    scanPage.ToggleTorch();

                };

                // scanPage.ToggleTorch();

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
                //customOverlay.Children.Add(btn_Manual);

                // var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                grid.Children.Add(Overlay);
                Overlay.Children.Add(torch);
                Overlay.BindingContext = Overlay;

                scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
                {
                    DefaultOverlayTopText = "Zeskanuj kod ",
                    //DefaultOverlayBottomText = " Skanuj kod ";
                    DefaultOverlayShowFlashButton = true,
                    IsTorchOn = true,  //////dodane

                };


                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;
                    scanPage.AutoFocus();
                    scanPage.IsTorchOn = true; //dodane
                    scanPage.HasTorch = true; //dodane
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                        {
                            if (scanPage.IsScanning)
                            {
                                scanPage.AutoFocus();
                                scanPage.IsTorchOn = true;
                            }

                            return true;
                        });
                        Navigation.PopModalAsync();
                        pobierztwrkod(result.Text);
                        entry_ilosc.Focus();
                    });
                };
                await Navigation.PushModalAsync(scanPage); /////!!!!!!!


                Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                {
                    scanPage.IsTorchOn = true;
                    torch.IsToggled = true;

                    return false;
                });
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            }
        }

        //private void Abort_Clicked(object sender, EventArgs e)
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        this.scanPage.IsScanning = false;
        //        await Navigation.PopModalAsync();
        //    });
        //}

        private void Btn_Skanuj_Clicked(object sender, EventArgs e)
        {
            SkanowanieEan();
        }         

        public async void pobierztwrkod(string _ean)
        {
            var app = Application.Current as App;

            try
            {
                if (SettingsPage.SprConn())
                {
                    if (!string.IsNullOrEmpty(_ean) || _ean != "201000")
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
                            command.CommandText = $"Select twr_kod, twr_nazwa, Twr_NumerKat twr_symbol, cast(twc_wartosc as decimal(5,2))cena " +
                                ",cast(sum(TwZ_Ilosc) as int)ilosc, case when len(twr_kod) > 5 and len(twr_url)> 5 then " +
                                    "replace(twr_url, substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4), " +
                                " substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4) + 'Miniatury/') else twr_kod end as twr_url,twr_ean " +
                                "from cdn.towary " +
                                "join cdn.TwrCeny on Twr_twrid = TwC_Twrid and TwC_TwrLp = 2 " +
                                "left join cdn.TwrZasoby on Twr_twrid = TwZ_TwrId " +
                                "where twr_ean='" + _ean + "' or twr_kod='" + _ean + "'" +
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
                                twr_cena = Convert.ToString(rs["cena"]);


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
                                    twr_cena = dane[0].cena;

                                }
                                else

                                    await DisplayAlert("Uwaga", "Kod nie istnieje!", "OK");
                            }
                            rs.Close();
                            rs.Dispose();
                            connection.Close();

                            entry_kodean.Text = twrkod;
                            lbl_ean.Text = twr_ean;
                            lbl_symbol.Text = twr_symbol;
                            lbl_nazwa.Text = twr_nazwa;
                            lbl_cena.Text = twr_cena;
                            lbl_stan.Text = "Stan : " + stan_szt;
                            if (!string.IsNullOrEmpty(twr_url))
                                img_foto.Source = twr_url.Replace("Miniatury/", ""); //twr_url;

                        }
                        catch (Exception)
                        {
                            await DisplayAlert("Uwaga", "Nie znaleziono towaru", "OK");
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
                        twr_cena = dane[0].cena;

                        entry_kodean.Text = twrkod;
                        lbl_ean.Text = twr_ean;
                        lbl_symbol.Text = twr_symbol;
                        lbl_nazwa.Text = twr_nazwa;
                        lbl_cena.Text = twr_cena;
                        lbl_stan.Text = "Stan : " + stan_szt;
                        if (!string.IsNullOrEmpty(twr_url))
                            img_foto.Source = twr_url.Replace("Miniatury/", ""); //twr_url;

                    }
                    else

                        await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
                }
            }
            catch (Exception s)
            {

                await DisplayAlert(null, s.Message, "OK");
            }

            //return twrkod;
            
        }

        public void GetDataFromTwrKod(string _twrkod)
        {
            var app = Application.Current as App;
            if (SettingsPage.SprConn()&&!string.IsNullOrEmpty(_twrkod))
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
            }
            //return twrkod;
            //kodean.Text = twrkod;
            lbl_ean.Text = twr_ean;
            lbl_symbol.Text = twr_symbol;
            lbl_nazwa.Text = twr_nazwa;
            lbl_stan.Text = "Stan : " + stan_szt;
            img_foto.Source = twr_url;
            entry_ilosc.Focus();
        }
    }
}

