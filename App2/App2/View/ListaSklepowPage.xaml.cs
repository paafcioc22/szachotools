using App2.OptimaAPI;
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
    public partial class ListaSklepowPage : ContentPage
    {
        private string opcja;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Upewnij się, że listaSklepows jest pusta przed ładowaniem danych
            listaSklepows.Clear();
            await Getsklepy();
            szukaj.Focus();
        }


        public static ObservableCollection<Model.ListaSklepow> listaSklepows;

        public ListaSklepowPage(string param)
        {
            InitializeComponent();
            listaSklepows = new ObservableCollection<Model.ListaSklepow>();
            opcja = param;
       
            MyListView.ItemsSource = listaSklepows;
        }

        private async Task Getsklepy()
        {
            try
            {
                ServiceDokumentyApi api = new ServiceDokumentyApi();
                var sklepy = await api.GetSklepyInfo();

                foreach (var item in sklepy)
                {
                    listaSklepows.Add(new Model.ListaSklepow
                    {
                        mag_kod = item.MagKod,
                        mag_nazwa = item.Magazyn,
                        mag_gidnumer = item.Id.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Uwaga", ex.Message, "OK");
            }
        }


        public static IEnumerable<Model.ListaSklepow> Getsklep(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return listaSklepows;
            return listaSklepows.Where(c => c.mag_kod.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));
        }

        public ListView ListViewSklepy { get { return MyListView; } } 
 
        private void szukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyListView.ItemsSource = Getsklep(e.NewTextValue);
        }
    }
}
