using App2.Services;
using App2.View.Foto;
using App2.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
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
        StartPageViewModel viewModel;
        public StartPage()
        {
            InitializeComponent();

            this.BindingContext =
            viewModel = new StartPageViewModel();

        }


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

        }

        private async Task CheckMinVersionSzachotools()
        {
            var version = DependencyService.Get<IAppVersionProvider>();

            try
            {
                var versionString = version.AppVersion;
                var bulidVer = version.BuildVersion;
              

                var AktualnaWersja = await App.TodoManager.GetBuildVer();

                if (bulidVer < Convert.ToInt16(AktualnaWersja))
                {
                    var update = await DisplayAlert("Nowa wersja", "Dostępna nowa wersja..Chcesz pobrać(zalecane)??", "Tak", "Nie");

                    if (update)
                    {
                        await version.OpenAppInStore();

                    }
                    else
                    {
                        viewModel.IsButtonsEnabled=false;
                    }

                }

            }
            catch (Exception)
            {
                version.ShowShort("Błąd pobierania wersji aplikacji");
                //await DisplayAlert("Uwaga", "Brak połączenia z internetem", "OK");
            }
        }

        //public void blokujPrzyciski()
        //{
        //    //IsEnabled = false;

        //    btn_CreateMM.IsEnabled = false;
        //    btn_przyjmijMM.IsEnabled = false;
        //    btn_ListaAkcji.IsEnabled = false;
        //}

        //public void OdblokujPrzyciski()
        //{

        //    btn_CreateMM.IsEnabled = true;
        //    btn_przyjmijMM.IsEnabled = true;
        //    btn_ListaAkcji.IsEnabled = true;

        //}





        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //var currentSession = App.SessionManager.CurrentSession;
            //if (currentSession != null)
            //{
            //    // Ustawienie UserName w ViewModel
            //    var viewModel = BindingContext as StartPageViewModel;
            //    if (viewModel != null)
            //    {
            //        // viewModel.UserName = currentSession.UserName;
            //        //OdblokujPrzyciski();
            //    }
            //}
            //else
            //{
            //    //blokujPrzyciski();
            //    // Opcjonalnie: przekierowanie do strony logowania lub wyświetlenie komunikatu
            //    // await Navigation.PushAsync(new LoginPage());
            //}

            await CheckMinVersionSzachotools();

            //if (!string.IsNullOrEmpty(user) && user != "Wylogowany")
            //{
            //    lbl_user.Text = "Zalogowany : " + user; //dodałem zalogowane
            //}

            //if (user == "Wylogowany")
            //{
            //    lbl_user.Text = "Wylogowany"; //dodałem zalogowane
            //}


            //if (string.IsNullOrEmpty(lbl_user.Text) || (lbl_user.Text == "Wylogowany"))
            //{
            //    blokujPrzyciski();
            //}
            //else
            //{
            //    OdblokujPrzyciski();
            //}

        }

        private async void BtnCreateMm_Clicked(object sender, EventArgs e)
        {
            if (isClicked) // Jeśli przycisk został już kliknięty, nie wykonuj żadnych działań.
                return;

            isClicked=true;
            try
            {
                connected = await SettingsPage.SprConn();
                if (connected)
                {
                    await Navigation.PushModalAsync(new View.StartCreateMmPage()); 

                } 
            }
            catch (Exception s)
            {
                await DisplayAlert("Błąd", s.Message, "OK");

            }
            finally
            {
                isClicked=false;
            }
         

        }
        //weryfikator
        private async void SkanTwr_Clicked(object sender, EventArgs e)
        {
            btn_weryfikator.IsEnabled = false;
            viewModel.IsBusy = true;
            try
            {
                if (await SettingsPage.SprConn())
                {
                    viewModel.IsBusy = false;
                    await Navigation.PushAsync(new View.WeryfikatorCenPage());
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "Ok");
                viewModel.IsBusy = false;
            }

            btn_weryfikator.IsEnabled = true;

        }
        //utwórz MM
        private bool isClicked = false;
        private async void BtnListaMMp_Clicked(object sender, EventArgs e)
        {

            if (isClicked) // Jeśli przycisk został już kliknięty, nie wykonuj żadnych działań.
                return;

            isClicked = true; // Ustaw flagę na true, ponieważ przycisk został kliknięty.
            viewModel.IsBusy = true;

            try
            {

                connected = await SettingsPage.SprConn();
                if (connected)
                {
                    viewModel.IsBusy = false;
                    await Navigation.PushAsync(new PrzyjmijMM_ListaMMDoPrzyjecia()); 

                }
                else
                {
                    viewModel.IsBusy = false;
                    //await DisplayAlert(null, "Brak połączenia z siecią", "OK");
                }
                 
            }
            catch (Exception x)
            {
           
                await DisplayAlert(null, x.Message, "OK");
            }
            finally
            {
                viewModel.IsBusy = false;
                isClicked = false;

            }

        }

        private async void Btn_Login_Clicked(object sender, EventArgs e)
        {
            btn_login.IsEnabled = false;
            try
            {
                var page = new LoginLista();

                //page.ListViewLogin.ItemSelected += async (source, args) =>
                //{
                //    var pracownik = args.SelectedItem as Pracownik;
                //    if (pracownik == null) throw new Exception("Pracownik jest null!");


                //    if (SettingsPage.SelectedDeviceType == 1)
                //    {
                //        page.SkanujIdetyfikator();
                //    }
                //    else
                //    {
                //        page.enthaslo.Focus();
                //    }

                //    if (await page.IsPassCorrect(pracownik.opegidnumer))
                //    {
                //        //Device.BeginInvokeOnMainThread(() =>
                //        //{
                //            //lbl_user.Text = "Zalogowany : " + pracownik.opekod;
                //            //App.SessionManager.CreateSession(pracownik.opekod);
                //        //});
                //    }
                //    //if (await page.IsPassCorrect(pracownik.opegidnumer))
                //       // lbl_user.Text = "Zalogowany : " + pracownik.opekod;

                //};

                await Navigation.PushModalAsync(page);

                btn_login.IsEnabled = true;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");

            }
            finally
            {         
                isClicked = false;
            }

            btn_login.IsEnabled = true;

        }

        private async void Btn_ListAkcje_Clicked(object sender, EventArgs e)
        {
            if (isClicked) // Jeśli przycisk został już kliknięty, nie wykonuj żadnych działań.
                return;

            isClicked = true;
            try
            {
                connected = await SettingsPage.SprConn();
                if (connected)
                {
                    //await Navigation.PushModalAsync(new List_AkcjeView());
                    await Navigation.PushAsync(new List_AkcjeView());
                  
                }
                else
                    await DisplayAlert(null, "Brak połączenia z siecią", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
            finally
            {
                isClicked = false;
            }


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

            DependencyService.Get<IAppVersionProvider>().ShowLong("Wciśnij jeszcze raz by wyjść z aplikacji");
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

            viewModel.IsBusy = true;
            _userTapped = true;
            try
            {
                connected = await SettingsPage.SprConn();
                if (connected)
                {
                    viewModel.IsBusy = false;
                    await Navigation.PushAsync(new View.CreatePaczkaListaZlecen());

                }
            }
            catch (Exception ex)
            {   
                await DisplayAlert("błąd", ex.Message, "OK");
            }finally
            { 
                _userTapped = false; 
                viewModel.IsBusy = false;
            } 
        }

        private async void btn_zdjecia_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PushAsync(new Login());

        }

        private async void logoutbutton_Clicked(object sender, EventArgs e)
        {
            var wantsToLogout = await DisplayAlert(null, "Czy chcesz się wylogować?", "Tak", "Nie");
            if (wantsToLogout)
            {
                App.SessionManager.EndSession();
            }
        }
    }

}