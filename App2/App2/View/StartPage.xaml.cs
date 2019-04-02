using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
        private bool connected;
        public static string user="";
        public StartPage ()
		{
			InitializeComponent ();
            this.BindingContext = this;
            this.IsBusy = false;
            //this.IsEnabled = false;
 
            // sprwersja();
        }

        public static bool CzyPrzyciskiWlaczone;


      
        public bool CheckInternetConnection()
        {
            string CheckUrl = "http://google.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl); 
                iNetRequest.Timeout = 2000; 
                WebResponse iNetResponse = iNetRequest.GetResponse(); 
                iNetResponse.Close();  
                return true;

            }
            catch (WebException )
            {
                  return false;
            }
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
                    if (bulidVer != AktualnaWersja)
                    // await DisplayAlert(null, "Używana wersja nie jest aktualna", "OK");
                    {
                        var update = await DisplayAlert("Nowa wersja", "Dostępna nowa wersja..Chcesz pobrać??", "Tak", "Nie");

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
        }

        public void OdblokujPrzyciski()
        {

            btn_CreateMM.IsEnabled = true;
            btn_przyjmijMM.IsEnabled = true;
        }


        private bool _userTapped;
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs args)
        {
            //  string query = "[CDN].[PC_WykonajSelect] 58611";
            //  var Maglista =  App.TodoManager.InsertDataNiezgodnosci(query); 


            if (_userTapped)
                return;

            _userTapped = true;
             
                await Navigation.PushModalAsync(new View.SettingsPage());

            _userTapped = false;


        }

        protected override void OnAppearing()
        {
            lbl_user.Text = user;

           
                sprwersja();

                if ((lbl_user.Text == "") || (lbl_user.Text is null) || (lbl_user.Text == "Wylogowany"))
                {
                    blokujPrzyciski();
                }
                else
                {
                    OdblokujPrzyciski();
                }
               
                
        }

        private async void BtnCreateMm_Clicked(object sender, EventArgs e)
        {
            btn_CreateMM.IsEnabled = false;
            connected = SettingsPage.SprConn();
            if (connected)
            {
               await Navigation.PushModalAsync(new View.StartCreateMmPage());
                btn_CreateMM.IsEnabled = true;

            }
            else
               await DisplayAlert(null, "Brak połączenia z siecią", "OK");
            btn_CreateMM.IsEnabled = true;

        }

        private async void SkanTwr_Clicked(object sender, EventArgs e)
        {
            btn_weryfikator.IsEnabled = false;
            await Navigation.PushModalAsync(new View.WeryfikatorCenPage());
            btn_weryfikator.IsEnabled = true;

        }

        private async void BtnListaMMp_Clicked(object sender, EventArgs e)
        {
            //IsBusy = true;
            //try
            //{
                btn_przyjmijMM.IsEnabled = false;
                WaitIco.IsRunning = true;
                connected = SettingsPage.SprConn();
                if (connected)
                {
                    await Navigation.PushModalAsync(new PrzyjmijMM_ListaMMDoPrzyjecia());
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
            connected = SettingsPage.SprConn();
            if (connected)
            {
                 
                await Navigation.PushModalAsync(new LoginLista());
                btn_login.IsEnabled = true;

                //WaitIco.IsRunning = false;
            }
            else
                await DisplayAlert(null, "Brak połączenia z siecią", "OK");
            btn_login.IsEnabled = true;

        }






        //       SELECT* 0101604002406956344
        //FROM[CDN_Joart_ZRO].[CDN].[DefCeny]
        //       where DfC_Nieaktywna = 0



        // select DfC_Nazwa, twr_kod, twr.*  
        // from cdn.twrceny twr
        // join cdn.[DefCeny] on twr.TwC_TwCNumer= DfC_DfCId
        // join cdn.Towary on twr_twrid = twc_twrid

        //public bool SprConn() //Third way, slightly slower than Method 1
        //{
        //    //  NadajWartosci();
        //    var connStr = new SqlConnectionStringBuilder
        //    {
        //        DataSource = SettingsPage._serwer,
        //        InitialCatalog = SettingsPage._database,
        //        UserID = SettingsPage._uid,
        //        Password = SettingsPage._pwd,
        //        ConnectTimeout = 3
        //    }.ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            //DisplayAlert("Connected", "Połączono z siecia", "OK");
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
        //            return false;
        //        }
        //    }
        //}

    }

}