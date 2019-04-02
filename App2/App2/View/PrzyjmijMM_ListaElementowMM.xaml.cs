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
        public PrzyjmijMM_ListaElementowMM(string EAN_mmka, int gidnumer)
        {
            InitializeComponent();

            Model.PrzyjmijMMClass.ListOfTwrOnMM.Clear();
            Model.PrzyjmijMMClass przyjmijMMClass = new Model.PrzyjmijMMClass();
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            _gidnumer = gidnumer;
            przyjmijMMClass.GetlistMMElements(EAN_mmka, gidnumer);
            tytul.Text = przyjmijMMClass.GetNrDokMmp;// mmp; 
            PrzypiszListe();

            MyListView.ItemsSource = Model.PrzyjmijMMClass.GetMmkas(); 
        }

        private async void PrzypiszListe()
        {

            try
            {
                Model.PrzyjmijMMLista.przyjmijMMListas.Clear();
               // await _connection.CreateTableAsync<Model.PrzyjmijMMLista>();
                foreach (var ss in Model.PrzyjmijMMClass.GetMmkas())
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
                    przyjmijMMLista.Operator = View.LoginLista._nazwisko;
                    Model.PrzyjmijMMLista.przyjmijMMListas.Add(przyjmijMMLista);
                   // await _connection.InsertAsync(przyjmijMMLista);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(null, ex.Message, "OK");
            }
            //PrzyjmijListaMM = await _connection.Table<Model.PrzyjmijMMClass>().Where(c => c.GIDdokumentuMM == _gidnumer).ToListAsync();

        }


        //public PrzyjmijMM_ListaElementowMM(Model.RaportListaMM raportListaMM)
        //{
        //    InitializeComponent();

        //    //Model.PrzyjmijMMClass.ListOfTwrOnMM.Clear();
        //    //Model.PrzyjmijMMClass przyjmijMMClass = new Model.PrzyjmijMMClass();

        //    //przyjmijMMClass.GetlistMMElements(EAN_mmka, gidnumer);
        //    //tytul.Text = przyjmijMMClass.GetNrDokMmp;// mmp; 

        //    MyListView.ItemsSource = Model.RaportListaMM.RaportListaMMs;
        //}


         void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            //var mm = e.Item as Model.PrzyjmijMMClass;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            //await Navigation.PushModalAsync(new ListaMMP(null,mm.GIDdokumentuMM));
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
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
                await Navigation.PushModalAsync(new RaportListaElementow(_gidnumer));  //!!
                // var ile = Model.RaportListaMM.RaportListaMMs.Count;
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
