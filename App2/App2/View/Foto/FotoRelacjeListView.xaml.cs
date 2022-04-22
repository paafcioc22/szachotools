using App2.Model;
using App2.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FotoRelacjeListView : ContentPage
    {


        //public ObservableCollection<NagElem> ListaZFiltrem { get; set; }
        public IList<NagElem> Items2 { get; set; }
      

        //private BindableProperty IsSearchingProperty =
        //   BindableProperty.Create("IsSearching", typeof(bool), typeof(FotoRelacjeListView), false);
        //public bool IsSearching
        //{
        //    get { return (bool)GetValue(IsSearchingProperty); }
        //    set { SetValue(IsSearchingProperty, value); }
        //}

      
        private bool _istapped;

        FotoRelacjeViewModel viewModel;
        public FotoRelacjeListView()
        {
            InitializeComponent();
            BindingContext = viewModel = new FotoRelacjeViewModel();
            //ListaZFiltrem = new ObservableCollection<NagElem>();
             
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            if (_istapped)
                return;

            _istapped = true;

 
            var pozycja = e.Item as Model.NagElem; 
             
           await Navigation.PushAsync(new OddzialyListView(pozycja));
            

            _istapped = false;

      
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            //GetAkcje();

            base.OnAppearing();

            if (viewModel.ListaZFiltrem.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        //private async void GetAkcje()
        //{
        //    IsSearching = true;
          

        //    try
        //    {
        //        if (StartPage.CheckInternetConnection())
        //        {
        //            string user = LoginLista._user;
        //            int dodajDni = user == "ADM" ? 50 : 0;

        //            string Webquery2 = $@"cdn.PC_WykonajSelect N'select distinct AkN_GidNumer, AkN_GidTyp  
        //                    , AkN_GidNazwa ,AkN_NazwaAkcji, AkN_DataStart,AkN_DataKoniec,Ake_FiltrSQL,IsSendData
        //                    from cdn.pc_akcjeNag 
        //                    INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
        //                    join [CDN].[PC_AkcjeTyp] on akn_gidtyp = GidTypAkcji
        //                    where AkN_GidTyp=16 and AkN_DataKoniec>=GETDATE() -30 -{dodajDni} 
        //                    and getdate() >= AkN_DataStart
        //                    order by AkN_DataStart desc
        //                 '";


        //            var AkcjeElemLista = await App.TodoManager.PobierzDaneZWeb<NagElem>(Webquery2);
        //            Items2 = AkcjeElemLista;

        //            //ListaZFiltrem = AkcjeElemLista;
        //            //ListaView.ItemsSource = AkcjeElemLista;

        //        }
        //    }
        //    catch (Exception x)
        //    {
        //        await DisplayAlert(null, x.Message, "OK");
        //    }

        //    IsSearching = false;
        //}

    }
}