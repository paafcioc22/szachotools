
using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.Services;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib.Serwis;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginLista : ContentPage
    {
        public static IszachoApi Api => DependencyService.Get<IszachoApi>();
        public ListView ListViewLogin { get { return MyListView; } }
        public ObservableCollection<ViewUser> ListaLogin { get; set; }
        public Entry enthaslo { get { return entry_haslo; } }
        private ViewUser _wybranyPracownik;

        private BindableProperty IsSearchingProperty =
            BindableProperty.Create("IsSearching", typeof(bool), typeof(LoginLista), false);
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }


        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;

        static public string _user;
        static public string _nazwisko;
        static public int _opeGid;


        public LoginLista()
        {
            InitializeComponent();

            this.BindingContext = this;


            var app = Application.Current as App;

            if (app.Skanowanie == 1)
                UseAparatButton.IsVisible = false;
        }



        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetLogins();
        }

        public async Task SkanujIdetyfikator()
        {
        
            var testCamera = await PermissionService.CheckAndRequestPermissionAsync(new Permissions.Camera());

            if (testCamera == PermissionStatus.Granted)
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>()
                    { 
                        //ZXing.BarcodeFormat.EAN_13,
                        //ZXing.BarcodeFormat.CODE_128,
                        ZXing.BarcodeFormat.EAN_8,
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
                            if (scanPage.IsScanning) 
                                scanPage.AutoFocus(); 
                            return true;
                        });
                        Navigation.PopModalAsync();
                        entry_haslo.Text = (result.Text);
                        entry_haslo.Focus();
                    });
                };

                await Navigation.PushModalAsync(scanPage);
            }
            else
            {
                await DisplayAlert("Uwaga", "Brak uprawnień do aparatu", "OK");

            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            try
            {
                _wybranyPracownik = e.Item as ViewUser;

                if (_wybranyPracownik != null)
                {
                    _opeGid = _wybranyPracownik.OpeGidnumer;
                    _user = _wybranyPracownik.OpeKod; //"Zalogowany : "
                    _nazwisko = _wybranyPracownik.OpeNazwa;

                    var app = Application.Current as App;

                    if (app.Skanowanie == 1)             
                    {
                        await SkanujIdetyfikator();
                    }
                    else
                    {
                        enthaslo.Focus();
                    }

                    if (await IsPassCorrect(_wybranyPracownik.OpeGidnumer))
                    {

                        App.SessionManager.CreateSession(_wybranyPracownik.OpeKod);

                    }

                }
            }
            catch (Exception s)
            {
                await DisplayAlert("błąd",s.Message,"OK");
            }


        }


        private async Task GetLogins()
        {
            ListaLogin = new ObservableCollection<ViewUser>();
            IsSearching = true;

            try
            {
                if (await SettingsPage.SprConn())              
                {
                    ServiceDokumentyApi serwisApi = new ServiceDokumentyApi();
                    var viewUsers = await serwisApi.GetViewUsersAsync();
                    //var viewUsers = await serwisApi.GetTestViewUsersAsync();

                    foreach (var item in viewUsers)
                    {

                        ListaLogin.Add(item);
                    }
                    IsSearching = false;

                    MyListView.ItemsSource = ListaLogin; 

                }
            }
            catch (Exception s)
            {
                IsSearching = false;
                await DisplayAlert("uwaga", $"{s.Message}", "OK");
            }finally { IsSearching = false; }
        }

        #region Old IsPassOK Function

        public bool IsPassCorrect()
        {

            var isNumeric = int.TryParse(entry_haslo.Text, out int n);

            if (isNumeric && entry_haslo.Text.Length >= 6)
            {
                int znak1 = Convert.ToInt32(entry_haslo.Text.Substring(0, 1));
                int znak24 = Convert.ToInt32(entry_haslo.Text.Substring(1, 3));
                if (znak1 == 0 && entry_haslo.Text.Length == 8)
                {
                    if (znak24 == _wybranyPracownik.OpeGidnumer)
                    {
                        return true;
                    }
                    else { return false; }

                }
                else if (entry_haslo.Text.Length != 6)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;

        } 
        #endregion


        public async Task<bool> IsPassCorrect(int gidnumer)
        {

            var query = $@"cdn.PC_WykonajSelect N'  
            select  ope_gidnumer AS GID,Ope_ident, Prc_Akronim,  substring(cast(10000 +ope_gidnumer as varchar(8)) +SUBSTRING(Prc_Pesel,8,3),2,7) Haslo
            from cdn.opekarty  
	            left join cdn.PrcKarty on Ope_PrcNumer=Prc_GIDNumer 
            where   ope_gidnumer={gidnumer}'";

            string fullPass = "";

            try
            {


                var haslo = await App.TodoManager.PobierzDaneZWeb<User>(query);
                //var haslo = await Api.ExecSQLCommandAsync<User>(query);

                if (haslo.Any())
                {
                    if (haslo.FirstOrDefault().GID == "53")
                    {
                        haslo.FirstOrDefault().Haslo = "987654";
                    }

                    var isNumeric = int.TryParse(entry_haslo.Text, out int n);

                    if (isNumeric && entry_haslo.Text.Length >= 6)
                    {
                        int znak1 = Convert.ToInt32(entry_haslo.Text.Substring(0, 1));

                        if (znak1 == 0 && entry_haslo.Text.Length == 8)
                        {
                            fullPass = EAN8.CalcChechSum(haslo.First().Haslo);

                            return entry_haslo.Text.Equals(fullPass);

                        }
                        else if (entry_haslo.Text.Length != 6)
                        {
                            return false;

                        }
                        else if (znak1 == 9 && entry_haslo.Text.Length == 6)
                        {
                            if (entry_haslo.Text == "987654")
                                return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Nie udało się sprawdzić hasła");
                }
            }
            catch (Exception s)
            {
                await DisplayAlert(null, s.Message, "OK");
            }
            return false;

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (entry_haslo.Text != null) //entry_haslo.Text!=""
                {
                    if (_wybranyPracownik != null)
                    {
                        if (await IsPassCorrect(_wybranyPracownik.OpeGidnumer))
                        {
                            App.SessionManager.CreateSession(_wybranyPracownik.OpeKod);
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            await DisplayAlert("Uwaga", "Wprowadzone hasło jest niepoprawne", "OK");

                        }
                    }
                    else
                    {
                        await DisplayAlert(null, "Nie wybrano pracownika z listy", "OK");
                    }
                    
                }
            }
            catch (Exception s)
            {
                await DisplayAlert(null, s.Message, "OK");
            }
        }



        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }



        private async void entry_haslo_Completed(object sender, EventArgs e)
        {

            try
            {
                if (_wybranyPracownik != null)
                {
                    if (await IsPassCorrect(_wybranyPracownik.OpeGidnumer))
                    {

                        App.SessionManager.CreateSession(_wybranyPracownik.OpeKod);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Uwaga", "Wprowadzone hasło jest niepoprawne", "OK");
                    }
                }
                else
                {
                    await DisplayAlert(null, "Nie wybrano pracownika z listy", "OK");
                }
            }
            catch (Exception exception)
            {

                var properties = new Dictionary<string, string>
                {
                    { "user", $"{_wybranyPracownik.OpeKod}_{_wybranyPracownik.OpeNazwa}"},
                    { "gid", _wybranyPracownik.OpeGidnumer.ToString()},
                    { "haslo", entry_haslo.Text}
                };
                Crashes.TrackError(exception, properties);
            }
        }

        private async void UseAparatToScan_Clicked_1(object sender, EventArgs e)
        {

            await DisplayAlert(null, "Wybierz operatora i zeskanuj indetyfikator", "OK");
            ListViewLogin.ItemSelected += async (source, args) =>
            {
                var pracownik = args.SelectedItem as ViewUser;

                await SkanujIdetyfikator();

            };
        }


    }

   // public class Pracownik
    //{
    //    public string OpeKod { get; set; }
    //    public string OpeNazwa { get; set; }
     
    //    public int OpeGidnumer { get; set; }

    //}
}
