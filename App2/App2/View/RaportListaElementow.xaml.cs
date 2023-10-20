
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaportListaElementow : ContentPage
    {
        //public ObservableCollection<striPrzyjmijMMClass> Items { get; set; }
        SQLiteAsyncConnection _connection;
        private int _gidnumer;
        public RaportListaElementow(int gidnumer)
        {
            InitializeComponent();
             _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            _gidnumer = gidnumer;
			//MyListView.ItemsSource = Model.RaportListaMM.RaportListaMMs;  

        }
        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Model.RaportListaMM>();
            await _connection.CreateTableAsync<Model.PrzyjmijMMLista>();

            var listaraport = await _connection.Table<Model.RaportListaMM>().Where(c => c.GIDdokumentuMM == _gidnumer).ToListAsync();
            MyListView.ItemsSource = listaraport;// Model.RaportListaMM.RaportListaMMs;

              

            base.OnAppearing();
        }

        public RaportListaElementow(Model.RaportListaMM raportListaMM)
        {
            InitializeComponent();
             

            MyListView.ItemsSource = Model.RaportListaMM.RaportListaMMs;
        }

         private async  void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
           


            try
            {
                if (e.Item == null)
                    return;
                var mmka = e.Item as Model.RaportListaMM;

                string odpowiedz = await DisplayPromptAsync("Podaj Nową Wartość:", null, "Zatwierdź", "Anuluj", "Ilość",-1, Keyboard.Numeric,"");

                  
                if (!String.IsNullOrEmpty(odpowiedz))
                {
                    mmka.ilosc_OK = Convert.ToInt16(odpowiedz);
                    await _connection.UpdateAsync(mmka);
                }


            }
            catch (Exception ex)
            {

               await  DisplayAlert(null, ex.Message, "OK");
            }

            //MyListView.ItemsSource = Model.PrzyjmijMMClass.GetMmkas();
            //MyListView.EndRefresh();

            ((ListView)sender).SelectedItem = null;
        }

        private async void Btn_AddTwr_Clicked(object sender, EventArgs e)
        {
              
            await Navigation.PushModalAsync(new RaportLista_AddTwrKod(_gidnumer));  

        }
          

        private void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            _connection.Table<Model.RaportListaMM>().DeleteAsync();
            
            _connection.ExecuteAsync("drop table RaportListaMM");
        }

        private async void Btn_CreateRaport_Clicked(object sender, EventArgs e)
        {
            try
            {

                Model.ListDiffrence.listDiffrences.Clear();
                var listaZMM = Model.PrzyjmijMMLista.przyjmijMMListas.Where(n => n.GIDdokumentuMM == _gidnumer);

                await Navigation.PushAsync(new View.RaportListaRoznice(listaZMM.ToList()[0].GIDdokumentuMM)); //!!!

                //Model.PrzyjmijMMLista.przyjmijMMListas.Clear();
                Model.ListDiffrence.listDiffrences.Clear();

            }
            catch (Exception ex)
            {

                await DisplayAlert(null, ex.Message, "ok");
            }
        }
    }
}
