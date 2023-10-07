using App2.Model.ApiModel;
using App2.OptimaAPI;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using static Xamarin.Forms.Button;
using static Xamarin.Forms.Button.ButtonContentLayout;

namespace App2.View
{
    class AddTwrPage : ContentPage
    {
        private RestClient _client;
        private int dokumentId;
        private Label stan;
        private Label ean;
        private Label symbol;
        private Label nazwa;
        private Entry kodean;
        private Entry ilosc;
        private Image foto;
        private Button btn_Skanuj;
        private Button btn_Zapisz;
     

        string twrkod;
        string stan_szt;
        string twr_url;
        string twr_nazwa;
        string twr_symbol;
        string twr_ean;
        TwrInfo twr_info = new TwrInfo();

        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;
        ServiceDokumentyApi serwisApi = new ServiceDokumentyApi();
        private readonly DokElementDto elementDto;

        public AddTwrPage(int gidnumer) //dodawamoe pozycji
        {
            this.Title = "Dodaj MM";
            StackLayout stackLayout_gl = new StackLayout();
            StackLayout stackLayout = new StackLayout();
            StackLayout stack_naglowek = new StackLayout();

            var app = Application.Current as App;

            _client = new RestClient($"http://{app.Serwer}"); //todo : pobierz adres ip z konfiguracji aplikacji


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

            dokumentId = gidnumer;

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
            ilosc.Completed += async (object sender, EventArgs e) =>
            {
                await ZapiszPozycje();

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

            stackLayout.VerticalOptions = LayoutOptions.Center;
            stackLayout.Padding = new Thickness(30, 0, 30, 0);

            stackLayout_gl.Children.Add(stackLayout);
            absoluteLayout.Children.Add(stackLayout_gl, new Rectangle(0, 0.5, 1, 1), AbsoluteLayoutFlags.HeightProportional);

            Content = stackLayout_gl;

        }

        public AddTwrPage(DokElementDto _elementDto) //edycja
        {
            this.Title = "Dodaj MM";

            this.elementDto = _elementDto;

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
            kodean.Text = elementDto.TwrKod;
            kodean.IsEnabled = false;
            kodean.Keyboard = Keyboard.Telephone;
            kodean.Unfocused += Kodean_Unfocused;
            kodean.ReturnCommand = new Command(() => ilosc.Focus());
            stackLayout.Children.Add(kodean);

            ilosc = new Entry();
            ilosc.Keyboard = Keyboard.Text;
            ilosc.Placeholder = "Wpisz Ilość";
            ilosc.Keyboard = Keyboard.Telephone;
            ilosc.Text = elementDto.TwrIlosc.ToString();
            ilosc.Completed += async (object sender, EventArgs e) =>
            {
                await EdytujPozyce();

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
            GetDataFromTwrKod(elementDto.TwrKod, false);
            ilosc.Focus();
            
        }


        public AddTwrPage(DokElementDto _elementDto, string CzyFoto = null)
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
            GetDataFromTwrKod(_elementDto.TwrKod, true);


        }

        private async void Kodean_Unfocused(object sender, FocusEventArgs e)
        {
            twr_info = await pobierztwrkod(kodean.Text);
        }

        private async Task EdytujPozyce()
        {
            if (ilosc.Text != null && kodean.Text != null)
            {
                Int32.TryParse(ilosc.Text, out int iloscSkanowana);
                if (iloscSkanowana > Int32.Parse(stan_szt) && iloscSkanowana == 0)
                {
                    await DisplayAlert(null, "Wpisana ilość przekracza stan lub 0 ", "OK");
                }
                else
                {
                    var createElementDto = new CreateDokElementDto()
                    {
                        DokTyp = (int)GidTyp.Mm,
                        TwrIlosc = iloscSkanowana,
                        TwrKod = twr_info.Twr_Kod,
                        TwrNazwa = twr_info.Twr_Nazwa
                    };

                    var resposne = await serwisApi.UpadteElement(iloscSkanowana, elementDto.DokNaglowekId, elementDto.Id);
                    if (resposne.IsSuccessful)
                    {
                        await DisplayAlert("Dodano..", $"{iloscSkanowana} szt", "OK");
                    }

                    //Model.DokMM dokMM = new Model.DokMM();
                    //dokMM.gidnumer = _gidnumer;
                    //dokMM.twrkod = kodean.Text;
                    //dokMM.szt = Convert.ToInt32(ilosc.Text);
                    //dokMM.UpdateElement(dokMM);
                    //Model.DokMM.dokElementy.GetEnumerator();// (usunMM);
                    await serwisApi.GetDokWithElementsById(elementDto.DokNaglowekId);
                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie uzupełniono wszystkich pól!", "OK");
            }
        }

        private void Btn_Update_Clicked(object sender, EventArgs e)
        {
            EdytujPozyce();
        }
        private async Task ZapiszPozycje()
        {
            int totalTwrIlosc = 0;
            //int iloscSkanowana = 0;
            CreateDokElementDto createElementDto;

            if (!string.IsNullOrEmpty(ilosc.Text) &&
                !string.IsNullOrEmpty(kodean.Text))
            {
                Int32.TryParse(ilosc.Text, out int iloscSkanowana);

                createElementDto = new CreateDokElementDto()
                {
                    DokTyp = (int)GidTyp.Mm,
                    TwrIlosc = iloscSkanowana,
                    TwrKod = twr_info.Twr_Kod,
                    TwrNazwa = twr_info.Twr_Nazwa
                };

                if ((iloscSkanowana > Int32.Parse(stan_szt)) || iloscSkanowana == 0)
                {
                    await DisplayAlert(null, "Wpisana ilość przekracza stan lub jest 0 ", "OK");
                }
                else
                {                  

                    var apiResponse = await serwisApi.SaveElement(createElementDto, elementDto.DokNaglowekId);

                    if (await serwisApi.ExistsOtherDocs(kodean.Text, elementDto.DokNaglowekId))
                        await DisplayAlert("Ostrzeżenie", "Dodawany towar znajduje się już na innej MM", "OK");

                    //int IleIstnieje = dokMM.SaveElement(dokMM);
                    var listaMMzKodem = await serwisApi.GetDokWithElementByTwrkod(kodean.Text);

                    if (listaMMzKodem.IsSuccessful || listaMMzKodem.HttpStatusCode == HttpStatusCode.NotFound)
                    {
                        var listaIstniejacych = listaMMzKodem.Data;

                        totalTwrIlosc = serwisApi.TotalTwrIloscFromAllDoks(listaIstniejacych);
                    }

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
                            var resposne = await serwisApi.UpadteElement(updatedQuantity, elementDto.DokNaglowekId, conflictInfo.IdElement);
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
                        if(apiResponse.ErrorMessage !=null)
                            await DisplayAlert("Błąd", apiResponse.ErrorMessage, "OK");
                        else
                            await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
                    }



                    //if (totalTwrIlosc > 0)
                    //{
                    //    var odp = await DisplayAlert("UWAGA!", "Dodawany kod już znajduje się na liście. Chcesz zsumować ilości?", "TAK", "NIE");
                    //    if (odp)
                    //    {
                    //        int suma = Int32.Parse(ilosc.Text) + totalTwrIlosc;
                    //        if (suma > Int32.Parse(stan_szt))
                    //        {
                    //            await DisplayAlert(null, "Łączna ilość przekracza stan ", "OK");
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            //Model.DokMM dokMM = new Model.DokMM();
                    //            dokMM.gidnumer = _gidnumer;
                    //            dokMM.twrkod = kodean.Text;
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
                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie uzupełniono wszystkich pól!", "OK");
            }
        }
        //public async void ZapiszPozycje(int mmGidnumer, string twrKod, int ilosc, int stan_szt)
        //{
        //    if ((ilosc) > (stan_szt) && (ilosc) == 0)
        //    {
        //        await DisplayAlert(null, "Wpisana ilość przekracza stan ", "OK");
        //    }
        //    else
        //    {
        //        Model.DokMM dokMM = new Model.DokMM();
        //        dokMM.gidnumer = mmGidnumer;
        //        dokMM.twrkod = twrKod;
        //        dokMM.szt = (ilosc);

        //        int IleIstnieje = dokMM.SaveElement(dokMM);

        //        if (IleIstnieje > 0)
        //        {
        //            var odp = await DisplayAlert("UWAGA!", "Dodawany kod już znajduje się na liście. Chcesz zsumować ilości?", "TAK", "NIE");
        //            if (odp)
        //            {
        //                int suma = (ilosc) + IleIstnieje;
        //                if (suma > (stan_szt))
        //                {
        //                    await DisplayAlert(null, "Łączna ilość przekracza stan ", "OK");
        //                    return;
        //                }
        //                else
        //                {
        //                    //Model.DokMM dokMM = new Model.DokMM();
        //                    dokMM.gidnumer = mmGidnumer;
        //                    dokMM.twrkod = twrKod;
        //                    dokMM.szt = suma;// Convert.ToInt32(ilosc.Text);
        //                    dokMM.UpdateElement(dokMM);
        //                    dokMM.getElementy(_gidnumer);
        //                }
        //            }
        //            else
        //            {

        //                await DisplayAlert("Uwaga", "Dodanie towaru odrzucone", "OK");
        //            }
        //        }
        //        await Navigation.PopModalAsync();
        //    }

        //}
        private async void Btn_Zapisz_Clicked(object sender, EventArgs e)
        {
            await ZapiszPozycje();

        }


        public async Task SkanowanieEan()
        {

            if (await SettingsPage.SprConn())
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
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                        {
                            if (scanPage.IsScanning) scanPage.AutoFocus(); return true;
                        });
                        await Navigation.PopModalAsync();
                        twr_info = await pobierztwrkod(result.Text);

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

        private async void Btn_Skanuj_Clicked(object sender, EventArgs e)
        {
            await SkanowanieEan();
        }

        public async Task<TwrInfo> pobierztwrkod(string _ean)
        {
            TwrInfo product = null;

            var karta = new TwrKodRequest()
            {
                Twrcenaid = 3,//todo : to powinna być cena z ustawienia
                Twrkod = "",
                Twrean = _ean
            };

            var request = new RestRequest("/api/gettowar")
                  .AddJsonBody(karta);

            try
            {

                var response = await _client.ExecutePostAsync<List<TwrInfo>>(request);

                if (response.IsSuccessful)
                {
                    product = response.Data.FirstOrDefault();

                    twrkod = product.Twr_Kod;
                    stan_szt = product.Stan_szt.ToString();
                    twr_url = product.Twr_Url;
                    twr_nazwa = product.Twr_Nazwa;
                    twr_symbol = product.Twr_Symbol;
                    twr_ean = product.Twr_Ean;

                }
                else
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        string Webquery = "cdn.pc_pobierztwr '" + _ean + "'";
                        var dane = await App.TodoManager.PobierzTwrAsync(Webquery);
                        if (dane != null)
                        {
                            twrkod = dane.Twr_Kod;
                            twr_url = dane.Twr_Url;
                            twr_nazwa = dane.Twr_Nazwa;
                            twr_ean = dane.Twr_Ean;
                            //twr_cena = dane[0].cena;
                        }

                        var mess = $"{(HttpStatusCode)response.StatusCode} : {response.Content}";
                        //await DisplayAlert("", mess, "OK");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                        var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";
                        await DisplayAlert("", mess, "OK");
                    }

                }
            }
            catch (Exception ex)
            {
                // Obsłuż błędy żądania HTTP
                var dsa = ex.Message;
            }


            kodean.Text = twrkod;
            ean.Text = twr_ean;
            symbol.Text = twr_symbol;
            nazwa.Text = twr_nazwa;
            stan.Text = "Stan : " + stan_szt;
            foto.Source = twr_url;

            return product;
        }

        public async void GetDataFromTwrKod(string _twrkod, bool CzyFoto)
        {

            var karta = new TwrKodRequest()
            {
                Twrcenaid = 3,
                Twrkod = _twrkod

            };

            var request = new RestRequest("/api/gettowar")
                  .AddJsonBody(karta);

            try
            {

                var response = await _client.ExecutePostAsync<List<TwrInfo>>(request);

                if (response.IsSuccessful)
                {
                    var product = response.Data.FirstOrDefault();

                    twrkod = product.Twr_Kod;
                    stan_szt = product.Stan_szt.ToString();
                    twr_url = product.Twr_Url;
                    twr_nazwa = product.Twr_Nazwa;
                    twr_symbol = product.Twr_Symbol;
                    twr_ean = product.Twr_Ean;

                }
                else
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {

                        var mess = $"{(HttpStatusCode)response.StatusCode} : {response.Content}";
                        await DisplayAlert("", mess, "OK");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                        var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";
                        await DisplayAlert("", mess, "OK");

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
            catch (Exception ex)
            {
                // Obsłuż błędy żądania HTTP
                var dsa = ex.Message;
            }

        }
    }
}
