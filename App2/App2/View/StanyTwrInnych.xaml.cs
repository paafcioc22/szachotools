using App2.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StanyTwrInnych : ContentPage
    {
        public static ObservableCollection<Model.Magazynn> listaSklepows;
        private static RestClient _client;
        private readonly string twrGidnumer;

        public  StanyTwrInnych(string TwrGidnumer)
        {
            InitializeComponent();
 
            var app = Application.Current as App;
            _client = new RestClient($"http://{app.Serwer}");
            twrGidnumer = TwrGidnumer;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await PobierzDaneDoListy(twrGidnumer);
            }
            catch (Exception ex)
            {
                // Odpowiednie logowanie błędów lub wyświetlenie komunikatu
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }


        private async Task PobierzDaneDoListy(string twrGidnumer)
        {
            string query = "CDN.StanyTowaruWOddzialach_int " + twrGidnumer;
            try
            {

                var Maglista = await App.TodoManager.GetTodoItemsAsync(query);
                listaSklepows = Maglista;
                MyListView.ItemsSource = listaSklepows;
            }
            catch (Exception s)
            {
                await DisplayAlert(null, s.Message, "OK");
            }
            // MyListView.ItemsSource = Maglista;
        }

        private async  void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            try
            {

                var tel = e.Item as Model.Magazynn;
                //(75) 649-71-89
                //518 - 275 - 950
                //string telefon = "(75) 649-71-89";
                string tele = await GetNumberTel(tel.MagKod);
                await Launcher.OpenAsync($"tel:{tele}") ;
                //await DisplayAlert(null, $"Ta funkcja pozwoli wykonać tel do {tel.Magazyn}", "OK");

                //Deselect Item
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception s)
            {

                await DisplayAlert(null, s.Message, "oik");
            }
        }
    
        async Task<string> GetNumberTel(string mag_kod)
        {
                      
            try
            {
                var request = new RestRequest($"/api/getMagazynInfo/{mag_kod}");

                var response = await _client.ExecuteGetAsync<Magazynn>(request);


                if (response.IsSuccessful)
                {
                    return response.Data.Telefon;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }

        
        }

        public static IEnumerable<Model.Magazynn> Getsklep(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return listaSklepows;
            //return listaSklepows.Where(c => c.Magazyn.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));

            if(listaSklepows != null)
                return listaSklepows.Where(c => c.Magazyn.Contains(searchText.ToUpper()));
            return null;
        }


        private void szukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(MyListView.ItemsSource is IEnumerable<Magazynn> items && items.Any())
            {
                MyListView.ItemsSource = Getsklep(e.NewTextValue);
            }
        }
    }
}

 