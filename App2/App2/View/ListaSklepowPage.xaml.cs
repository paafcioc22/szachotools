using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaSklepowPage : ContentPage
    {
        private string opcja;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            szukaj.Focus();
        }

        public ObservableCollection<string> Items { get; set; }
        public static ObservableCollection<Model.ListaSklepow> listaSklepows;

        public ListaSklepowPage(string param)
        {
            InitializeComponent();
             listaSklepows = new ObservableCollection<Model.ListaSklepow>();
            opcja = param;
            var app = Application.Current as App;
            try
            {
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = "SERVER=" + app.Serwer +
                ";DATABASE=" + app.BazaProd +
                ";TRUSTED_CONNECTION=No;UID=" + app.User +
                ";PWD=" + app.Password
                };
                connection.Open();
                command.CommandText = "select [adr_magkod] mag_kod,[adr_magnazwa] mag_nazwa, adr_magnumer " +
                    "from [CDN].[Joart_adresy] order by 1";
     
                //sprSymSezon();
                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                while (rs.Read())
                {
                    
                    listaSklepows.Add(new Model.ListaSklepow
                    {
                        mag_kod = Convert.ToString(rs["mag_kod"]),
                        mag_nazwa = Convert.ToString(rs["mag_nazwa"]),
                        mag_gidnumer = Convert.ToString(rs["adr_magnumer"])
                         
                    });
                    
                }
                rs.Close();
                rs.Dispose();
                connection.Close();
               
            }
            catch (Exception ex)
            {
                DisplayAlert("Uwaga", ex.Message, "OK");
            }

			MyListView.ItemsSource = listaSklepows;
            szukaj.Focus();
        }


        public static IEnumerable<Model.ListaSklepow> Getsklep(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return listaSklepows;
            return listaSklepows.Where(c => c.mag_kod.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));
        }

        public ListView ListViewSklepy { get { return MyListView; } }

        //async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;

        //    var sklep = e.Item as Model.ListaSklepow;

        //    //await DisplayAlert("Kliknięto :", sklep.mag_kod, "OK");
        //    if (opcja == "edytuj")
        //    { AddElementMMList._magDcl.Text = sklep.mag_kod; }
        //    //{ AddElementMM._magDcl.Text = sklep.mag_kod; }
        //    else
        //    { AddMMPage._magDcl.Text = sklep.mag_kod; }
        //    //await DisplayAlert(null, "An item was tapped.", "OK");
        //    await Navigation.PopModalAsync();
        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}

        private void szukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyListView.ItemsSource = Getsklep(e.NewTextValue);
        }
    }
}
