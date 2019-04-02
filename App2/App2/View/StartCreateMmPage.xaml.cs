using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartCreateMmPage : ContentPage
	{
        private DokMMViewModel DokMMViewModel;
        Model.DokMM DokMM;
        public static int flagaMM;
        public StartCreateMmPage ()
		{
			InitializeComponent ();
            //    SettingsPage settingsPage = new SettingsPage();
              
              //  if (SprConn())
              //  {
                    DokMMViewModel = new ViewModel.DokMMViewModel();
                    DokMM = new Model.DokMM();
                    BindingContext = Model.DokMM.dokMMs; //DokMMViewModel.dokMMs; 
                    ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
                    flagaMM = 1;
                    StartTimer(true);
               // }
               // else
               // {
               //     DisplayAlert(null, "Brak połączenia", "OK");
              //  }
        }
        public  void StartTimer(bool status)
        {
            
            Device.StartTimer(System.TimeSpan.FromSeconds(7), () =>
            {
                Device.BeginInvokeOnMainThread(UpdateUserDataAsync);
                if ( flagaMM == 0)
                {
                    return false;
                }
                else
                {
                     return true;
                }
            }); 
            
        }

        private void UpdateUserDataAsync()
        {
            Model.DokMM dokMM = new Model.DokMM();

            ListaMMek.ItemsSource = dokMM.getMMki();
        }

        private async void Btn_AddMm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new View.AddMMPage());

            DokMMViewModel = new ViewModel.DokMMViewModel();
            ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            DokMMViewModel = new ViewModel.DokMMViewModel();
            ListaMMek.ItemsSource = null;
            ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
        }

        private async void ListaMMek_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (SprConn())
            {
                var mmka = e.Item as Model.DokMM;
                //await DisplayAlert("Kliknięto :", mmka.gidnumer.ToString(), "OK");
                ((ListView)sender).SelectedItem = null;
                Model.DokMM dokMM = new Model.DokMM();
                dokMM.getElementy(mmka.gidnumer);
                await Navigation.PushModalAsync(new View.AddElementMMList(mmka));
                //flagaMM = 0;
                //StartTimer(false);
            }
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert(null, "Czy chcesz usunąć MM z listy?", "Tak", "Nie");
            if (action)
            {
                var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
                Model.DokMM.dokMMs.Remove(usunMM);
                Model.DokMM.DeleteMM(usunMM);
            }
        }

        public bool SprConn() //Third way, slightly slower than Method 1
        {
            //  NadajWartosci();
            var app = Application.Current as App;
            var connStr = new SqlConnectionStringBuilder
            {
                DataSource = app.Serwer,//SettingsPage._serwer,
                InitialCatalog = app.BazaProd,//SettingsPage._database,
                UserID = app.User,//SettingsPage._uid,
                Password = app.Password,//SettingsPage._pwd,
                ConnectTimeout = 3
            }.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    //DisplayAlert("Connected", "Połączono z siecia", "OK");
                    return true;
                }
                catch (Exception )
                {
                    //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
                    return false;
                }
            }
        }
    }
}