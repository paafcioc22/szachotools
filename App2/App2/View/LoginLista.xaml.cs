﻿
using App2.Model;
using App2.OptimaAPI;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
 
using System.IO;
 
 
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginLista : ContentPage
    {
        public   ObservableCollection<Pracownik> ListaLogin { get; set; }
      
        string konfiguracyjna;

        static string haslo;
        static public string _user;
        static public string _nazwisko;
        static public int _opeGid;
        static string haslo_chk;
         

        public ListView ListViewLogin { get { return MyListView; } }
        public Entry enthaslo { get { return entry_haslo; } }

        public LoginLista()
        {
            InitializeComponent();
            var app = Application.Current as App;
         
            konfiguracyjna = app.BazaConf;
            GetLogins();

            if (SettingsPage.SelectedDeviceType == 1)
                UseAparatButton.IsVisible = false;
        }
 
        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;



        public async void SkanujIdetyfikator()
        {
            if (await SettingsPage.SprConn())
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
                            if (scanPage.IsScanning) scanPage.AutoFocus(); return true;
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
                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            }
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var prac = e.Item as Pracownik;
            //haslo = prac.opehaslo;
            //haslo_chk = prac.opechk;
            _opeGid = prac.opegidnumer;
            _user = prac.opekod; //"Zalogowany : "
            _nazwisko = prac.openazwa;
            

           // SkanujIdetyfikator();
             
 
        }

         

        private async void GetLogins()
        {
            ListaLogin = new ObservableCollection<Pracownik>();
            
            ServiceDokumentyApi serwisApi= new ServiceDokumentyApi();
            var viewUsers= await serwisApi.GetViewUsersAsync();

            foreach (var item in viewUsers)
            {

           
                ListaLogin.Add(new Pracownik
                {
                    opekod = item.OpeKod,
                    openazwa = item.OpeNazwa,
                    //opehaslo = Convert.ToString(rs["Ope_Haslo"]),
                    //opechk = Convert.ToString(rs["Ope_HasloChk"]),
                    opegidnumer = item.OpeGidnumer                 

                 }); 
            }
         

            MyListView.ItemsSource = ListaLogin;
        }

        public bool IsPassCorrect()
        {

            var isNumeric = int.TryParse(entry_haslo.Text, out int n);

            if (isNumeric&& entry_haslo.Text.Length>=6)
            {
                int znak1 = Convert.ToInt32(entry_haslo.Text.Substring(0, 1));
                int znak24 = Convert.ToInt32(entry_haslo.Text.Substring(1, 3));
                if (znak1 == 0 && entry_haslo.Text.Length == 8)
                {
                    if (znak24 == _opeGid)
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

                if (haslo.Count > 0)
                {
                    if (haslo[0].GID == "53")
                    {
                        haslo[0].Haslo = "987654";
                    }

                    var isNumeric = int.TryParse(entry_haslo.Text, out int n);

                    if (isNumeric && entry_haslo.Text.Length >= 6)
                    {
                        int znak1 = Convert.ToInt32(entry_haslo.Text.Substring(0, 1));

                        if (znak1 == 0 && entry_haslo.Text.Length == 8)
                        {
                            fullPass = EAN8.CalcChechSum(haslo[0].Haslo);

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
                    throw new ArgumentException("Nie udało się pobrać hasła ");
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


                    if (await IsPassCorrect(_opeGid))
                    {


                        View.StartPage.user = _user;


                        View.StartPage startPage = new StartPage();
                        startPage.OdblokujPrzyciski();

                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Uwaga", "Wprowadzone hasło jest niepoprawne", "OK");

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
                if (await IsPassCorrect(_opeGid))
                {

                    View.StartPage.user = _user;

                    View.StartPage startPage = new StartPage();
                    startPage.OdblokujPrzyciski();


                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Uwaga", "Wprowadzone hasło jest niepoprawne", "OK");
                }
            }
            catch (Exception exception)
            {

                var properties = new Dictionary<string, string>
                {
                    { "user", _user},
                    { "gid", _opeGid.ToString()},
                    { "haslo", entry_haslo.Text}
                };
                Crashes.TrackError(exception, properties);
            }
        }

        private void UseAparatToScan_Clicked_1(object sender, EventArgs e)
        {

            DisplayAlert(null, "Wybierz operatora i zeskanuj indetyfikator", "OK");
            ListViewLogin.ItemSelected += (source, args) =>
            {
                var pracownik = args.SelectedItem as Pracownik;

                SkanujIdetyfikator();

            };
        }

        
    }

    public  class Pracownik
    {
        public string opekod { get; set; }
        public string openazwa { get; set; }
        //public string opehaslo { get; set; }
        //public string opechk { get; set; }
        public int  opegidnumer { get; set; }
        
    }
}
