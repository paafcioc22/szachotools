using App2.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class PrzyjmijMM_ListaElementowMM : ContentPage
    {
        public List<Model.PrzyjmijMMClass> PrzyjmijListaMM { get; set; }
        private SQLiteAsyncConnection _connection;
        private int _gidnumer;
        public PrzyjmijMM_ListaElementowMM(  int gidnumer)
        {
            InitializeComponent();

            PrzyjmijMMClass przyjmijMMClass = new PrzyjmijMMClass();
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            _gidnumer = gidnumer;
            
          
           

        }

        protected override async void OnAppearing()
        {
            PrzyjmijMMClass przyjmijMMClass= new PrzyjmijMMClass();
            base.OnAppearing();
            Model.PrzyjmijMMClass.ListOfTwrOnMM.Clear();
            await przyjmijMMClass.GetlistMMElementsAsync(_gidnumer); 
            await PrzypiszListe();
            MyListView.ItemsSource = PrzyjmijMMClass.GetMmkas();
            tytul.Text = PrzyjmijMMClass.GetMmkas().First().GetNrDokMmp;
        }

        private async Task  PrzypiszListe()
        {

            try
            {
                Model.PrzyjmijMMLista.przyjmijMMListas.Clear();
               // await _connection.CreateTableAsync<Model.PrzyjmijMMLista>();
                foreach (var ss in Model.PrzyjmijMMClass.ListOfTwrOnMM)
                {
                    Model.PrzyjmijMMLista przyjmijMMLista = new Model.PrzyjmijMMLista();
                    przyjmijMMLista.twrkod = ss.twrkod;
                    przyjmijMMLista.nazwa = ss.nazwa;
                    przyjmijMMLista.ilosc = ss.ilosc;
                    przyjmijMMLista.nrdokumentuMM = ss.GetNrDokMmp;
                    przyjmijMMLista.GIDdokumentuMM = ss.GIDdokumentuMM;
                    przyjmijMMLista.GIDMagazynuMM = ss.GIDMagazynuMM;
                    przyjmijMMLista.DatadokumentuMM = ss.DatadokumentuMM;
                    przyjmijMMLista.XLGIDMM = ss.XLGIDMM;
                    przyjmijMMLista.Operator = View.LoginLista._user+" "+ View.LoginLista._nazwisko;
                    Model.PrzyjmijMMLista.przyjmijMMListas.Add(przyjmijMMLista);
                 
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(null, ex.Message, "OK");
            }
            //PrzyjmijListaMM = await _connection.Table<Model.PrzyjmijMMClass>().Where(c => c.GIDdokumentuMM == _gidnumer).ToListAsync();

        }



        bool _userTapped;
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (_userTapped)
                return;

            _userTapped = true;
            if (e.Item == null)
                return;
            var mm = e.Item as Model.PrzyjmijMMClass;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
             Navigation.PushModalAsync(new View.RaportLista_AddTwrKod(mm));
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
            _userTapped = false;
        }


        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Model.PrzyjmijMMClass przyjmijMMClass = new Model.PrzyjmijMMClass();
            MyListView.ItemsSource = Model.PrzyjmijMMClass.GetMmkas(e.NewTextValue);
        }

        private async void Btn_StartRaport_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new RaportListaElementow(_gidnumer));  //!!

                var ile = await _connection.Table<Model.RaportListaMM>().Where(c => c.GIDdokumentuMM == _gidnumer).ToListAsync();

                if (ile.Count == 0)
                    await Navigation.PushModalAsync(new RaportLista_AddTwrKod(_gidnumer));
            }
            catch (Exception ex)
            {
                await DisplayAlert(null, ex.Message, "OK");
            }
        }
    }
}
