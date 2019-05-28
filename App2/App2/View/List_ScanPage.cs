using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
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
        private Entry entry_kodean;
        private Entry entry_ilosc;
        private Image img_foto;
        private Button btn_Skanuj;
        private Button btn_AddEanPrefix;
        private Button btn_Zapisz;
        private Int32 _gidnumer;
        private SQLiteAsyncConnection _connection;
        private string skanean;
        private Model.AkcjeNagElem _akcja;
        ZXingDefaultOverlay overlay;
        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        private ZXingScannerView zxing;


        public List_ScanPage(Model.AkcjeNagElem akcje) //edycja
        {
            this.Title = "Dodaj MM";

            _akcja = akcje;

            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;

            NavigationPage.SetHasNavigationBar(this, false);

            FlexLayout stackLayout = new FlexLayout();
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
                Device.OpenUri(new Uri(akcje.TwrUrl.Replace("Miniatury/", "")));
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
                
                FontAttributes=FontAttributes.Bold,
                TextColor =Color.Black
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
                BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}",
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
                        ile_zeskanowancyh += 1;
                        overlay.BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}";
                        DisplayAlert(null, $"Zeskanowanych szt : {ile_zeskanowancyh}", "OK");

                        entry_kodean.Text = ile_zeskanowancyh.ToString();

                    }
                    else
                    {

                        DisplayAlert(null, "Probujesz zeskanować inny model..", "OK");
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
            stackLayout.Padding = 8;
            stackLayout_gl.Children.Add(stackLayout);
            stackLayout_gl.Children.Add(open_url);

            Content = stackLayout_gl; 
        }


        int ile_zeskanowancyh = 0;
        private void Open_url_Clicked(object sender, EventArgs e)
        {
            //Device.OpenUri(new Uri(_MMElement.url.Replace("Miniatury/", "")));
            _akcja.TwrSkan = Convert.ToInt32(entry_kodean.Text);
            Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            if (ile_zeskanowancyh > 0)
                _akcja.TwrSkan = ile_zeskanowancyh;
            return base.OnBackButtonPressed();
        }

    }
}
