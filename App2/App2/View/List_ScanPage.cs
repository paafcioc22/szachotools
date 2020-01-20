
using App2.Model;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Array Controls;



        public List_ScanPage(Model.AkcjeNagElem akcje) //edycja
        {
            this.Title = "Dodaj MM";
            //_blueToothService = DependencyService.Get<IBlueToothService>();


            // CrossSewooXamarinCPCL.Current.createCpclService(int iCodePage);
            _akcja = akcje;


            Controls = new[] { "Dodaj mm", "Przegladaj", "Tworz" };




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
            lbl_ean.TextDecorations = TextDecorations.Underline;
            var tapCopyLabelEan = new TapGestureRecognizer();
            tapCopyLabelEan.Tapped += async (s, e) =>
            {
                await Clipboard.SetTextAsync(_akcja.TwrEan);
                await DisplayAlert(null, "skopiowano ean", "ok");
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
            stackLayout.Children.Add(entry_skanowanaIlosc);
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

        void WidokSkaner()
        {

            var scrollView = new ScrollView();

            cpclConst = new CPCLConst();
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


            img_foto = new Image();
            img_foto.Source = _akcja.TwrUrl.Replace("Miniatury/", "");
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Launcher.OpenAsync(_akcja.TwrUrl.Replace("Miniatury/", ""));
            };
            img_foto.GestureRecognizers.Add(tapGestureRecognizer);


            AbsoluteLayout.SetLayoutBounds(img_foto, new Rectangle(0, 0.09, 1, .4));
            AbsoluteLayout.SetLayoutFlags(img_foto, AbsoluteLayoutFlags.All);

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
                await DisplayAlert(null, "skopiowano ean", "ok");
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
            AbsoluteLayout.SetLayoutBounds(open_url, new Rectangle(0.5, 1, .9, 50));
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
                HorizontalOptions = LayoutOptions.End
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
                HorizontalOptions = LayoutOptions.End
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
                else {
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
                Keyboard = Keyboard.Numeric,
                WidthRequest = 180,
                //Placeholder = View.SettingsPage.CzyDrukarkaOn ? "Wpisz/zeskanuj Ean" : "Drukarka nie połączona",
                Placeholder = "Wpisz/zeskanuj Ean",
                TextColor = Color.Black,

            };

            if (List_AkcjeView.TypAkcji.Contains("Przerzuty") || List_AkcjeView.TypAkcji.Contains("Zwrot"))
            {
                entry_EanSkaner.ReturnCommand = new Command(() => setFocusEntryIlosc(entry_EanSkaner.Text));
            }
            else { 
                entry_EanSkaner.IsReadOnly = View.SettingsPage.CzyDrukarkaOn ? false : true;
                entry_EanSkaner.ReturnCommand = new Command(() => zapiszdrukuj()); 
            }

            Button addToMM = new Button()
            {
                Text="Dodaj do MMki",
                CornerRadius =60,
                WidthRequest=120,
                FontSize=10,
                BackgroundColor= Color.DarkCyan,
                TextColor = Color.AntiqueWhite
            };
            AbsoluteLayout.SetLayoutBounds(addToMM, new Rectangle(1, .70, .20, 50));
            AbsoluteLayout.SetLayoutFlags(addToMM, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            stack_dane.Children.Add(lbl_twrkod);
            stack_dane.Children.Add(lbl_nazwa);
            stack_dane.Children.Add(lbl_ean);
            stack_dane.Children.Add(lbl_stan);
            stack_dane.Children.Add(entry_skanowanaIlosc);
            stack_dane.Children.Add(entry_EanSkaner);
            stack_dane.Children.Add(lbl_symbol);
            stack_dane.Children.Add(lbl_cena);
            //stack_dane.Children.Add(lbl_cena1);
            AbsoluteLayout.SetLayoutBounds(stack_dane, new Rectangle(0, 1, 1, .55));
            AbsoluteLayout.SetLayoutFlags(stack_dane, AbsoluteLayoutFlags.All);


             

            absoluteLayout.Children.Add(img_foto);
            absoluteLayout.Children.Add(lbl_naglowek);
            absoluteLayout.Children.Add(stack_dane);
            absoluteLayout.Children.Add(open_url);
            if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                absoluteLayout.Children.Add(enterEanButton);
            //absoluteLayout.Children.Add(addToMM);
            if (List_AkcjeView.TypAkcji.Contains("Przerzuty")|| List_AkcjeView.TypAkcji.Contains("Zwrot"))
            {
                absoluteLayout.Children.Add(frame_btn);
                absoluteLayout.Children.Add(grid);
            }
            





            //scrollView.Content = stackLayout_gl;

            Content = absoluteLayout;
            Appearing += (object sender, System.EventArgs e) => entry_EanSkaner.Focus();
        }

        private void setFocusEntryIlosc(string entryEan)
        {
            if (entryEan == _akcja.TwrEan)
                entry_skanowanaIlosc.Focus();
            else
                DisplayAlert(null, "Błędny ean", "OK");
        }

        

        private async void BtnBrowseMM_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new View.StartCreateMmPage());
        }

        private async void BtnAddToMM_Clicked(object sender, EventArgs e)
        {
            var ListaMMWBuforze = new List<string>();
            var dokmm = new DokMM();
            var listaMM = dokmm.getMMki().Where(c => c.statuss == 0);
            int gidnumerMM;
            //var listaMM = Model.DokMM.dokMMs.Where(c =>c.statuss ==0);
            int ilosc;

            
             

                var czyPoprawnaIlosc = Int32.TryParse(entry_skanowanaIlosc.Text, out ilosc);
                if (czyPoprawnaIlosc && ilosc > 0)
                {
                    DokMM dokMM = new DokMM();
                    var listaIstniejacych = dokMM.ListaIstniejacych(_akcja.TwrKod);
                    List<string> naJakichMM = new List<string>();

                    int sumaGlobalna = listaIstniejacych.Count > 0 ? listaIstniejacych.Sum(c => c.szt) : 0;


                    foreach (var mm in listaIstniejacych)
                    {
                        naJakichMM.Add(string.Format($"{mm.mag_dcl} - {mm.opis.Replace("Pakował(a):", "")} : {mm.szt} szt"));
                    }

                if ((ilosc) > (_akcja.TwrStan - sumaGlobalna))
                {

                    if (listaIstniejacych.Count > 0)
                        await DisplayActionSheet("Ilość przekracza stan- występuje już :", "OK", null, naJakichMM.ToArray());
                    else
                        await DisplayAlert("Uwaga", "Wpisana Ilość przekracza stan", "OK");

                } 
                else
                {
                    if (ilosc <= (_akcja.TwrStan - sumaGlobalna) && listaIstniejacych.Count > 0)
                        await DisplayActionSheet("Model występuje już na mmkach:", "OK", null, naJakichMM.ToArray()); 
                    foreach (var mm in listaMM)
                    {
                        ListaMMWBuforze.Add(mm.gidnumer + ")" + mm.mag_dcl + " " + mm.opis.Replace("Pakował(a):", ""));
                    }
                    var odp = await DisplayActionSheet("Wybierz MM:", "Wyjdź", null, ListaMMWBuforze.ToArray());

                    if (odp != "Wyjdź")
                    {
                        var gdzieCut = odp.IndexOf(')');
                        var cutNrmmki = odp.Substring(0, gdzieCut);
                        Int32.TryParse(cutNrmmki, out gidnumerMM);

                        ZapiszPozycje(gidnumerMM, _akcja.TwrKod, ilosc, _akcja.TwrStan, odp);
                    }

                }

                }
                else
                    await DisplayAlert(null, "Błędna ilosc", "OK"); 
            
            
        }

        public async void ZapiszPozycje(int mmGidnumer, string twrKod, int ilosc, int stan_szt,string opis)
        {
            Model.DokMM dokMM = new Model.DokMM();
            dokMM.gidnumer = mmGidnumer;
            dokMM.twrkod = twrKod;
            dokMM.szt = (ilosc);

             
            //var listaIstniejacych= dokMM.ListaIstniejacych(dokMM.twrkod);
            //List<string> naJakichMM = new List<string>();

            //int sumaGlobalna = listaIstniejacych.Count > 0 ? listaIstniejacych.Sum(c => c.szt) : 0;
            //if ((ilosc) > (stan_szt-sumaGlobalna) )
            //{
            //    foreach (var mm in listaIstniejacych)
            //    {
            //        naJakichMM.Add(string.Format($"{mm.mag_dcl} - {mm.opis.Replace("Pakował(a):", "")} : {mm.szt} szt"));
            //    }
            //    await DisplayActionSheet("Ilość przekracza stan- występuje już :","OK",null,naJakichMM.ToArray());
            //}
            //else
            //{
                 

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
                        dokMM.getElementy(mmGidnumer);
                        var opis2 = opis.Substring(opis.IndexOf(')') + 1);
                        //var oooo = opis.Substring(opis.IndexOf(')') + 1, opis.Length - opis.IndexOf(')') + 2);
                        await DisplayAlert("Dodano..", $"..{ilosc} szt do {opis2}", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
                }

                // await Navigation.PopModalAsync(); 
            }
            else 
            {
                var opis2 = opis.Substring(opis.IndexOf(')') + 1);
                await DisplayAlert("Dodano..", $"..{dokMM.szt} szt do {opis2}", "OK");
            }

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
                                if (await PrintCommand())
                                {
                                    //ile_zeskanowancyh += 1;
                                    Zapisz();
                                    entry_skanowanaIlosc.Text = ile_zeskanowancyh.ToString();
                                    entry_EanSkaner.Text = "";
                                    if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh + 1))
                                        entry_EanSkaner.Focus(); 
                                }else
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
                                        Zapisz();
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
                await DisplayAlert(null, "Błąd połączenia z drukarką", "OK");
            }


        }


        int polozenie;
            bool printSukces;
        public async Task<bool> PrintCommand(string ile = null)
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
                    printSukces= false;
                }
                else
                {
                    //await DisplayAlert("Printing Result", "Printing success", "OK");
                    
                    printSukces= true;
                     
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

        public async void Zapisz()
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

                bool CzyWpisanoIlosc = Int32.TryParse(entry_skanowanaIlosc.Text, out ile_zeskanowancyh);

                //ile_zeskanowancyh = Convert.ToInt32(entry_kodean.Text);
                
                 if(CzyWpisanoIlosc)
                    if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                    {
                        Zapisz();
                        if (ile_zeskanowancyh > 0)
                            _akcja.TwrSkan = ile_zeskanowancyh;
                        await Navigation.PopModalAsync();
                    }
                    else await DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");
                 else await DisplayAlert("Uwaga", "Błędna wartość", "OK");


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
            bool CzyWpisanoIlosc = Int32.TryParse(entry_skanowanaIlosc.Text, out ile_zeskanowancyh);

            //ile_zeskanowancyh = Convert.ToInt32(entry_kodean.Text);

            if (CzyWpisanoIlosc)
                if (CzyMniejszeNStan(_akcja.TwrStan, ile_zeskanowancyh))
                    _akcja.TwrSkan = ile_zeskanowancyh;
                else
                {
                    DisplayAlert("Uwaga", "Wartość większa niż stan", "OK");
                    
                    return true;
                }

            
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
