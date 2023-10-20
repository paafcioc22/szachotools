﻿using App2.View.Foto;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
        private bool connected;
        public static string user;
        public StartPage ()
		{
			InitializeComponent ();
            this.BindingContext = this;
            this.IsBusy = false;
         
             
        }

        public static bool CzyPrzyciskiWlaczone;


      
        public static bool CheckInternetConnection()
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }




            //string CheckUrl = "http://google.com";

            //try
            //{
            //    HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

            //    iNetRequest.Timeout = 4000;

            //    WebResponse iNetResponse = iNetRequest.GetResponse();

            //    iNetResponse.Close();

                
            //    return true;

            //}
            //catch (WebException )
            //{
            //      return false;
            //}
        }

        private async void sprwersja()
        {
            try
            {
                if (CheckInternetConnection())
                {

                    var version = DependencyService.Get<Model.IAppVersionProvider>();
                    var versionString = version.AppVersion;
                    var bulidVer = version.BuildVersion;
                    //lbl_appVersion.Text = versionString;
                    //_version = bulidVer;

                    var AktualnaWersja = await App.TodoManager.GetBuildVer();

                    if (bulidVer < Convert.ToInt16(AktualnaWersja))
                    // await DisplayAlert(null, "Używana wersja nie jest aktualna", "OK");
                    {
                        var update = await DisplayAlert("Nowa wersja", "Dostępna nowa wersja..Chcesz pobrać(zalecane)??", "Tak", "Nie");

                        if (update)
                        {
                            await version.OpenAppInStore();

                        }
                        else
                        {
                            blokujPrzyciski();
                        }
                        // blokujPrzyciski();
                    }
                }
                else
                {
                   await DisplayAlert("Uwaga", "Brak połączenia z internetem", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Uwaga", "Brak połączenia z internetem", "OK");
            }
        }

        public void blokujPrzyciski()
        {
            //IsEnabled = false;
         
            btn_CreateMM.IsEnabled = false;
            btn_przyjmijMM.IsEnabled = false;
            btn_ListaAkcji.IsEnabled = false;
        }

        public void OdblokujPrzyciski()
        {

            btn_CreateMM.IsEnabled = true;
            btn_przyjmijMM.IsEnabled = true;
            btn_ListaAkcji.IsEnabled = true;

        }





        protected   override void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(user) && user!= "Wylogowany" )
            {
                lbl_user.Text = "Zalogowany : "+ user; //dodałem zalogowane
            }

            if ( user == "Wylogowany")
            {
                lbl_user.Text = "Wylogowany" ; //dodałem zalogowane
            }


            //sprwersja();
              
                if (string.IsNullOrEmpty(lbl_user.Text) || (lbl_user.Text == "Wylogowany"))
                {
                    blokujPrzyciski();
                }
                else
                {
                    OdblokujPrzyciski();
                }

                
        }
        //przyjmij mm
        private async void BtnCreateMm_Clicked(object sender, EventArgs e)
        {
            btn_CreateMM.IsEnabled = false;
            connected = await SettingsPage.SprConn();
            if (connected)
            {
               await Navigation.PushModalAsync(new View.StartCreateMmPage());
                btn_CreateMM.IsEnabled = true;

            }
            else
               await DisplayAlert(null, "Brak połączenia z serwisem", "OK");
            btn_CreateMM.IsEnabled = true;

        }
        //weryfikator
        private async void SkanTwr_Clicked(object sender, EventArgs e)
        {
            btn_weryfikator.IsEnabled = false;
                    
            await Navigation.PushModalAsync(new View.WeryfikatorCenPage());

            
            btn_weryfikator.IsEnabled = true;

        }
        //utwórz MM
        private async void BtnListaMMp_Clicked(object sender, EventArgs e)
        {
            //IsBusy = true;
            //try
            //{
                btn_przyjmijMM.IsEnabled = false;
                WaitIco.IsRunning = true;
                connected = await SettingsPage.SprConn();
                if (connected)
                {
                    await Navigation.PushAsync(new PrzyjmijMM_ListaMMDoPrzyjecia());
                    //IsBusy = false;
                    WaitIco.IsRunning = false;
                    btn_przyjmijMM.IsEnabled = true;

                }
                else
                    await DisplayAlert(null, "Brak połączenia z siecią", "OK");
                btn_przyjmijMM.IsEnabled = true;
            //}
            //catch (Exception x)
            //{
            //   await DisplayAlert(null, x.Message, "OK");
            //}
            //Navigation.PushModalAsync(new ListaMMP("0101604002410401032"));
        }

        private async void Btn_Login_Clicked(object sender, EventArgs e)
        {
            btn_login.IsEnabled = false;
            connected =await SettingsPage.SprConn();
            if (connected)
            {

                // await Navigation.PushModalAsync(new LoginLista());

                var page = new LoginLista();

                page.ListViewLogin.ItemSelected += async (source, args) =>
                {
                    var pracownik = args.SelectedItem as Pracownik;
                    if (SettingsPage.SelectedDeviceType == 1)
                    {
                        page.SkanujIdetyfikator();
                    }
                    else {
                        page.enthaslo.Focus();
                    }

                    
                    if(await page.IsPassCorrect(pracownik.opegidnumer))
                        lbl_user.Text = "Zalogowany : " + pracownik.opekod; ;
                   // Navigation.PopModalAsync();
                };

               await Navigation.PushModalAsync(page);

                btn_login.IsEnabled = true;

                 
            }
            else
                await DisplayAlert(null, "Brak połączenia z siecią", "OK");
            btn_login.IsEnabled = true;

              
        }

        private async void Btn_ListAkcje_Clicked(object sender, EventArgs e)
        {
            btn_ListaAkcji.IsEnabled = false;
            connected = await SettingsPage.SprConn();
            if (connected)
            { 
                   
                //await Navigation.PushModalAsync(new List_AkcjeView());
                await Navigation.PushAsync(new List_AkcjeView());

                btn_ListaAkcji.IsEnabled = true; 
            }
            else
                await DisplayAlert(null, "Brak połączenia z siecią", "OK");
            btn_ListaAkcji.IsEnabled = true;


        }


        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
 
            if (_userTapped)
                return;

            _userTapped = true;

            //await Navigation.PushModalAsync(new PrzyjmijMM_ListaElementowMM(null, mm.GIDdokumentuMM));
            await Navigation.PushAsync(new SettingsPage());

            _userTapped = false;

        
        }

        private bool _userTapped;
        private async void Btn_settings_Tapped(object sender, EventArgs e)
        {
             
            if (_userTapped)
                return;

            _userTapped = true;

            await Navigation.PushAsync(new View.SettingsPage());

            _userTapped = false;
        }

        private async void Help_Clicked(object sender, EventArgs e)
        {
            if (_userTapped)
                return;

            _userTapped = true;

            await Launcher.OpenAsync("http://serwer.szachownica.com.pl:81/zdjecia/Instrukcja_Aplikacji_SzachoTools.pdf");
            //await Navigation.PushAsync(new View.widoktestowy());
            _userTapped = false;
        }

        private bool maybe_exit = false;
        protected override bool OnBackButtonPressed()
        //-------------------------------------------------------------------
        {
            //some more custom checks here
            //..

            if (maybe_exit) return false; //QUIT

            DependencyService.Get<Model.IAppVersionProvider>().ShowLong ("Wciśnij jeszcze raz by wyjść z aplikacji");
            maybe_exit = true;

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                maybe_exit = false; //reset those 2 seconds
                                    // false - Don't repeat the timer 
                return false;
            });
            return true; //true - don't process BACK by system
        }

        private async void BtnCreatePaczka_Clicked(object sender, EventArgs e)
        {

            if (_userTapped)
                return;

            _userTapped = true;
            connected = await SettingsPage.SprConn();
            if (connected)
            {
                await Navigation.PushAsync(new View.CreatePaczkaListaZlecen());

            }
            _userTapped = false;
        }

        private async void btn_zdjecia_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PushAsync(new Login());
             
        } 
         

    }

}