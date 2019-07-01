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
    public partial class List_AkcjeAfterFiltr : ContentPage
    {
        public ObservableCollection<Model.AkcjeNagElem> Items { get; set; }

        public List_AkcjeAfterFiltr(IEnumerable<Model.AkcjeNagElem> nowa)
        {
            Items = new ObservableCollection<Model.AkcjeNagElem>();
          
            InitializeComponent();
            BindingContext = this;

            Items = Convert(nowa);

            MyListView.ItemsSource = Items;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = this;

            MyListView.ItemsSource = Items.OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList(); ;
        }
        public ObservableCollection<T> Convert<T>(IEnumerable<T> original)
        {
            return new ObservableCollection<T>(original);
        }


        private bool _istapped;
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (_istapped)
                return;

            _istapped = true;

            var pozycja = e.Item as Model.AkcjeNagElem;

            await Navigation.PushModalAsync(new List_ScanPage(pozycja));

            _istapped = false;

            ((ListView)sender).SelectedItem = null;
        }
    }
}
