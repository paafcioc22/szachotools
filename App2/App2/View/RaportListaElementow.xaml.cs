using Acr.UserDialogs;
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


                PromptResult prr = await UserDialogs.Instance.PromptAsync
                    (
                        new PromptConfig
                        {
                            InputType = InputType.Number,
                            Message = "Podaj Ilośc:  ",
                            OkText = "Zatwierdź",
                            CancelText = "Anuluj",
                            
                           // Title = "Pytanie..",
                           // IsCancellable = true,
                        }
                    );
                //await DisplayAlert("info", prr.Text, "ok");
                if (prr.Text != "")
                {
                    mmka.ilosc_OK = Convert.ToInt16(prr.Text);
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

                //foreach (var ww in listaZMM)
                //{
                //    Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence {
                //        twrkod = ww.twrkod,
                //        ilosc = ww.ilosc * -1,
                //        typ = "MM",
                //        IleZMM =ww.ilosc,
                //        NrDokumentu = ww.nrdokumentuMM,
                //        GidMagazynu = ww.GIDMagazynuMM,
                //        DataDokumentu= ww.DatadokumentuMM,
                //        TwrNazwa = ww.nazwa
                //    }); 
                //}


                //var listaZskanowania = await _connection.QueryAsync<Model.RaportListaMM>(@"select RaportListaMM.twrkod, RaportListaMM.nazwa,
                //RaportListaMM.ilosc_OK , RaportListaMM.nrdokumentuMM, RaportListaMM.DatadokumentuMM
                //from RaportListaMM 
                //where RaportListaMM.GIDdokumentuMM = ? ", _gidnumer);
                ////odczy
                //foreach (var ww in listaZskanowania)
                //{
                //    Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence {
                //        twrkod = ww.twrkod,
                //        ilosc = ww.ilosc_OK,
                //        typ = "Skan",
                //        IleZeSkan =ww.ilosc_OK,
                //        NrDokumentu = ww.nrdokumentuMM,
                //        DataDokumentu = ww.DatadokumentuMM,
                //        GidMagazynu = ww.GIDMagazynuMM,
                //        TwrNazwa= ww.nazwa
                //    });
                //}

                //var lista = Model.ListDiffrence.listDiffrences; 

                //var nowa2 = lista.GroupBy(g => g.twrkod).
                //    SelectMany(c => c.Select(
                //        csline => new Model.ListDiffrence
                //        {
                //            twrkod = csline.twrkod,
                //            IleZeSkan= c.Sum(x=>x.IleZeSkan),
                //            IleZMM = csline.IleZMM,
                //            ilosc = c.Sum(cc => cc.ilosc),
                //            NrDokumentu= csline.NrDokumentu,
                //            GidMagazynu = csline.GidMagazynu,
                //            DataDokumentu = csline.DataDokumentu,
                //            TwrNazwa=csline.TwrNazwa
                //        })).ToList().GroupBy(p => new
                //        {
                //            p.twrkod,
                //            p.ilosc // modyfikacja
                //            //p.IleZMM,
                //            //p.IleZeSkan
                //        }).Select(g => g.First()).Where(f =>f.ilosc!=0).OrderBy(x => x.twrkod);



                // await Navigation.PushModalAsync(new View.RaportListaRoznice(nowa2, listaZMM));

                await Navigation.PushModalAsync(new View.RaportListaRoznice(listaZMM.ToList()[0].GIDdokumentuMM)); //!!!

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
