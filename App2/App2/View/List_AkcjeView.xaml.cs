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
    public partial class List_AkcjeView : ContentPage
    {
        public ObservableCollection<Model.AkcjeNagElem> Items { get; set; }
        public ObservableCollection<Model.Test> Items_test { get; set; }
        public static string TypAkcji;

        public List_AkcjeView()
        {
            InitializeComponent();
            GetAkcje();

            //StworzListe();
        }

         

        private async  void GetAkcje()
        {
            try
            {
                string Webquery = @"cdn.PC_WykonajSelect N'select distinct AkN_GidTyp  ,AkN_GidNazwa +'' (''+cast( count(distinct Ake_AknNumer)as varchar)+'')'' AkN_GidNazwa  
                    from cdn.pc_akcjeNag INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                    where AkN_DataKoniec>=GETDATE() -10 
                    group by AkN_GidTyp  , AkN_GidNazwa '";
                var twrdane = await App.TodoManager.GetGidAkcjeAsync(Webquery);

                Items = twrdane;
                MyListView.ItemsSource = twrdane;
            }
            catch (Exception x)
            {

                await DisplayAlert(null, x.Message, "OK");
            }
        }


        bool _istapped;
         async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            if (_istapped)
                return;

            _istapped = true;

            var pozycja = e.Item as Model.AkcjeNagElem;
       
                await Navigation.PushModalAsync(new View.List_AkcjeElemView(pozycja.AkN_GidTyp));

                TypAkcji = pozycja.AkN_GidNazwa;

            _istapped = false;


            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
           
            var nowa = Items_test.Where(n => n.twrkod == "twr2").Select(n => { n.ilosc+=1; return n; }).ToList();
            MyListView.ItemsSource = nowa;
        }
    }
}
