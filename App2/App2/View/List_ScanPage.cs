
using App2.Model;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using SQLite;
using System;
using System.Threading;
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
        private Entry entry_kodean;
        private Entry entry_EanSkaner;
        private Image img_foto;

        private SQLiteAsyncConnection _connection;
        private string skanean;
        private Model.AkcjeNagElem _akcja;
        ZXingDefaultOverlay overlay;
        ZXingScannerPage scanPage;
        int ile_zeskanowancyh = 0;
        //private ISewooXamarinCPCL SettingsPage._cpclPrinter =  SettingsPage._cpclPrinter;
        CPCLConst cpclConst;
        int IResult;
        bool CanPrint;

        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);
        // private  ISewooXamarinCPCL cpclPrinter;
        string drukarka;




        public List_ScanPage(Model.AkcjeNagElem akcje) //edycja
        {
            this.Title = "Dodaj MM";
            //_blueToothService = DependencyService.Get<IBlueToothService>();


            // CrossSewooXamarinCPCL.Current.createCpclService(int iCodePage);
            _akcja = akcje;


            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
            {
                cpclConst = View.SettingsPage.cpclConst;
                deviceListe();
                CanPrint = View.SettingsPage.CzyDrukarkaOn;
            }




            //cpclConst = new CPCLConst();
            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);

            if (SettingsPage.SelectedDeviceType == 1)
            {
                WidokAparat();
            }
            else
            {
                WidokSkaner();
            }


        }

        void WidokAparat()
        {

            //if (List_AkcjeView.TypAkcji.Contains("Przecena"))
            //deviceListe();

            var scrollView = new ScrollView();

            cpclConst = new CPCLConst();
            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);

            var layout = new AbsoluteLayout();

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
            img_foto.Source = _akcja.TwrUrl.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(_akcja.TwrUrl.Replace("Miniatury/", ""));
            };
            img_foto.GestureRecognizers.Add(tapGestureRecognizer);

            //AbsoluteLayout.SetLayoutBounds(img_foto, new Rectangle(.1,.1,1,1));
            //AbsoluteLayout.SetLayoutFlags(img_foto, AbsoluteLayoutFlags.All);

            //layout.Children.Add(img_foto);
            stackLayout.Children.Add(img_foto);
            //_gidnumer = mmka.gi;


            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;
            lbl_stan.Text = "Stan : " + _akcja.TwrStan + " szt";
            lbl_stan.FontAttributes = FontAttributes.Bold;

            stackLayout.Children.Add(lbl_stan);

            lbl_twrkod = new Label();
            lbl_twrkod.HorizontalOptions = LayoutOptions.Center;
            lbl_twrkod.Text = "Kod towaru : " + _akcja.TwrKod;

            lbl_ean = new Label();
            lbl_ean.HorizontalOptions = LayoutOptions.Center;
            lbl_ean.Text = _akcja.TwrEan;

            entry_kodean = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                Text = _akcja.TwrSkan == 0 ? "" : _akcja.TwrSkan.ToString(),
                WidthRequest = 60,
                Placeholder = "Ilość",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
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
            open_url.Text = List_AkcjeView.TypAkcji.Contains("Przecena") ? "Zacznij skanowanie" : "Zapisz";
            open_url.CornerRadius = 15;
            open_url.Clicked += Open_url_Clicked;
            open_url.VerticalOptions = LayoutOptions.EndAndExpand;



            Button enterEan = new Button();
            enterEan.Text = "Wydrukuj ręcznie";
            enterEan.CornerRadius = 15;
            enterEan.Clicked += EnterEan_Clicked; ;
            enterEan.VerticalOptions = LayoutOptions.EndAndExpand;



            stackLayout.Children.Add(lbl_twrkod);
            stackLayout.Children.Add(lbl_nazwa);
            stackLayout.Children.Add(lbl_ean);
            stackLayout.Children.Add(entry_kodean);
            stackLayout.Children.Add(lbl_symbol);
            stackLayout.Children.Add(lbl_cena);
            stackLayout.Children.Add(lbl_cena1);


            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(15, 0, 15, 0);
            stackLayout.Padding = 8;
            //stackLayout_gl.Children.Add(layout);   //dodane
            stackLayout_gl.Children.Add(stackLayout);
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                stackLayout_gl.Children.Add(enterEan);
            stackLayout_gl.Children.Add(open_url);
            stackLayout_gl.Padding = new Thickness(15);

            scrollView.Content = stackLayout_gl;

            Content = scrollView;

        }

        async void WidokSkaner()
        {

            var scrollView = new ScrollView();

            cpclConst = new CPCLConst();
            ile_zeskanowancyh = _akcja.TwrSkan > 0 ? _akcja.TwrSkan : ile_zeskanowancyh;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);

            var layout = new AbsoluteLayout();

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
            img_foto.Source = _akcja.TwrUrl.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(_akcja.TwrUrl.Replace("Miniatury/", ""));
            };
            img_foto.GestureRecognizers.Add(tapGestureRecognizer);

            //AbsoluteLayout.SetLayoutBounds(img_foto, new Rectangle(.1,.1,1,1));
            //AbsoluteLayout.SetLayoutFlags(img_foto, AbsoluteLayoutFlags.All);

            //layout.Children.Add(img_foto);
            stackLayout.Children.Add(img_foto);
            //_gidnumer = mmka.gi;


            lbl_stan = new Label();
            lbl_stan.HorizontalOptions = LayoutOptions.Center;
            lbl_stan.Text = "Stan : " + _akcja.TwrStan + " szt";
            lbl_stan.FontAttributes = FontAttributes.Bold;
            stackLayout.Children.Add(lbl_stan);

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
                await DisplayAlert(null, "skopiowano ean", "ok");
            };
            lbl_ean.GestureRecognizers.Add(tapCopyLabelEan);


            entry_kodean = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                Text = _akcja.TwrSkan == 0 ? "" : _akcja.TwrSkan.ToString(),
                WidthRequest = 60,
                Placeholder = "Ilość",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
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



            Button enterEanButton = new Button();
            enterEanButton.Text = "Wydrukuj ręcznie";
            enterEanButton.CornerRadius = 15;
            enterEanButton.Clicked += EnterEan_Clicked; ;
            enterEanButton.VerticalOptions = LayoutOptions.EndAndExpand;

            entry_EanSkaner = new Entry()
            {
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                IsReadOnly = View.SettingsPage.CzyDrukarkaOn ? false : true,
                ReturnCommand = new Command(() => zapiszdrukuj()),
                WidthRequest = 180,
                Placeholder = View.SettingsPage.CzyDrukarkaOn ? "Wpisz/zeskanuj Ean" : "Drukarka nie połączona",
                TextColor = Color.Black
            };

            stackLayout.Children.Add(lbl_twrkod);
            stackLayout.Children.Add(lbl_nazwa);
            stackLayout.Children.Add(lbl_ean);
            stackLayout.Children.Add(entry_kodean);
            stackLayout.Children.Add(entry_EanSkaner);
            stackLayout.Children.Add(lbl_symbol);
            stackLayout.Children.Add(lbl_cena);
            stackLayout.Children.Add(lbl_cena1);


            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(8, 0, 8, 0);
            //stackLayout.Padding = 8;
            //stackLayout_gl.Children.Add(layout);   //dodane
            stackLayout_gl.Children.Add(stackLayout);
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                stackLayout_gl.Children.Add(enterEanButton);
            stackLayout_gl.Children.Add(open_url);
            stackLayout_gl.Padding = new Thickness(8, 0, 8, 0);

            scrollView.Content = stackLayout_gl;

            Content = scrollView;
            Appearing += (object sender, System.EventArgs e) => entry_EanSkaner.Focus();
        }

        async void zapiszdrukuj()
        {
            if (_akcja.TwrEan == entry_EanSkaner.Text)
            {
                if(View.SettingsPage.CzyDrukarkaOn)
                ile_zeskanowancyh += 1;
                if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                {
                    if (CanPrint)
                        PrintCommand();
                    //DisplayAlert(null, "Drukuje..", "OK");
                    Zapisz();
                    entry_kodean.Text = ile_zeskanowancyh.ToString();
                    entry_EanSkaner.Text = "";
                    if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh + 1))
                        entry_EanSkaner.Focus();
                }
                else
                {
                    await DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");
                    ile_zeskanowancyh -= 1;
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Skanujesz model inny niż otwarty!", "OK");
                entry_EanSkaner.Text = "";
            }
        }


        private async void EnterEan_Clicked(object sender, EventArgs e)
        {
            try
            {

                string odpowiedz = await DisplayPromptAsync("Druk bez zliczania sztuk", "( awaryjny )", "Drukuj", "Anuluj", "Wprowadź Ean:");

                if (odpowiedz != "")
                {
                    if (_akcja.TwrEan == (odpowiedz))
                    {
                        //string ile = odpowiedz.Text.Substring(odpowiedz.Text.IndexOf(",", 0) + 1, 1);

                        if (CanPrint)
                            PrintCommand();
                    }
                    else
                    {
                        await DisplayAlert("Uwaga", "Podany Ean nie pasuje", "OK");
                    }
                }
            }
            catch (Exception x)
            {
                await DisplayAlert(null, x.Message, "OK"); ;
            }

        }

        private async void Skanuj(sbyte typDevice)//DRUKARKA
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
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            skanean = result.Text;

                            if (skanean == lbl_ean.Text)
                            {
                                if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                                {
                                    ile_zeskanowancyh += 1;
                                    overlay.BottomText = $"Zeskanowanych szt : {ile_zeskanowancyh}/{_akcja.TwrStan}";
                                    //DisplayAlert(null, $"Zeskanowanych szt : {ile_zeskanowancyh}", "OK");
                                    if (CanPrint)
                                        PrintCommand();

                                    Zapisz();
                                    entry_kodean.Text = ile_zeskanowancyh.ToString();

                                }
                                else
                                {
                                    DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");

                                }


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
            else
            {


                //var layout = new AbsoluteLayout();

                //var enterEAn = new Entry
                //{

                //};
                //AbsoluteLayout.SetLayoutBounds(enterEAn, new Rectangle(0.5, .5, .5, .2));
                //AbsoluteLayout.SetLayoutFlags(enterEAn, AbsoluteLayoutFlags.All);

                //var label = new Label
                //{
                //    Text = $"Zeskanowanych szt : {ile_zeskanowancyh}/{_akcja.TwrStan}"
                //};
                //AbsoluteLayout.SetLayoutBounds(enterEAn, new Rectangle(0.6, .5, .5, .2));
                //AbsoluteLayout.SetLayoutFlags(enterEAn, AbsoluteLayoutFlags.All);

                //layout.Children.Add(enterEAn);
                //layout.Children.Add(label);

                //new ContentPage
                //{
                //    Content = new AbsoluteLayout
                //    {
                //           Children = { layout }
                //    },
                //};



                //var page = new SkanerPage(_akcja);
                //page.ListViewSklepy.ItemSelected += (source, args) =>
                //{
                //    var sklep = args.SelectedItem as Model.ListaSklepow;

                //    _magDcl.Text = sklep.mag_kod;
                //    Navigation.PopModalAsync();
                //};

                //Navigation.PushModalAsync(page);





            }
        }



        private async void deviceListe()
        {
            //try
            //{
            //var app = Application.Current as App;
            SettingsPage._cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);

            //await printSemaphore.WaitAsync();

            //    var list = await SettingsPage._cpclPrinter.connectableDevice();


            //    if (list.Count > 0 && app.Drukarka > 0)
            //    {
            //        drukarka = list[app.Drukarka].Address;
            //        System.Diagnostics.Debug.WriteLine("lista pełna");

            //    }
            //    else
            //    {
            //        drukarka = "00:00:00:00:00:00";
            //        System.Diagnostics.Debug.WriteLine("lista zerowa");
            //    }

            //}
            //catch (Exception ss)
            //{
            //    System.Diagnostics.Debug.WriteLine(ss.Message);
            //    System.Diagnostics.Debug.WriteLine("coś nie pykło");

            //}
            //finally
            //{
            //    printSemaphore.Release();

            //}
            /////////////////////////////////////////////////////////////////////////////
            try
            {
                if (!View.SettingsPage.CzyDrukarkaOn)
                {

                    IResult = await SettingsPage._cpclPrinter.connect(View.SettingsPage.editAddress.Text);
                    if (IResult == cpclConst.LK_SUCCESS)
                    {
                        View.SettingsPage.CzyDrukarkaOn = true;
                        CanPrint = true;
                        await DisplayAlert(null, "Połączono z drukarką", "OK");
                    }
                    else
                    {
                        CanPrint = false;
                        //await DisplayAlert(null, "Drukarka Nie podłączone", "OK");

                        ErrorStatusDisp("Błąd  drukarki", IResult);
                        View.SettingsPage.CzyDrukarkaOn = false;

                    }

                }
                else
                {
                    //CanPrint = true;
                    //View.SettingsPage.CzyDrukarkaOn = true;

                }
            }
            catch (Exception)
            {
                await DisplayAlert(null, "Błąd", "OK");
            }


        }


        int polozenie;
        public async void PrintCommand(string ile = null)
        {

            int drukSzt;
            //_cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2); 

            if (ile != null)
                drukSzt = Convert.ToInt16(ile);
            else
                drukSzt = 1;


            await printSemaphore.WaitAsync();
            try
            {
                //int iResult;

                //iResult = await SettingsPage._cpclPrinter.printerCheck();
                //switch (Device.RuntimePlatform)
                //{
                //    case Device.iOS:
                //        iResult = await SettingsPage._cpclPrinter.printStatus();
                //        Debug.WriteLine("PrinterStatus = " + iResult);
                //        break;
                //    default:
                //        Debug.WriteLine("printerCheck = " + iResult);
                //        break;
                //}

                //if (iResult != cpclConst.LK_SUCCESS)
                //{
                //    ErrorStatusDisp("Printer error", iResult);
                //    return;
                //}

                var cenaZl = _akcja.TwrCena.Substring(0, _akcja.TwrCena.IndexOf(".", 0));
                var cenaGr = _akcja.TwrCena.Substring(_akcja.TwrCena.IndexOf(".", 0) + 1, 2);

                switch (cenaZl.Length)
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

                int koniecLinii = (_akcja.TwrCena1.Length <= 5) ? 155 : 175;


                string twr_nazwa = (_akcja.TwrNazwa.Length > 20 ? _akcja.TwrNazwa.Substring(0, 20) : _akcja.TwrNazwa);

                string procent = Convert.ToInt16((Double.Parse(_akcja.TwrCena.Replace(".", ",")) / Double.Parse(_akcja.TwrCena1.Replace(".", ",")) - 1) * 100).ToString();

                await SettingsPage._cpclPrinter.setForm(0, 200, 200, 370, 350, drukSzt);
                await SettingsPage._cpclPrinter.setBarCodeText(7, 0, 0);
                await SettingsPage._cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);

                // Set the code page of font number(7) 
                await SettingsPage._cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);

                //await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 111, 0, "Szachownica", 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 5, _akcja.TwrKod, 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 30, twr_nazwa, 0);
                await SettingsPage._cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_BCS_EAN13, 1,
    cpclConst.LK_CPCL_BCS_0RATIO, 40, 80, 55, _akcja.TwrEan, 0);

                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 50, 115, _akcja.TwrCena1, 0);
                await SettingsPage._cpclPrinter.printLine(40, 160, koniecLinii, 125, 10);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 1, polozenie, 160, cenaZl, 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 158, 160, cenaGr, 0);
                //190

                await SettingsPage._cpclPrinter.setConcat(cpclConst.LK_CPCL_CONCAT, polozenie, 160);
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 3, 0, cenaZl);
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 0, cenaGr);//góra grosze
                                                                                                   //await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 45, cenaGr); dół
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_7, 0, 60, "zł");
                await SettingsPage._cpclPrinter.resetConcat();

                //wait SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 220, 150, "pln", 0);
                if (Convert.ToInt16(procent) < -20)
                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 245, $"{procent}%", 0);

                await SettingsPage._cpclPrinter.printForm();



                //iResult = await SettingsPage._cpclPrinter.printResults();
                //switch (Device.RuntimePlatform)
                //{
                //    case Device.iOS:
                //        iResult = await SettingsPage._cpclPrinter.printStatus();
                //        Debug.WriteLine("PrinterStatus = " + iResult);
                //        break;
                //    default:
                //        Debug.WriteLine("PrinterResults = " + iResult);
                //        break;
                //}
                //if (iResult != cpclConst.LK_SUCCESS)
                //{
                //    ErrorStatusDisp("Printing error", iResult);
                //}
                //else
                //{
                //    await DisplayAlert("Printing Result", "Printing success", "OK");
                //}
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

        private async void Zapisz()
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
            akcjeNagElem.TwrGidNumer = _akcja.TwrGidNumer;
            var wynik = await _connection.QueryAsync<Model.AkcjeNagElem>("select * from AkcjeNagElem where AkN_GidNumer = ? and TwrKod=?", akcjeNagElem.AkN_GidNumer, akcjeNagElem.TwrKod);

            if (wynik.Count > 0)
            {
                var wpis = wynik[0];
                wpis.TwrSkan = ile_zeskanowancyh;
                // wpis.TwrStanVsSKan = $"{_akcja.TwrSkan}/{_akcja.TwrStan}"; //dodane
                //if (SprIlosc(_akcja.TwrStan, ile_zeskanowancyh))
                await _connection.UpdateAsync(wpis);
                //else await DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");
            }
            else
            {
                akcjeNagElem.TwrSkan = ile_zeskanowancyh;
                // akcjeNagElem.TwrStanVsSKan = $"{_akcja.TwrSkan}/{_akcja.TwrStan}"; //dodane

                await _connection.InsertAsync(akcjeNagElem);


            }
        }



        int iResult;
        private async void Open_url_Clicked(object sender, EventArgs e)
        {
            //cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);
            if (SettingsPage.SelectedDeviceType != 1)
                entry_EanSkaner.Focus();

            if (!List_AkcjeView.TypAkcji.Contains("Przecena"))
            {

                ile_zeskanowancyh = Convert.ToInt32(entry_kodean.Text);
                if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                {
                    Zapisz();
                    if (ile_zeskanowancyh > 0)
                        _akcja.TwrSkan = ile_zeskanowancyh;
                    await Navigation.PopModalAsync();
                }

                else await DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");


            }
            else
            {
                try
                {

                    //  if (Object.ReferenceEquals(null,   SettingsPage._cpclPrinter))
                    if (View.SettingsPage.CzyDrukarkaOn)
                    {
                        iResult = await SettingsPage._cpclPrinter.printerCheck();

                        if (iResult != cpclConst.LK_SUCCESS)
                        {
                            ErrorStatusDisp("Printer error", iResult);

                            CanPrint = false;
                            return;
                        }
                    }
                    if (!List_AkcjeView.TypAkcji.Contains("Przecena"))
                        Skanuj(SettingsPage.SelectedDeviceType);
                    else if (List_AkcjeView.TypAkcji.Contains("Przecena") && CanPrint == false)
                        await DisplayAlert(null, "Do przeceny wymagana drukarka", "OK");
                    else
                        Skanuj(SettingsPage.SelectedDeviceType);
                }
                catch (Exception s)
                {
                    await DisplayAlert(null, s.Message, "OK");
                }
                // Navigation.PushModalAsync(new Drukowanie(_akcja));
            }
            //_akcja.TwrSkan = Convert.ToInt32(entry_kodean.Text);
            //Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            _akcja.TwrSkan = ile_zeskanowancyh;
            return base.OnBackButtonPressed();


            //if (SprIlosc(_akcja.TwrStan, ile_zeskanowancyh))
            //    _akcja.TwrSkan = ile_zeskanowancyh;
            //else
            //{

            //   DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");
            //    return base.OnBackButtonPressed(); ;
            //}
            //return base.OnBackButtonPressed();
        }

    }
}
