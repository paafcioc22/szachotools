using App2.Model;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
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
        //private Entry entry_ilosc;
        private Image img_foto;
        //private Button btn_Skanuj;
        //private Button btn_AddEanPrefix;
        //private Button btn_Zapisz;
        //private Int32 _gidnumer;
        private SQLiteAsyncConnection _connection;
        private string skanean;
        private Model.AkcjeNagElem _akcja;
        ZXingDefaultOverlay overlay;
        //ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        //private ZXingScannerView zxing;
        int ile_zeskanowancyh = 0;
        private ISewooXamarinCPCL _cpclPrinter;
        CPCLConst cpclConst;

     

        string drukarka; 

        public List_ScanPage(Model.AkcjeNagElem akcje) //edycja
        {
            this.Title = "Dodaj MM";
            //_blueToothService = DependencyService.Get<IBlueToothService>();
            _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService();
            cpclConst = new CPCLConst();
            _akcja = akcje;


           

            deviceListe();
           ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
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
            open_url.Text = List_AkcjeView.TypAkcji.Contains("Przerzut")?"Zapisz": "Zacznij skanowanie";
            open_url.CornerRadius = 15;

            
            open_url.Clicked += Open_url_Clicked;

            //overlay = new ZXingDefaultOverlay
            //{
            //    TopText = $"Skanowany : {akcje.TwrKod}",
            //    BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}",
            //    AutomationId = "zxingDefaultOverlay",


            //};

            //var torch = new Switch
            //{
            //};

            //torch.Toggled += delegate
            //{
            //    scanPage.ToggleTorch();
            //};



            //try
            //{



            //    overlay.Children.Add(torch);
            //    open_url.Clicked += async delegate
            //    {
            //        scanPage = new ZXingScannerPage(
            //            new ZXing.Mobile.MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 3000 }, overlay);
            //        scanPage.DefaultOverlayShowFlashButton = true;
            //        scanPage.OnScanResult += (result) =>
            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            skanean = result.Text;

            //            if (skanean == lbl_ean.Text)
            //            {
            //                ile_zeskanowancyh += 1;
            //                overlay.BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}";
            //                DisplayAlert(null, $"Zeskanowanych szt : {ile_zeskanowancyh}", "OK");
            //                PrintCommand();
                            
            //                Zapisz();
            //                entry_kodean.Text = ile_zeskanowancyh.ToString();

            //            }
            //            else
            //            {

            //                DisplayAlert(null, "Probujesz zeskanować inny model..", "OK");
            //            }
            //        });
            //        await Navigation.PushModalAsync(scanPage);
            //    };
            //}
            //catch (Exception x)
            //{
            //    System.Diagnostics.Debug.WriteLine(x.Message);
            //} 
            open_url.VerticalOptions = LayoutOptions.EndAndExpand; 


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
            stackLayout_gl.Padding = new Thickness(15);

            Content = stackLayout_gl; 
        }


        private async void Skanuj()
        {
            overlay = new ZXingDefaultOverlay
            {
                TopText = $"Skanowany : {_akcja.TwrKod}",
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



            try
            {



                overlay.Children.Add(torch);
                
                    scanPage = new ZXingScannerPage(
                        new ZXing.Mobile.MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 3000 }, overlay);
                    scanPage.DefaultOverlayShowFlashButton = true;
                    scanPage.OnScanResult += (result) =>
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        skanean = result.Text;

                        if (skanean == lbl_ean.Text)
                        {
                            ile_zeskanowancyh += 1;
                            overlay.BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}";
                            DisplayAlert(null, $"Zeskanowanych szt : {ile_zeskanowancyh}", "OK");
                            PrintCommand();

                            Zapisz();
                            entry_kodean.Text = ile_zeskanowancyh.ToString();

                        }
                        else
                        {

                            DisplayAlert(null, "Probujesz zeskanować inny model..", "OK");
                        }
                    });
                    await Navigation.PushModalAsync(scanPage);
                 
            }
            catch (Exception x)
            {
                System.Diagnostics.Debug.WriteLine(x.Message);
            }
        }

        private  async void deviceListe()
        {
            var app = Application.Current as App;

            try
            {
                var list = await _cpclPrinter.connectableDevice();


                if (list.Count > 0)
                    drukarka = list[app.Drukarka].Address;
            }
            catch (Exception ss)
            {
                System.Diagnostics.Debug.WriteLine(ss.Message);
                 
            }


             

        }
         
        public async void PrintCommand ()//=> new Command(async () =>
        {

             
            await _cpclPrinter.setForm(0, 200, 200, 250,350, 1);
            await _cpclPrinter.setBarCodeText(0, 1, 0);
            await _cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);
             
            await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 111, 5, "SZACHOWNICA", 0);
            await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 30, _akcja.TwrKod, 0);
            await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 50, _akcja.TwrNazwa, 0);
            await _cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_BCS_EAN13, 1, 
cpclConst.LK_CPCL_BCS_0RATIO, 35, 75, 80, _akcja.TwrEan ,0);


            await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_5, 3, 111, 120, _akcja.TwrCena+" pln", 0);


            //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 120, 10, "SZACHOWNICA", 0);
            //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 1, 10, 70, _akcja.TwrKod, 0);
            //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 10, 120, _akcja.TwrNazwa, 0);
            //await _cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_BCS_EAN13, 2, cpclConst.LK_CPCL_BCS_1RATIO, 60, 20, 210, _akcja.TwrEan, 0);

            //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 2, 120, 300, _akcja.TwrCena, 0);

            await _cpclPrinter.printForm();


        }
     

        private  async void Zapisz()
        {

            Model.AkcjeNagElem akcjeNagElem = new Model.AkcjeNagElem();
            akcjeNagElem.AkN_GidNumer = _akcja.AkN_GidNumer;
            akcjeNagElem.TwrKod = _akcja.TwrKod;
            akcjeNagElem.TwrGrupa = _akcja.TwrGrupa;
            akcjeNagElem.TwrDep = _akcja.TwrDep;
            akcjeNagElem.TwrCena = _akcja.TwrCena;
            akcjeNagElem.TwrEan = _akcja.TwrEan;
            akcjeNagElem.TwrSymbol = _akcja.TwrSymbol;
            akcjeNagElem.TwrUrl = _akcja.TwrUrl;

            var wynik = await _connection.QueryAsync<Model.AkcjeNagElem>("select * from AkcjeNagElem where AkN_GidNumer = ? and TwrKod=?", akcjeNagElem.AkN_GidNumer, akcjeNagElem.TwrKod);

            if (wynik.Count > 0)
            {
                var wpis = wynik[0];
                wpis.TwrSkan = ile_zeskanowancyh;
                await _connection.UpdateAsync(wpis);
            }
            else
            {
                akcjeNagElem.TwrSkan = ile_zeskanowancyh;
                await _connection.InsertAsync(akcjeNagElem);
            }
        }


        
        private void Open_url_Clicked(object sender, EventArgs e)
        {
            if (List_AkcjeView.TypAkcji.Contains("Przerzut"))
            {

                ile_zeskanowancyh = Convert.ToInt32(entry_kodean.Text);
                Zapisz();
                Navigation.PopModalAsync();
            }
            else
            {
                Skanuj(); 
            }
            //_akcja.TwrSkan = Convert.ToInt32(entry_kodean.Text);
            //Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            if (ile_zeskanowancyh > 0)
                _akcja.TwrSkan = ile_zeskanowancyh;
            return base.OnBackButtonPressed();
        }

    }
}
