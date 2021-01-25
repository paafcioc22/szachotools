using App2.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.TworzPaczki
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePaczkaListaPaczek : ContentPage
    {
        public ObservableCollection<FedexPaczka> Items { get; set; }
        private int fmm_gidnumer;
        public CreatePaczkaListaPaczek(int fmm_gidnumer)
        {
            InitializeComponent();
            BindingContext = this;
            this.fmm_gidnumer = fmm_gidnumer;

            
        }

        private async void GetListaPaczek(int fmm_gidnumer)
        {
           var query  = $@"  cdn.PC_WykonajSelect '
              select Fmm_GidNumer,Fmm_EleNumer,max(Fmm_NrListu)Fmm_NrListu, max(Fmm_NrPaczki)Fmm_NrPaczki,max(Fmm_Elmenty)Fmm_Elmenty, max(Fmm_DataZlecenia)Fmm_DataZlecenia, max(Fmm_MagDcl)Fmm_MagDcl ,max(Fmm_MagZrd)Fmm_MagZrd
                    from cdn.PC_FedexMM
                    where fmm_gidnumer={fmm_gidnumer} and Fmm_EleNumer<>0 
                    group by Fmm_EleNumer,Fmm_GidNumer'
                    ";//and trn_Stan = 1

            var lista = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);
            Items = lista;
            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            var pozycja = e.Item as FedexPaczka;

           // await DisplayAlert("Item Tapped", $"nr zl {pozycja.Fmm_GidNumer}, nr karon{pozycja.Fmm_EleNumer}", "OK");
            await Navigation.PushAsync(new CreatePaczkaListaMM(pozycja.Fmm_GidNumer, pozycja.Fmm_EleNumer));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            GetListaPaczek(this.fmm_gidnumer);            
            
            
            base.OnAppearing();
        }

        static int i ;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            i++;

            CreatePaczkaReposytorySQL reposytorySQL = new CreatePaczkaReposytorySQL();

            await reposytorySQL.SavePaczkaToBase(new FedexPaczka(), this.fmm_gidnumer, 0);

            GetListaPaczek(this.fmm_gidnumer);
             

        }
    }
}
