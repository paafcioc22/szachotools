using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

        public  StanyTwrInnych(string TwrGidnumer)
        {
            InitializeComponent();
            PobierzDaneDoListy(TwrGidnumer);  
        }

        private async void PobierzDaneDoListy(string twrGidnumer)
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

        private   void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
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
                string tele = GetNumberTel(tel.MagKod);
                Launcher.OpenAsync($"tel:{tele}") ;
                //await DisplayAlert(null, $"Ta funkcja pozwoli wykonać tel do {tel.Magazyn}", "OK");

                //Deselect Item
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception s)
            {

                DisplayAlert(null, s.Message, "oik");
            }
        }
        string nrtelefonu;
        string GetNumberTel(string mag_kod)
        {

            var app = Application.Current as App;

            
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = "SERVER=" + app.Serwer +
                    ";DATABASE=" + app.BazaProd +
                    ";TRUSTED_CONNECTION=No;UID=" + app.User +
                    ";PWD=" + app.Password
                };
                connection.Open();
                command.CommandText = $@"SELECT  adr_tel
                                          FROM CDN.Joart_adresy
                                          where adr_magkod='{mag_kod}'";


                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                while (rs.Read())
                {
                     nrtelefonu = Convert.ToString(rs["adr_tel"]);
                }

                rs.Close();
                rs.Dispose();
                connection.Close();

            return nrtelefonu;
        }

        public static IEnumerable<Model.Magazynn> Getsklep(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return listaSklepows;
            //return listaSklepows.Where(c => c.Magazyn.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));
            return listaSklepows.Where(c => c.Magazyn.Contains(searchText.ToUpper()));
        }


        private void szukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyListView.ItemsSource = Getsklep(e.NewTextValue);
        }
    }
}

 