
using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.Services;
using Azure;
using FFImageLoading.Forms;
using Microsoft.AppCenter.Analytics;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
namespace App2.View
{
    public class List_ScanPage : ContentPage
    {
        private Label lbl_stan;
        private Label lbl_twrkod;
        private Label lbl_ean;
        private Label lbl_symbol;
        private Label lbl_nazwa;
        private Label lbl_cena;
        private Label lbl_cena1;
        private Entry entry_skanowanaIlosc;
        private Entry entry_EanSkaner;


        private SQLiteAsyncConnection _connection;
        private string skanean;
        private Model.AkcjeNagElem _akcja;
        ZXingDefaultOverlay overlay;
        ZXingScannerPage scanPage;

        int ile_zeskanowancyh = 0;

        CPCLConst cpclConst;
         
        int IResult;
        bool CanPrint;

        int koniecLinii;
        int polozenie;
        bool printSukces;
        List<AkcjeNagElem> listaToSend;
        int iResult;
        private short magnumer;
        Array Controls;

        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);
        ServiceDokumentyApi serwisAPi;

        public List_ScanPage(AkcjeNagElem akcje) //edycja
        {
            this.Title = "Dodaj MM";

            _akcja = akcje;

            Controls = new[] { "Dodaj mm", "Przegladaj", "Tworz" };
            serwisAPi = new ServiceDokumentyApi();

            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);



        }

        protected async override void OnAppearing()
        {
            base.OnAppearing(); 
           

            if (SettingsPage.SelectedDeviceType == 1)
            {
                WidokAparat();
                Analytics.TrackEvent("przecena tryb aparat");
            }
            else
            {
                WidokSkaner();
            }
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                DependencyService.Get<IAppVersionProvider>().ShowLong("Sprawdź status drukarki i kolor etykiet");

            if (_akcja.TwrCena == 0)
                await DisplayAlert("Uwaga", "sprawdź konfigurację ceny w ustawieniach", "OK");

            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
            {
                //cpclConst = SettingsPage.cpclConst;
                cpclConst = new CPCLConst();
                await ConnectPrinter();
                CanPrint = SettingsPage.CzyDrukarkaOn;
            }

        }

        void WidokAparat()
        {

            //if (List_AkcjeView.TypAkcji.Contains("Przecena"))
            //deviceListe();

            //cpclConst = new CPCLConst();
            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);



            StackLayout stack_dane = new StackLayout();
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout layout2 = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(layout2, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(layout2, AbsoluteLayoutFlags.All);

            Label lbl_naglowek = new Label();
            lbl_naglowek.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl_naglowek.VerticalOptions = LayoutOptions.Start;
            lbl_naglowek.HorizontalTextAlignment = TextAlignment.Center;
            lbl_naglowek.Text = "Szczegóły pozycji";
            lbl_naglowek.FontSize = 20;
            lbl_naglowek.TextColor = Color.Bisque;
            lbl_naglowek.BackgroundColor = Color.DarkCyan;
            AbsoluteLayout.SetLayoutBounds(lbl_naglowek, new Rectangle(0, 0, 1, .1));
            AbsoluteLayout.SetLayoutFlags(lbl_naglowek, AbsoluteLayoutFlags.All);


            //img_foto = new Image();
            //img_foto.Source = _akcja.TwrUrl.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(_akcja.TwrUrl.Replace("Miniatury/", "").Replace("small", "large"));
            };
            //img_foto.GestureRecognizers.Add(tapGestureRecognizer);

            var cachedImage = new CachedImage()
            {
                //HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Center, 
                CacheDuration = TimeSpan.FromMinutes(2),
                //DownsampleToViewSize = true,
                RetryCount = 1,
                RetryDelay = 100,
                BitmapOptimizations = false,
                ErrorPlaceholder = "NotSended.png",
                Source = StartPage.CheckInternetConnection() ? _akcja.TwrUrl.Replace("Miniatury/", "").Replace("small", "home") : _akcja.TwrUrl

            };

            cachedImage.GestureRecognizers.Add(tapGestureRecognizer);


            AbsoluteLayout.SetLayoutBounds(cachedImage, new Rectangle(0, 0.09, 1, .4));
            AbsoluteLayout.SetLayoutFlags(cachedImage, AbsoluteLayoutFlags.All);

            //_gidnumer = mmka.gi;


            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;
            lbl_stan.Text = "Stan : " + _akcja.TwrStan + " szt";
            lbl_stan.FontAttributes = FontAttributes.Bold;


            lbl_twrkod = new Label();
            lbl_twrkod.HorizontalOptions = LayoutOptions.Center;
            lbl_twrkod.Text = "Kod towaru : " + _akcja.TwrKod;

            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;
            lbl_ean.Text = _akcja.TwrEan;
            lbl_ean.TextDecorations = TextDecorations.Underline;
            var tapCopyLabelEan = new TapGestureRecognizer();
            tapCopyLabelEan.Tapped += async (s, e) =>
            {
                await Clipboard.SetTextAsync(_akcja.TwrEan);
                DependencyService.Get<IAppVersionProvider>().ShowLong("Skopiowano Ean");
                //await DisplayAlert(null, "skopiowano ean", "ok");
            };
            lbl_ean.GestureRecognizers.Add(tapCopyLabelEan);


            entry_skanowanaIlosc = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                Text = _akcja.TwrSkan == 0 ? "" : _akcja.TwrSkan.ToString(),
                WidthRequest = 60,
                Placeholder = "Ilość",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                entry_skanowanaIlosc.IsReadOnly = true;
            //    entry_skanowanaIlosc.IsReadOnly = false;

            entry_skanowanaIlosc.Focused += (object sender, FocusEventArgs e) =>
            {
                if (string.IsNullOrEmpty(entry_skanowanaIlosc.Text))
                    SprawdzEanApart();
            };


            lbl_symbol = new Label();
            lbl_symbol.HorizontalOptions = LayoutOptions.Center;
            lbl_symbol.Text = "Symbol : " + _akcja.TwrSymbol;

            lbl_nazwa = new Label();
            lbl_nazwa.HorizontalOptions = LayoutOptions.Center;
            lbl_nazwa.Text = "Nazwa : " + _akcja.TwrNazwa;

            lbl_cena = new Label();
            lbl_cena.HorizontalOptions = LayoutOptions.Center;
            lbl_cena.Text = "Cena : " + _akcja.TwrCena + " Zł";

            lbl_cena1 = new Label();
            lbl_cena1.HorizontalOptions = LayoutOptions.Center;
            lbl_cena1.Text = "Cena pierwsza : " + _akcja.TwrCena1 + " Zł";

            Button open_url = new Button();
            //open_url.Text = List_AkcjeView.TypAkcji.Contains("Przerzut") ? "Zapisz" : "Zacznij skanowanie";
            open_url.Text = List_AkcjeView.TypAkcji.Contains("Przecena") ? "Zacznij skanowanie" : "Zapisz";
            open_url.CornerRadius = 15;
            open_url.Clicked += Open_url_Clicked;
            open_url.VerticalOptions = LayoutOptions.EndAndExpand;
            AbsoluteLayout.SetLayoutBounds(open_url, new Rectangle(0.5, .99, .8, 50));
            AbsoluteLayout.SetLayoutFlags(open_url, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);

        



            Grid grid = new Grid()
            {
                Margin = new Thickness(0, 0, 10, 120),
                RowSpacing = 10
            };
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });


            var stack1 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.End
            };
            var label1 = new Label()
            {
                Text = "Przeglądaj MMki",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold

            };
            var frame1 = new Button()
            {
                BackgroundColor = Color.FromRgb(32, 178, 170),
                CornerRadius = 25,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                ImageSource = "icons8_analyze_48.9"
            };
            frame1.Clicked += BtnBrowseMM_Clicked;
            stack1.Children.Add(label1);
            stack1.Children.Add(frame1);
            grid.Children.Add(stack1);
            Grid.SetRow(stack1, 0);

            var stack2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.End
            };
            var label2 = new Label()
            {
                Text = "Dodaj do MM",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold
            };
            var frame2 = new Button()
            {
                BackgroundColor = Color.FromRgb(32, 178, 170),
                CornerRadius = 25,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                ImageSource = "icons8_add_property_48.9"
            };
            frame2.Clicked += BtnAddToMM_Clicked;
            stack2.Children.Add(label2);
            stack2.Children.Add(frame2);
            grid.Children.Add(stack2);

            Grid.SetRow(stack2, 1);

            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(1, 1, -1, -1));
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.PositionProportional);


            Button frame_btn = new Button()
            {
                BackgroundColor = Color.DarkCyan,
                CornerRadius = 30,
                WidthRequest = 60,
                HeightRequest = 60,
                Margin = new Thickness(0, 0, 10, 50),
                Text = "+",
                Padding = new Thickness(0, 0, 0, 0),
                FontSize = 35,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,

            };
            //AbsoluteLayout.SetLayoutBounds(frame_btn, new Rectangle(1, .70, .20, 50));
            AbsoluteLayout.SetLayoutBounds(frame_btn, new Rectangle(1, 1, -1, -1));
            AbsoluteLayout.SetLayoutFlags(frame_btn, AbsoluteLayoutFlags.PositionProportional);
            //AbsoluteLayout.SetLayoutFlags(frame_btn, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            var tapp = new TapGestureRecognizer();
            frame_btn.Clicked += async (sender, e) =>
            {
                //await((Frame)sender).ScaleTo(.5, 50, Easing.Linear);
                //await Task.Delay(100);
                //await((Frame)sender).ScaleTo(1, 50, Easing.Linear);

                if (!stack1.IsVisible)
                {
                    frame_btn.Text = "-";
                    stack_dane.Opacity = .2;
                    stack1.IsVisible = true;
                    await stack1.TranslateTo(0, 0, 100);
                    await stack1.TranslateTo(0, -10, 100);
                    await stack1.TranslateTo(0, 0, 100);

                    stack2.IsVisible = true;
                    await stack2.TranslateTo(0, 0, 100);
                    await stack2.TranslateTo(0, -10, 100);
                    await stack2.TranslateTo(0, 0, 100);
                }
                else
                {
                    await (frame1).ScaleTo(0, 50, Easing.Linear);
                    await (frame2).ScaleTo(0, 50, Easing.Linear);
                    stack1.IsVisible = false;
                    stack2.IsVisible = false;
                    stack_dane.Opacity = 1;
                    frame_btn.Text = "+";
                    await (frame1).ScaleTo(1, 50, Easing.Linear);
                    await (frame2).ScaleTo(1, 50, Easing.Linear);
                }



            };


            Button enterEanButton = new Button()
            {
                Text = "Druk AWARYJNY",
                CornerRadius = 15,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontSize = 12
            };
            enterEanButton.Clicked += EnterEan_Clicked;

            AbsoluteLayout.SetLayoutBounds(enterEanButton, new Rectangle(1, .82, .25, 50));
            AbsoluteLayout.SetLayoutFlags(enterEanButton, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);

 
            Button addToMM = new Button()
            {
                Text = "Dodaj do MMki",
                CornerRadius = 60,
                WidthRequest = 120,
                FontSize = 10,
                BackgroundColor = Color.DarkCyan,
                TextColor = Color.AntiqueWhite
            };
            AbsoluteLayout.SetLayoutBounds(addToMM, new Rectangle(1, .70, .20, 50));
            AbsoluteLayout.SetLayoutFlags(addToMM, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            stack_dane.Children.Add(lbl_twrkod);
            stack_dane.Children.Add(lbl_nazwa);
            stack_dane.Children.Add(lbl_ean);
            stack_dane.Children.Add(lbl_stan);
            stack_dane.Children.Add(entry_skanowanaIlosc);
            //stack_dane.Children.Add(entry_EanSkaner);
            stack_dane.Children.Add(lbl_symbol);
            stack_dane.Children.Add(lbl_cena);
            stack_dane.Children.Add(lbl_cena1);
            AbsoluteLayout.SetLayoutBounds(stack_dane, new Rectangle(0, 1, 1, .55));
            AbsoluteLayout.SetLayoutFlags(stack_dane, AbsoluteLayoutFlags.All);




            //absoluteLayout.Children.Add(img_foto);
            absoluteLayout.Children.Add(cachedImage);
            absoluteLayout.Children.Add(lbl_naglowek);
            absoluteLayout.Children.Add(stack_dane);
            absoluteLayout.Children.Add(open_url);
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                absoluteLayout.Children.Add(enterEanButton);
            //absoluteLayout.Children.Add(addToMM);
            if (List_AkcjeView.TypAkcji.Contains("Przerzuty") || List_AkcjeView.TypAkcji.Contains("Zwrot"))
            {
                absoluteLayout.Children.Add(frame_btn);
                absoluteLayout.Children.Add(grid);
            }

             

            Content = absoluteLayout;

        }

        private void backAction_Clicked(object sender, EventArgs e)
        {
            if (entry_EanSkaner.IsFocused)
                entry_EanSkaner.Unfocus();

             Navigation.PopAsync();

        }

        void WidokSkaner()
        {

            //var scrollView = new ScrollView();

            //cpclConst = new CPCLConst();
            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);



            StackLayout stack_dane = new StackLayout();
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout layout2 = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(layout2, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(layout2, AbsoluteLayoutFlags.All);

            Label lbl_naglowek = new Label();
            lbl_naglowek.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl_naglowek.VerticalOptions = LayoutOptions.Start;
            lbl_naglowek.HorizontalTextAlignment = TextAlignment.Center;
            lbl_naglowek.Text = "Szczegóły pozycji";
            lbl_naglowek.FontSize = 20;
            lbl_naglowek.TextColor = Color.Bisque;
            lbl_naglowek.BackgroundColor = Color.DarkCyan;
            AbsoluteLayout.SetLayoutBounds(lbl_naglowek, new Rectangle(0, 0, 1, .1));
            AbsoluteLayout.SetLayoutFlags(lbl_naglowek, AbsoluteLayoutFlags.All);


            //img_foto = new Image();
            //img_foto.Source = _akcja.TwrUrl.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(_akcja.TwrUrl.Replace("Miniatury/", "").Replace("small", "large"));
            };
            //img_foto.GestureRecognizers.Add(tapGestureRecognizer);

            var cachedImage = new CachedImage()
            {
                //HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Center, 
                CacheDuration = TimeSpan.FromMinutes(2),
                //DownsampleToViewSize = true,
                RetryCount = 1,
                RetryDelay = 100,
                BitmapOptimizations = false,
                ErrorPlaceholder = "NotSended.png",
                Source = StartPage.CheckInternetConnection() ? _akcja.TwrUrl.Replace("Miniatury/", "").Replace("small", "home") : _akcja.TwrUrl

            };

            cachedImage.GestureRecognizers.Add(tapGestureRecognizer);


            AbsoluteLayout.SetLayoutBounds(cachedImage, new Rectangle(0, 0.09, 1, .4));
            AbsoluteLayout.SetLayoutFlags(cachedImage, AbsoluteLayoutFlags.All);

            //_gidnumer = mmka.gi;


            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;
            lbl_stan.Text = "Stan : " + _akcja.TwrStan + " szt";
            lbl_stan.FontAttributes = FontAttributes.Bold;


            lbl_twrkod = new Label();
            lbl_twrkod.HorizontalOptions = LayoutOptions.Center;
            lbl_twrkod.Text = "Kod towaru : " + _akcja.TwrKod;

            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;
            lbl_ean.Text = _akcja.TwrEan;
            lbl_ean.TextDecorations = TextDecorations.Underline;
            var tapCopyLabelEan = new TapGestureRecognizer();
            tapCopyLabelEan.Tapped += async (s, e) =>
            {
                await Clipboard.SetTextAsync(_akcja.TwrEan);
                DependencyService.Get<IAppVersionProvider>().ShowLong("Skopiowano Ean");
                //await DisplayAlert(null, "skopiowano ean", "ok");
            };
            lbl_ean.GestureRecognizers.Add(tapCopyLabelEan);


            entry_skanowanaIlosc = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                Text = _akcja.TwrSkan == 0 ? "" : _akcja.TwrSkan.ToString(),
                WidthRequest = 60,
                Placeholder = "Ilość",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };
            if (List_AkcjeView.TypAkcji.Contains("Przecena") || List_AkcjeView.TypAkcji.Contains("Zwrot"))
                entry_skanowanaIlosc.IsReadOnly = true;





            lbl_symbol = new Label();
            lbl_symbol.HorizontalOptions = LayoutOptions.Center;
            lbl_symbol.Text = "Symbol : " + _akcja.TwrSymbol;

            lbl_nazwa = new Label();
            lbl_nazwa.HorizontalOptions = LayoutOptions.Center;
            lbl_nazwa.Text = "Nazwa : " + _akcja.TwrNazwa;

            lbl_cena = new Label();
            lbl_cena.HorizontalOptions = LayoutOptions.Center;
            lbl_cena.Text = "Cena : " + _akcja.TwrCena + " Zł";

            lbl_cena1 = new Label();
            lbl_cena1.HorizontalOptions = LayoutOptions.Center;
            lbl_cena1.Text = "Cena pierwsza : " + _akcja.TwrCena1 + " Zł";

            Button open_url = new Button();
            //open_url.Text = List_AkcjeView.TypAkcji.Contains("Przerzut") ? "Zapisz" : "Zacznij skanowanie";
            open_url.Text = List_AkcjeView.TypAkcji.Contains("Przecena") ? "Zacznij skanowanie" : "Zapisz";
            open_url.CornerRadius = 15;
            open_url.Clicked += Open_url_Clicked;
            open_url.VerticalOptions = LayoutOptions.EndAndExpand;
            AbsoluteLayout.SetLayoutBounds(open_url, new Rectangle(0.3, .99, .8, 50));
            AbsoluteLayout.SetLayoutFlags(open_url, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            Button back = new Button();
            back.CornerRadius = 20; 
            back.BorderColor= Color.Black;
            back.Text = "<<<";
            back.CornerRadius = 15;
            back.Clicked += backAction_Clicked;
            back.VerticalOptions = LayoutOptions.EndAndExpand;
            AbsoluteLayout.SetLayoutBounds(back, new Rectangle(1, .99, .15, 50));
            AbsoluteLayout.SetLayoutFlags(back, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);

            Grid grid = new Grid()
            {
                Margin = new Thickness(0, 0, 10, 120),
                RowSpacing = 10
            };
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });


            var stack1 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.End
            };
            var label1 = new Label()
            {
                Text = "Przeglądaj MMki",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold

            };
            var frame1 = new Button()
            {
                BackgroundColor = Color.FromRgb(32, 178, 170),
                CornerRadius = 25,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                ImageSource = "icons8_analyze_48.9"
            };
            frame1.Clicked += BtnBrowseMM_Clicked;
            stack1.Children.Add(label1);
            stack1.Children.Add(frame1);
            grid.Children.Add(stack1);
            Grid.SetRow(stack1, 0);

            var stack2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.End
            };
            var label2 = new Label()
            {
                Text = "Dodaj do MM",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold
            };
            var frame2 = new Button()
            {
                BackgroundColor = Color.FromRgb(32, 178, 170),
                CornerRadius = 25,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                ImageSource = "icons8_add_property_48.9"
            };
            frame2.Clicked += BtnAddToMM_Clicked;
            stack2.Children.Add(label2);
            stack2.Children.Add(frame2);
            grid.Children.Add(stack2);

            Grid.SetRow(stack2, 1);

            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(1, 1, -1, -1));
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.PositionProportional);


            Button frame_btn = new Button()
            {
                BackgroundColor = Color.DarkCyan,
                CornerRadius = 30,
                WidthRequest = 60,
                HeightRequest = 60,
                Margin = new Thickness(0, 0, 10, 50),
                Text = "+",
                Padding = new Thickness(0, 0, 0, 0),
                FontSize = 35,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,

            };
            //AbsoluteLayout.SetLayoutBounds(frame_btn, new Rectangle(1, .70, .20, 50));
            AbsoluteLayout.SetLayoutBounds(frame_btn, new Rectangle(1, 1, -1, -1));
            AbsoluteLayout.SetLayoutFlags(frame_btn, AbsoluteLayoutFlags.PositionProportional);
            //AbsoluteLayout.SetLayoutFlags(frame_btn, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            var tapp = new TapGestureRecognizer();
            frame_btn.Clicked += async (sender, e) =>
            {
                //await((Frame)sender).ScaleTo(.5, 50, Easing.Linear);
                //await Task.Delay(100);
                //await((Frame)sender).ScaleTo(1, 50, Easing.Linear);

                if (!stack1.IsVisible)
                {
                    frame_btn.Text = "-";
                    stack_dane.Opacity = .2;
                    stack1.IsVisible = true;
                    await stack1.TranslateTo(0, 0, 100);
                    await stack1.TranslateTo(0, -10, 100);
                    await stack1.TranslateTo(0, 0, 100);

                    stack2.IsVisible = true;
                    await stack2.TranslateTo(0, 0, 100);
                    await stack2.TranslateTo(0, -10, 100);
                    await stack2.TranslateTo(0, 0, 100);
                }
                else
                {
                    await (frame1).ScaleTo(0, 50, Easing.Linear);
                    await (frame2).ScaleTo(0, 50, Easing.Linear);
                    stack1.IsVisible = false;
                    stack2.IsVisible = false;
                    stack_dane.Opacity = 1;
                    frame_btn.Text = "+";
                    await (frame1).ScaleTo(1, 50, Easing.Linear);
                    await (frame2).ScaleTo(1, 50, Easing.Linear);
                }



            };


            Button enterEanButton = new Button()
            {
                Text = "Druk AWARYJNY",
                CornerRadius = 15,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontSize = 12
            };
            enterEanButton.Clicked += EnterEan_Clicked;

            AbsoluteLayout.SetLayoutBounds(enterEanButton, new Rectangle(1, .82, .25, 50));
            AbsoluteLayout.SetLayoutFlags(enterEanButton, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            entry_EanSkaner = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = SettingsPage.OnAlfaNumeric ? Keyboard.Default : Keyboard.Numeric,
                WidthRequest = 180,
                //Placeholder = View.SettingsPage.CzyDrukarkaOn ? "Wpisz/zeskanuj Ean" : "Drukarka nie połączona",
                Placeholder = "Wpisz/zeskanuj Ean",
                TextColor = Color.Black,

            };

            if (List_AkcjeView.TypAkcji.Contains("Przerzuty") || List_AkcjeView.TypAkcji.Contains("Zwrot"))
            {
                entry_EanSkaner.ReturnCommand = new Command(() => setFocusEntryIlosc(entry_EanSkaner.Text));
            }
            else
            {
                entry_EanSkaner.IsReadOnly = !View.SettingsPage.CzyDrukarkaOn;
                entry_EanSkaner.ReturnCommand = new Command(async () => await zapiszdrukuj());

            }

            Button addToMM = new Button()
            {
                Text = "Dodaj do MMki",
                CornerRadius = 60,
                WidthRequest = 120,
                FontSize = 10,
                BackgroundColor = Color.DarkCyan,
                TextColor = Color.AntiqueWhite
            };
            AbsoluteLayout.SetLayoutBounds(addToMM, new Rectangle(1, .70, .20, 50));
            AbsoluteLayout.SetLayoutFlags(addToMM, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            stack_dane.Children.Add(lbl_twrkod);
            stack_dane.Children.Add(lbl_nazwa);
            stack_dane.Children.Add(lbl_ean);
            stack_dane.Children.Add(lbl_stan);
            stack_dane.Children.Add(entry_skanowanaIlosc);
            stack_dane.Children.Add(lbl_symbol);
            stack_dane.Children.Add(lbl_cena);
            stack_dane.Children.Add(lbl_cena1);
            stack_dane.Children.Add(entry_EanSkaner);
            AbsoluteLayout.SetLayoutBounds(stack_dane, new Rectangle(0, 1, 1, .55));
            AbsoluteLayout.SetLayoutFlags(stack_dane, AbsoluteLayoutFlags.All);




            //absoluteLayout.Children.Add(img_foto);
            absoluteLayout.Children.Add(cachedImage);
            absoluteLayout.Children.Add(lbl_naglowek);
            absoluteLayout.Children.Add(stack_dane);
            absoluteLayout.Children.Add(open_url);
            absoluteLayout.Children.Add(back);
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                absoluteLayout.Children.Add(enterEanButton);
            //absoluteLayout.Children.Add(addToMM);
            if (List_AkcjeView.TypAkcji.Contains("Przerzuty") || List_AkcjeView.TypAkcji.Contains("Zwrot"))
            {
                absoluteLayout.Children.Add(frame_btn);
                absoluteLayout.Children.Add(grid);
            }






            //scrollView.Content = stackLayout_gl;

            Content = absoluteLayout;

            //Appearing += (object sender, System.EventArgs e) =>  entry_EanSkaner.Focus();
            Appearing += List_ScanPage_Appearing;

        }

        private void List_ScanPage_Appearing(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entry_EanSkaner.Text))
            {
                entry_EanSkaner.Focus();
            }
        }

        private void setFocusEntryIlosc(string entryEan)
        {
            if (entryEan == _akcja.TwrEan)
            {
                entry_skanowanaIlosc.IsReadOnly = false;
                entry_skanowanaIlosc.Focus();
                entry_skanowanaIlosc.CursorPosition = entry_skanowanaIlosc.Text.Length + 1;
            }
            else
                DisplayAlert(null, "Błędny ean", "OK");
        }

        private async void BtnBrowseMM_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new StartCreateMmPage());
        }

        private async void BtnAddToMM_Clicked(object sender, EventArgs e)
        {
            try
            {
                // List<string> ListaMMWBuforze = new List<string>();
                List<string> naJakichMM = new List<string>();
                var listmmBufor = await serwisAPi.GetDokAll(Model.ApiModel.GidTyp.Mm, false);
                //var listaMM = dokmm.getMMki().Where(c => c.statuss == 0);
                int gidnumerMM;
                int totalTwrIlosc = 0;
                //var listaMM = Model.DokMM.dokMMs.Where(c =>c.statuss ==0);
                int iloscZeskanowana;


                var czyPoprawnaIlosc = Int32.TryParse(entry_skanowanaIlosc.Text, out iloscZeskanowana);
                if (czyPoprawnaIlosc && iloscZeskanowana > 0)
                {
                    var listaMMzKodem = await serwisAPi.GetDokWithElementByTwrkod(_akcja.TwrKod);
                    if (listaMMzKodem.IsSuccessful || listaMMzKodem.HttpStatusCode == HttpStatusCode.NotFound)
                    {
                        //var listaIstniejacych = dokMM.ListaIstniejacych(_akcja.TwrKod);
                        var listaIstniejacych = listaMMzKodem.Data;

                        totalTwrIlosc = serwisAPi.TotalTwrIloscFromAllDoks(listaIstniejacych);

                        foreach (var mm in listaIstniejacych)
                        {
                            naJakichMM.Add(string.Format($"{mm.MagKod} - {mm.Opis.Replace("Pakował(a):", "")} : {mm.DokElements.First().TwrIlosc} szt"));
                        }

                        //jeśli skanowa > to co już jest i 
                        if ((iloscZeskanowana + totalTwrIlosc) > (_akcja.TwrStan))
                        {

                            if (listaIstniejacych.Count > 0)
                                await DisplayActionSheet("Ilość przekracza stan- występuje już :", "OK", null, naJakichMM.ToArray());
                            else
                                await DisplayAlert("Uwaga", "Wpisana Ilość przekracza stan", "OK");

                        }
                        else
                        {
                            if ((iloscZeskanowana + totalTwrIlosc <= _akcja.TwrStan) && listaIstniejacych.Count > 0)
                                await DisplayActionSheet("Model występuje już na mmkach:", "OK", null, naJakichMM.ToArray());


                            var listaBuforNazwy = serwisAPi.ListaMMBufor(listmmBufor);


                            if (listaBuforNazwy.Count > 0)
                            {
                                var odp = await DisplayActionSheet("Wybierz MM:", "Wyjdź", null, listaBuforNazwy.ToArray());
                                if (odp != "Wyjdź")
                                {
                                    var gdzieCut = odp.IndexOf(')');
                                    var cutNrmmki = odp.Substring(0, gdzieCut);
                                    Int32.TryParse(cutNrmmki, out gidnumerMM);

                                    ZapiszPozycje(gidnumerMM, _akcja, iloscZeskanowana, odp);
                                    entry_skanowanaIlosc.Text = (totalTwrIlosc + iloscZeskanowana).ToString();
                                }

                            }
                            else
                            {
                                await DisplayAlert(null, "Brak MM w buforze - utwórz nową", "OK");
                            }

                        }
                    }

                }
                else
                    await DisplayAlert(null, "Błędna ilosc", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert(null, "Coś poszło nie tak..Spróbuj ponownie", "OK");
            }


        }

        public async void ZapiszPozycje(int mmGidnumer, AkcjeNagElem twrInfo, int iloscSkanowana, string opis)
        {

            CreateDokElementDto elementDto = new CreateDokElementDto()
            {
                DokTyp = (int)GidTyp.Mm,
                TwrIlosc = iloscSkanowana,
                TwrKod = twrInfo.TwrKod,
                TwrNazwa = twrInfo.TwrNazwa
            };

            var apiResponse = await serwisAPi.SaveElement(elementDto, mmGidnumer);

            //todo : dodaj sprawdzenie ilosci dodanej
            if (apiResponse.ConflictInformation != null)
            {
                var conflictInfo = apiResponse.ConflictInformation;

                var isAddMore = await DisplayAlert(
                    "Konflikt",
                    $"Towar {conflictInfo.TwrKod} znajduje się już na liście : {conflictInfo.ExistingQuantity} sztuk. Czy chcesz zsumoawć ilości?",
                    "Tak",
                    "Nie"
                );

                if (isAddMore)
                {
                    // Wykonaj dodatkowe akcje, na przykład dodaj więcej towaru
                    var updatedQuantity = conflictInfo.ExistingQuantity + conflictInfo.AttemptedToAddQuantity;


                    // Możesz teraz wysłać updatedQuantity z powrotem do serwera do aktualizacji
                    var resposne = await serwisAPi.UpadteElement(updatedQuantity, mmGidnumer, conflictInfo.IdElement);
                    if (resposne.IsSuccessful)
                    {
                        await DisplayAlert("Dodano..", $"{conflictInfo.AttemptedToAddQuantity} szt, razem {updatedQuantity}szt", "OK");
                    }
                }
                else
                {
                    // Anuluj operację lub zareaguj w inny sposób na decyzję użytkownika
                }
            }
            else if (!apiResponse.IsSuccessful)
            {
                // Obsługa innych błędów
                await DisplayAlert("Błąd", apiResponse.ErrorMessage, "OK");
            }


            //if (IleIstnieje > 0)
            //{
            //    var odp = await DisplayAlert("UWAGA!", "Dodawany kod już znajduje się na liście. Chcesz zsumować ilości?", "TAK", "NIE");
            //    if (odp)
            //    {
            //        int suma = (ilosc) + IleIstnieje;
            //        if (suma > (stan_szt))
            //        {
            //            await DisplayAlert(null, "Łączna ilość przekracza stan ", "OK");
            //            return;
            //        }
            //        else
            //        {
            //            //Model.DokMM dokMM = new Model.DokMM();
            //            dokMM.gidnumer = mmGidnumer;
            //            dokMM.twrkod = twrKod;
            //            dokMM.szt = suma;// Convert.ToInt32(ilosc.Text);
            //            dokMM.UpdateElement(dokMM);
            //            dokMM.getElementy(mmGidnumer);
            //            var opis2 = opis.Substring(opis.IndexOf(')') + 1);
            //            //var oooo = opis.Substring(opis.IndexOf(')') + 1, opis.Length - opis.IndexOf(')') + 2);
            //            await DisplayAlert("Dodano..", $"..{ilosc} szt do {opis2}", "OK");

            //        }
            //    }
            //    else
            //    {
            //        await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
            //    }

            //}
            //else
            //{
            //    var opis2 = opis.Substring(opis.IndexOf(')') + 1);
            //    await DisplayAlert("Dodano..", $"..{dokMM.szt} szt do {opis2}", "OK");
            //}

        }

        async Task zapiszdrukuj()
        {
            if (_akcja.TwrEan == entry_EanSkaner.Text)
            {
                if (View.SettingsPage.CzyDrukarkaOn)
                    ile_zeskanowancyh += 1;

                if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                {
                    if (CanPrint)
                        if (await PrintCommand())
                        {
                            //ile_zeskanowancyh += 1;
                            await ZapiszDoSQLite();
                            entry_skanowanaIlosc.Text = ile_zeskanowancyh.ToString();
                            entry_EanSkaner.Text = "";
                            if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh + 1))
                                entry_EanSkaner.Focus();
                        }
                        else
                            ile_zeskanowancyh -= 1;
                    //DisplayAlert(null, "Drukuje..", "OK");

                }
                else
                {
                    await DisplayAlert("Uwaga", "Zeskanowano więcej niż jest na stanie", "OK");
                    ile_zeskanowancyh -= 1;
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Skanujesz model inny niż otwarty!", "OK");
                entry_EanSkaner.Text = "";
            }
        }

        async void SprawdzEanApart()
        {

            string odpowiedz = await DisplayPromptAsync("Potwierdź skanowany model", null, "OK", "Skanuj", "Wprowadź Ean:");
            if (odpowiedz == null)
            {
                overlay = new ZXingDefaultOverlay
                {
                    TopText = $"Skanowany : {_akcja.TwrKod}",

                    AutomationId = "zxingDefaultOverlay",
                };

                var torch = new Xamarin.Forms.Switch
                {
                };

                torch.Toggled += delegate
                {
                    scanPage.ToggleTorch();
                };



                try
                {

                    overlay.Children.Add(torch);

                    scanPage = new ZXingScannerPage(
                        new ZXing.Mobile.MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 3000 }, overlay);
                    scanPage.DefaultOverlayShowFlashButton = true;
                    scanPage.OnScanResult += (result) =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            odpowiedz = result.Text;

                            if (odpowiedz == lbl_ean.Text)
                            {
                                entry_skanowanaIlosc.Focus();
                                Navigation.PopModalAsync();
                            }
                            else
                            {
                                DisplayAlert(null, "Probujesz zeskanować inny model..", "OK");
                                entry_skanowanaIlosc.Unfocus();
                            }
                        });
                    await Navigation.PushModalAsync(scanPage);

                }
                catch (Exception x)
                {
                    System.Diagnostics.Debug.WriteLine(x.Message);
                }

            }
            else
            {
                if (odpowiedz == lbl_ean.Text)
                {
                    entry_skanowanaIlosc.IsEnabled = true;

                }
                else
                {
                    await DisplayAlert(null, "Probujesz zeskanować inny model..", "OK");
                    entry_skanowanaIlosc.Unfocus();
                }
            }

        }

        private async void EnterEan_Clicked(object sender, EventArgs e)
        {
            try
            {

                string odpowiedz = await DisplayPromptAsync("Druk bez zliczania sztuk", "( awaryjny )", "Drukuj", "Anuluj", "Wprowadź Ean:");


                if (!string.IsNullOrEmpty(odpowiedz))
                {
                    if (odpowiedz != "")
                    {
                        if (_akcja.TwrEan == (odpowiedz))
                        {
                            //string ile = odpowiedz.Text.Substring(odpowiedz.Text.IndexOf(",", 0) + 1, 1);

                            if (CanPrint)
                                await PrintCommand();
                        }
                        else
                        {
                            await DisplayAlert("Uwaga", "Podany Ean nie pasuje", "OK");
                        }
                    }
                }
            }
            catch (Exception x)
            {
                await DisplayAlert(null, x.Message, "OK"); ;
            }

        }

        private async void SkanujAparat(sbyte typDevice)//DRUKARKA
        {
            if (typDevice == 1)
            {

                overlay = new ZXingDefaultOverlay
                {
                    TopText = $"Skanowany : {_akcja.TwrKod}",
                    BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}/{_akcja.TwrStan}",
                    AutomationId = "zxingDefaultOverlay",


                };

                var torch = new Xamarin.Forms.Switch
                {
                };

                torch.Toggled += delegate
                {
                    scanPage.ToggleTorch();
                };



                try
                {

                    overlay.Children.Add(torch);

                    scanPage = new ZXingScannerPage(
                        new ZXing.Mobile.MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 3000 }, overlay);
                    scanPage.DefaultOverlayShowFlashButton = true;
                    scanPage.OnScanResult += (result) =>
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            skanean = result.Text;

                            if (skanean == lbl_ean.Text)
                            {
                                ile_zeskanowancyh += 1;
                                if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                                {
                                    //DisplayAlert(null, $"Zeskanowanych szt : {ile_zeskanowancyh}", "OK");
                                    if (CanPrint)

                                        if (await PrintCommand())
                                        {
                                            //ile_zeskanowancyh += 1;
                                            await ZapiszDoSQLite();
                                            entry_skanowanaIlosc.Text = ile_zeskanowancyh.ToString();


                                        }
                                        else
                                            ile_zeskanowancyh -= 1;

                                }
                                else
                                {
                                    await DisplayAlert("Uwaga", "Zeskanowano więcej niż jest na stanie", "OK");
                                    ile_zeskanowancyh -= 1;
                                }

                                overlay.BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}/{_akcja.TwrStan}";

                            }
                            else
                            {

                                await DisplayAlert(null, "Probujesz zeskanować inny model..", "OK");
                            }
                        });
                    await Navigation.PushModalAsync(scanPage);

                }
                catch (Exception x)
                {
                    System.Diagnostics.Debug.WriteLine(x.Message);
                }
            }
        }

        private async Task ConnectPrinter()
        {
            //try
            //{
            var app = Application.Current as App;
            SettingsPage._cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);

            try
            {
                if (!SettingsPage.CzyDrukarkaOn)
                {
                    //string adresDrukarki = string.IsNullOrEmpty(SettingsPage.editAddress.Text) ? "00:00:00:00" : SettingsPage.editAddress.Text;
                    string adresDrukarki = string.IsNullOrEmpty(app.Drukarka) ? "00:00:00:00" : app.Drukarka;
                    IResult = await SettingsPage._cpclPrinter.connect(adresDrukarki);
                    if (IResult == cpclConst.LK_SUCCESS)
                    {
                        SettingsPage.CzyDrukarkaOn = true;
                        CanPrint = true;
                        await DisplayAlert(null, "Połączono z drukarką", "OK");
                        entry_EanSkaner.IsReadOnly = false;

                    }
                    else
                    {
                        CanPrint = false;

                        ErrorStatusDisp("Błąd  drukarki", IResult);
                        SettingsPage.CzyDrukarkaOn = false;

                    }

                }
                else
                {

                }
            }
            catch (Exception s)
            {
                var tst = s.Message;
                await DisplayAlert(null, "Błąd połączenia z drukarką", "OK");
            }


        }

        public async Task<bool> PrintCommand(string ile = null)
        {
            int drukSzt;
            string typCodeEan = "";
            int przesuniecieDlaCode128 = 0;
            //_cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2); 

            if (ile != null)
                drukSzt = Convert.ToInt16(ile);
            else
                drukSzt = 1;


            _akcja.TwrCena30 = _akcja.TwrCena30 < _akcja.TwrCena ? _akcja.TwrCena : _akcja.TwrCena30;

            await printSemaphore.WaitAsync();
            try
            {
                if (SettingsPage.CzyCenaPierwsza)
                    _akcja.TwrCena = _akcja.TwrCena1;

                //var cenaZl = _akcja.TwrCena.Substring(0, _akcja.TwrCena.IndexOf(".", 0));
                //var cenaGr = _akcja.TwrCena.Substring(_akcja.TwrCena.IndexOf(".", 0) + 1, 2);

                int cenaZl = (int)_akcja.TwrCena; // część całkowita
                int cenaGr = (int)Math.Round((_akcja.TwrCena - cenaZl) * 100);
                string cenaGrFormatted = cenaGr.ToString("00");

                switch (cenaZl.ToString().Length)
                {
                    case 1:
                        polozenie = 125;

                        break;

                    case 2:
                        polozenie = 110;

                        break;

                    case 3:
                        polozenie = 85;

                        break;

                    default:
                        polozenie = 100;
                        break;
                }


                switch (_akcja.TwrCena1.ToString().Length)
                {
                    case 4:

                        koniecLinii = 130;
                        break;

                    case 5:

                        koniecLinii = 150;
                        break;

                    case 6:

                        koniecLinii = 170;
                        break;

                    default:
                        polozenie = 150;
                        break;
                }
                //= (_akcja.TwrCena1.Length <= 5) ? 155 : 165;

                int polozeniePLN = polozenie + 1 + 52 * cenaZl.ToString().Length;

                string twr_nazwa = (_akcja.TwrNazwa.Length > 20 ? _akcja.TwrNazwa.Substring(0, 20) : _akcja.TwrNazwa);

                //string procent = Convert.ToInt16((Double.Parse(_akcja.TwrCena.Replace(".", ",")) / Double.Parse(_akcja.TwrCena1.Replace(".", ",")) - 1) * 100).ToString();
                //var prr = double.IsInfinity((Double.Parse(_akcja.TwrCena.Replace(".", ",")) / Double.Parse(_akcja.TwrCena1.Replace(".", ",")) - 1) * 100) ? 0 : (Double.Parse(_akcja.TwrCena.Replace(".", ",")) / Double.Parse(_akcja.TwrCena1.Replace(".", ",")) - 1) * 100;
                var prr = double.IsInfinity(((double)_akcja.TwrCena / (double)_akcja.TwrCena1 - 1) * 100) ? 0 : (_akcja.TwrCena / _akcja.TwrCena1 - 1) * 100;
                string procent = Convert.ToInt16(prr).ToString();

                if (List_AkcjeView.TypAkcji.Contains("Zmiana"))//biała przecena
                    await SettingsPage._cpclPrinter.setForm(0, 200, 200, 270, 350, drukSzt);
                else
                    await SettingsPage._cpclPrinter.setForm(0, 200, 200, 370, 350, drukSzt);

                await SettingsPage._cpclPrinter.setBarCodeText(7, 0, 0);
                await SettingsPage._cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);

                // Set the code page of font number(7) 
                await SettingsPage._cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);

                if (SettingsPage.OnAlfaNumeric)
                {
                    typCodeEan = cpclConst.LK_CPCL_BCS_128;
                    przesuniecieDlaCode128 = 60;
                }
                else
                {
                    typCodeEan = cpclConst.LK_CPCL_BCS_EAN13;
                    przesuniecieDlaCode128 = 80;
                }



                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 5, _akcja.TwrKod, 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 30, twr_nazwa, 0);
                await SettingsPage._cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, typCodeEan, 1,
                        cpclConst.LK_CPCL_BCS_0RATIO, 40, przesuniecieDlaCode128, 55, _akcja.TwrEan, 0);

                if (!List_AkcjeView.TypAkcji.Contains("Zmiana"))
                {
                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 50, 135, _akcja.TwrCena1.ToString(), 0);
                    await SettingsPage._cpclPrinter.printLine(40, 170, koniecLinii, 145, 10);
                    if (Convert.ToInt16(procent) < -15)
                        await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 45, 245, $"~{procent}%", 0);

                }
                int YpolozenieCeny;
                int YpolozeniePLN;
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 1, polozenie, 160, cenaZl, 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 158, 160, cenaGr, 0);
                //190
                if (List_AkcjeView.TypAkcji.Contains("Zmiana"))
                {
                    YpolozenieCeny = 115;
                    YpolozeniePLN = 180;
                }
                else
                {
                    YpolozenieCeny = 177;
                    YpolozeniePLN = 240;
                }

                await SettingsPage._cpclPrinter.setConcat(cpclConst.LK_CPCL_CONCAT, polozenie, YpolozenieCeny);
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 3, 0, cenaZl.ToString());
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 0, cenaGr.ToString());//góra grosze
                await SettingsPage._cpclPrinter.resetConcat();
                //await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 45, cenaGr); dół
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, polozeniePLN, YpolozeniePLN, "PLN", 0);

                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_0, 0, 100, 120, "najnizsza cena z 30 dni", 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_0, 0, 200, 135, "przed obnizka", 0);//old value 140
                if (_akcja.TwrCena30 > 0 && _akcja.TwrCena30 < _akcja.TwrCena1)
                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 200, 150, $"{_akcja.TwrCena30}pln", 0);

                await SettingsPage._cpclPrinter.printForm();



                iResult = await SettingsPage._cpclPrinter.printResults();
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iResult = await SettingsPage._cpclPrinter.printStatus();
                        //Debug.WriteLine("PrinterStatus = " + iResult);
                        break;
                    default:
                        //Debug.WriteLine("PrinterResults = " + iResult);
                        break;
                }
                if (iResult != cpclConst.LK_SUCCESS)
                {
                    ErrorStatusDisp("Wydruk nieudany", iResult);
                    printSukces = false;
                }
                else
                {
                    //await DisplayAlert("Printing Result", "Printing success", "OK");

                    printSukces = true;

                }
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                await DisplayAlert("Exception", e.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }

            return printSukces;


        }

        async void ErrorStatusDisp(string strStatus, int errCode)
        {
            string errMsg = string.Empty;

            if (errCode > 0)
            {
                if ((errCode & cpclConst.LK_STS_CPCL_BATTERY_LOW) > 0)
                    errMsg += "Battery Low\n";
                if ((errCode & cpclConst.LK_STS_CPCL_COVER_OPEN) > 0)
                    errMsg += "Cover Open\n";
                if ((errCode & cpclConst.LK_STS_CPCL_PAPER_EMPTY) > 0)
                    errMsg += "Paper Empty\n";
                if ((errCode & cpclConst.LK_STS_CPCL_POWEROFF) > 0)
                    errMsg += "Power OFF\n";
                if ((errCode & cpclConst.LK_STS_CPCL_TIMEOUT) > 0)
                    errMsg += "Timeout\n";


                await DisplayAlert(strStatus, errMsg, "OK");
            }
            else
            {
                errMsg += "Nie znaleziono drukarki\n";
                await DisplayAlert(strStatus, "Return value = " + errMsg, "OK");
            }
        }

        private bool CzyMniejszeNStan(int stan, int skan)
        {
            if (skan <= stan)
                return true;
            else return false;
        }

        public async Task ZapiszDoSQLite()
        {
            AkcjeNagElem akcjeNagElem = new AkcjeNagElem();
            akcjeNagElem.AkN_GidNumer = _akcja.AkN_GidNumer;
            akcjeNagElem.TwrKod = _akcja.TwrKod;
            akcjeNagElem.TwrGrupa = _akcja.TwrGrupa;
            akcjeNagElem.TwrDep = _akcja.TwrDep;
            akcjeNagElem.TwrCena = _akcja.TwrCena;
            akcjeNagElem.TwrEan = _akcja.TwrEan;
            akcjeNagElem.TwrSymbol = _akcja.TwrSymbol;
            akcjeNagElem.TwrUrl = _akcja.TwrUrl;
            akcjeNagElem.TwrGidNumer = _akcja.TwrGidNumer;
            var wynik = await _connection.QueryAsync<AkcjeNagElem>("select * from AkcjeNagElem where AkN_GidNumer = ? and TwrKod=?", akcjeNagElem.AkN_GidNumer, akcjeNagElem.TwrKod);

            if (wynik.Count > 0)
            {
                var wpis = wynik[0];
                wpis.TwrSkan = ile_zeskanowancyh;
                _akcja.TwrSkan = ile_zeskanowancyh;
                wpis.SyncRequired = true;
                await _connection.UpdateAsync(wpis);

            }
            else
            {
                akcjeNagElem.TwrSkan = ile_zeskanowancyh;
                akcjeNagElem.SyncRequired=true;
                _akcja.TwrSkan = ile_zeskanowancyh;
                await _connection.InsertAsync(akcjeNagElem);

            }
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (_akcja.IsSendData)
            {
                // Wyświetl wskaźnik postępu 

                Task.Run(async () =>
                {
                    try
                    {
                        await SendDataSkan();
                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Błąd", $"Błąd połączenia - nie wysłano danych do centrali\nZrób to na oknie z podsumowaniem", "OK");
                        });
                    }
                });

                // Pozwól użytkownikowi wrócić
      
            }
        }

        //protected override bool OnBackButtonPressed()
        //{
          
        //    // listaToSend.Clear();
        //    if (_akcja.IsSendData)
        //    {
        //        // Wyświetl wskaźnik postępu 

        //        Task.Run(async () =>
        //        {
        //            try
        //            {
        //                await SendDataSkan();
        //            }
        //            catch (Exception ex)
        //            {
        //                Device.BeginInvokeOnMainThread(async () =>
        //                {
        //                    await DisplayAlert("Błąd", $"Błąd połączenia - nie wysłano danych do centrali\nZrób to na oknie z podsumowaniem", "OK");
        //                });
        //            }
        //        });

        //        // Pozwól użytkownikowi wrócić
        //        return false;
        //    }
        //    return base.OnBackButtonPressed();
        //}

        private async Task SendDataSkan()
        {

            listaToSend = new List<AkcjeNagElem>()
            {
                new AkcjeNagElem
                {
                     AkN_GidNumer = _akcja.AkN_GidNumer,
                     TwrKod = _akcja.TwrKod,
                     TwrGrupa = _akcja.TwrGrupa,
                     TwrDep = _akcja.TwrDep,
                     TwrCena = _akcja.TwrCena,
                     TwrEan = _akcja.TwrEan,
                     TwrSymbol = _akcja.TwrSymbol,
                     TwrUrl = _akcja.TwrUrl,
                     TwrGidNumer = _akcja.TwrGidNumer,
                     TwrSkan=_akcja.TwrSkan,
                     TwrStan = _akcja.TwrStan
                }

            };



            var wynikList = await _connection.QueryAsync<AkcjeNagElem>("select * from AkcjeNagElem where AkN_GidNumer = ? and TwrKod=?", _akcja.AkN_GidNumer, _akcja.TwrKod);

            AkcjeNagElem wpisKlasa = null;

            if (wynikList.Count > 0)
            {
                wpisKlasa = wynikList[0];

                wpisKlasa.SyncRequired = true;
                await _connection.UpdateAsync(wpisKlasa);

            }
            else
            {
                _akcja.SyncRequired = true;
                await _connection.InsertAsync(_akcja);
                wpisKlasa = _akcja;
            }

            // Jeśli jest połączenie, próbuj wysłać dane:
            if (await SettingsPage.SprConn())
            {
                var magGidnumer = (Application.Current as App).MagGidNumer;

                if (magGidnumer == 0)
                {
                    ServicePrzyjmijMM api = new ServicePrzyjmijMM();
                    var magazyn = await api.GetSklepMagNumer();
                    magGidnumer = (short)magazyn.Id;
                }

                if (listaToSend[0].TwrSkan > 0)
                {
                    string ase_operator = View.LoginLista._user + " " + View.LoginLista._nazwisko;
                    var odp = await App.TodoManager.InsertDataSkan(listaToSend, magGidnumer, ase_operator);

                    if (odp.Any())
                    {
                        // Jeśli dane zostały pomyślnie wysłane, oznacz je jako "wysłane" w SQLite.
                        wpisKlasa.SyncRequired = false;
                        await _connection.UpdateAsync(wpisKlasa);
                    }
                    else
                    {
                        await DisplayAlert(null, "Błąd synchronizacji z centrala", "OK");
                    }
                }
            }

        }

        //TODO popraw focus - po zapisaniu podnosi si e okno do entry


        private async void Open_url_Clicked(object sender, EventArgs e)
        {
            //cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);
            if (SettingsPage.SelectedDeviceType != 1 && string.IsNullOrEmpty(entry_EanSkaner.Text))
                entry_EanSkaner.Focus();
            try
            {
                if (!List_AkcjeView.TypAkcji.Contains("Przecena"))
                {

                    bool CzyWpisanoIlosc = Int32.TryParse(entry_skanowanaIlosc.Text, out ile_zeskanowancyh);

                    //ile_zeskanowancyh = Convert.ToInt32(entry_kodean.Text);

                    if (CzyWpisanoIlosc)
                        if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                        {
                            await ZapiszDoSQLite();
                            if (ile_zeskanowancyh > 0)
                                _akcja.TwrSkan = ile_zeskanowancyh;
                            if (_akcja.IsSendData)
                                await SendDataSkan();
                            await Navigation.PopAsync();
                        }
                        else await DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");
                    else await DisplayAlert("Uwaga", "Błędna wartość", "OK");


                }
                else
                {
 

                    if (!List_AkcjeView.TypAkcji.Contains("Przecena"))
                        SkanujAparat(SettingsPage.SelectedDeviceType);
                    else if (List_AkcjeView.TypAkcji.Contains("Przecena") && CanPrint == false)
                        await DisplayAlert(null, "Do przeceny wymagana drukarka", "OK");
                    else
                        SkanujAparat(SettingsPage.SelectedDeviceType);

                }
            }
            catch (Exception s)
            {
                await DisplayAlert(null, s.Message, "OK");
            }

        }






    }
}
