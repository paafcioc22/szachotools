using App2.Model;
using App2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OddzialyListView : ContentPage
    {
        private readonly NagElem pozycja;
        OddzialyViewModel viewModel;
        private bool _istapped;

        public OddzialyListView(Model.NagElem pozycja)
        {
            InitializeComponent();

            BindingContext = viewModel = new OddzialyViewModel(new PhotoViewModel(pozycja));
            this.pozycja = pozycja;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.ExecuteAsync();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (_istapped)
                return;

            _istapped = true;

            var sklep = e.Item as FotoOddzial;

            //todo: podmień  strone
            //await Navigation.PushAsync(new Foto2(pozycja,sklep.mag_kod,false ));
            await Navigation.PushAsync(new FotoTest(pozycja,sklep.mag_kod,false ));


            _istapped = false;

            ((ListView)sender).SelectedItem = null;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            viewModel.FilterItems();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {             
            switchTogg.IsToggled = !switchTogg.IsToggled;
        }

        private void SwitchTogg_Toggled(object sender, ToggledEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}