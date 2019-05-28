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
         

        public List_AkcjeView()
        {
            InitializeComponent();
            GetAkcje();

            //StworzListe();
        }

        void StworzListe()
        {
            Items_test = new ObservableCollection<Model.Test>();

            Items_test.Add(new Model.Test { gidnumer = 1, mag_dcl = "bol", twrkod = "twr1", ilosc = 2 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr2", ilosc = 42 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr3", ilosc = 51 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr4", ilosc = 8 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr5", ilosc = 9 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr6", ilosc = 11 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr7", ilosc = 1 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr8", ilosc = 2 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr9", ilosc = 44 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr10", ilosc = 4 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr11", ilosc = 6 });
            Items_test.Add(new Model.Test { gidnumer = 2, mag_dcl = "bol", twrkod = "twr12", ilosc = 99 }); 
            

            MyListView.ItemsSource = Items_test;
        }

        private async  void GetAkcje()
        {
            try
            {
                string Webquery = @"cdn.PC_WykonajSelect N'select distinct AkN_GidTyp  ,AkN_GidNazwa +'' (''+cast( count(distinct Ake_AknNumer)as varchar)+'')'' AkN_GidNazwa  
                    from cdn.pc_akcjeNag INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                    
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

         async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var pozycja = e.Item as Model.AkcjeNagElem;
            //var pozycja = e.Item as Model.Test;
            //pozycja.ilosc += 1;
            await Navigation.PushModalAsync(new View.List_AkcjeElemView(pozycja.AkN_GidTyp));




            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //var nowa = Items_test.Where(z => z.twrkod == "twr2").ToList();
            //nowa[0].ilosc += 1;
            var nowa = Items_test.Where(n => n.twrkod == "twr2").Select(n => { n.ilosc+=1; return n; }).ToList();
            MyListView.ItemsSource = nowa;
        }
    }
}
