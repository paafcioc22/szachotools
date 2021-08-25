using App2.Model;
using System;
using System.Collections;
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
    public partial class List_AkcjeElemView : ContentPage
    {
        public IList<AkcjeNagElem> Items2 { get; set; }
        public ObservableCollection<AkcjeNagElem> ListaZFiltrem { get; set; }

        private BindableProperty IsSearchingProperty =
           BindableProperty.Create("IsSearching", typeof(bool), typeof(List_AkcjeElemView), false);
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

        public List_AkcjeElemView( int gidtyp)
        {
            InitializeComponent();
             
            BindingContext = this;
            GetAkcje( gidtyp);

        }

        private async void GetAkcje(int _gidtyp)
        {
            IsSearching = true;

            List_AkcjeView akcjeView = new List_AkcjeView();
            string magnr= akcjeView.GetMagnumer().ToString();

            try
            {
                if (StartPage.CheckInternetConnection())
                {
                    string user = LoginLista._user;
                    int dodajDni = user == "ADM" ? 50 : 0;

                    string Webquery2 = $@"cdn.PC_WykonajSelect N'select distinct AkN_GidNumer, AkN_GidTyp  
                    , AkN_GidNazwa ,AkN_NazwaAkcji, AkN_DataStart,AkN_DataKoniec,Ake_FiltrSQL,IsSendData
                    from cdn.pc_akcjeNag 
                    INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                    join [CDN].[PC_AkcjeTyp] on akn_gidtyp = GidTypAkcji
                     where AkN_GidTyp={_gidtyp} and AkN_DataKoniec>=GETDATE() -10 -{dodajDni}
                        and  (AkN_MagDcl like ''%m={magnr},%'' or AkN_MagDcl=''wszystkie'')
                        and getdate() >= AkN_DataStart
                     order by AkN_DataStart desc
                     '";
                    var AkcjeElemLista = await App.TodoManager.GetGidAkcjeAsync(Webquery2);
                    ListaZFiltrem = AkcjeElemLista;

                    Items2 = AkcjeElemLista.GroupBy(dd => dd.AkN_GidNumer).Select(a => a.First()).ToList();
                    MyListView2.ItemsSource = Items2; 
                }
            }
            catch (Exception x)
            {

                await DisplayAlert(null, x.Message, "OK");
            }

            IsSearching = false;
        }

        bool _istapped;
        async void  Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            if (_istapped)
                return;

            _istapped = true;

                var pozycja = e.Item as Model.AkcjeNagElem;

                var nowa =ListaZFiltrem.Where(x => x.AkN_GidNumer == pozycja.AkN_GidNumer).ToList();

            await    Navigation.PushAsync(new List_AkcjeTwrList(nowa));

            _istapped = false;

            // await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
