using App2.Model;
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
    public partial class List_AkcjeAfterFiltr : ContentPage
    {
        public ObservableCollection<Model.AkcjeNagElem> Items;
        IList<Model.AkcjeNagElem> _listatwr;
        SqlConnection connection;
        public List_AkcjeAfterFiltr(IList<Model.AkcjeNagElem> nowa)
        {
            Items = new ObservableCollection<Model.AkcjeNagElem>();
          
            InitializeComponent();
            BindingContext = this;
            _listatwr = nowa;
            Items = Convert(nowa);

            var app = Application.Current as App;
            connection = new SqlConnection
            {
                ConnectionString = "SERVER=" + app.Serwer +
                ";DATABASE=" + app.BazaProd +
                ";TRUSTED_CONNECTION=No;UID=" + app.User +
                ";PWD=" + app.Password
            };


            MyListView.ItemsSource = Items;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = this;
             
            var tmp= Items.OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();
            MyListView.ItemsSource = Convert(tmp);


            //if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                SendDataSkan(tmp);
        }


        Int16 magnumer;
        private async void SendDataSkan(IList<AkcjeNagElem> sumaList)
        {
            SqlCommand command = new SqlCommand();

            connection.Open();
            command.CommandText = $@"SELECT  [Mag_GIDNumer]
                                  FROM  [CDN].[Magazyny]
                                  where mag_typ=1";


            SqlCommand query = new SqlCommand(command.CommandText, connection);
            SqlDataReader rs;

            rs = query.ExecuteReader();
            while (rs.Read())
            {
                magnumer = System.Convert.ToInt16(rs["Mag_GIDNumer"]);
            }

            rs.Close();
            rs.Dispose();
            connection.Close();
            string ase_operator = View.LoginLista._user + " " + View.LoginLista._nazwisko;
            var odp = await App.TodoManager.InsertDataSkan(sumaList, magnumer, ase_operator);
            if (odp != "OK")
                await DisplayAlert(null, odp, "OK");
        }


        public ObservableCollection<T> Convert<T>(IList<T> original)
        {
            return new ObservableCollection<T>(original);
        }


        private bool _istapped;
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (_istapped)
                return;

            _istapped = true;

            var pozycja = e.Item as Model.AkcjeNagElem;

            await Navigation.PushModalAsync(new List_ScanPage(pozycja));

            _istapped = false;

            ((ListView)sender).SelectedItem = null;
        }


        public   IEnumerable<Model.AkcjeNagElem> SzukajTowar(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return _listatwr;
            return _listatwr.Where(c => c.TwrKod.Contains(searchText.ToUpper()));
        
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyListView.ItemsSource = SzukajTowar(e.NewTextValue);
        }
    }
}
