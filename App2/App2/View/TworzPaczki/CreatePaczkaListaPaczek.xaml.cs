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
        public ListaZlecenView Pozycja { get; }

        private int fmm_gidnumer;
        CreatePaczkaReposytorySQL reposytorySQL;

        //private string fmm_MagDcl;
        //public CreatePaczkaListaPaczek(int fmm_gidnumer, string fmm_MagDcl)
        //{
        //    InitializeComponent();
        //    BindingContext = this;
        //    this.fmm_gidnumer = fmm_gidnumer;
        //    this.fmm_MagDcl = fmm_MagDcl;


        //    btn_dodajpaczke.IsEnabled = true;


        //}

        public CreatePaczkaListaPaczek(ListaZlecenView pozycja)
        {
            InitializeComponent();
            BindingContext = this;
            reposytorySQL = new CreatePaczkaReposytorySQL();

            this.fmm_gidnumer = pozycja.Fmm_gidnumer;
            //this.fmm_MagDcl = pozycja.Fmm_MagDcl;
            Pozycja = pozycja;

            if(!string.IsNullOrEmpty(pozycja.Fmm_nrlistu))
            btn_dodajpaczke.IsEnabled = false;

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
            await Navigation.PushAsync(new CreatePaczkaListaMM(pozycja));

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

            //CreatePaczkaReposytorySQL reposytorySQL = new CreatePaczkaReposytorySQL();

            await reposytorySQL.SavePaczkaToBase(new FedexPaczka(), this.fmm_gidnumer, 0);

            GetListaPaczek(this.fmm_gidnumer);
             

        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            var item = theViewCell.BindingContext as FedexPaczka;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
                if (string.IsNullOrEmpty(item.Fmm_NrListu ))
                {
                    var moreAction = new MenuItem { Text = "Usuń karton" };
                    moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    moreAction.Clicked += MenuItem_Clicked;
                    theViewCell.ContextActions.Add(moreAction);
                }
            }
        }

        private  async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert(null, "Czy chcesz usunąć karton z listy?", "Tak", "Nie");
            if (action)
            {
                var usunKarton = (sender as MenuItem).CommandParameter as Model.FedexPaczka;

                await reposytorySQL.RemovePaczka(usunKarton);
                GetListaPaczek(this.fmm_gidnumer);
            }
        }
    }
}
