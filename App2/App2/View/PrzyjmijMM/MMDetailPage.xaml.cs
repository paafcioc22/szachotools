using App2.Model;
using App2.Model.ApiModel;
using App2.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.PrzyjmijMM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MMDetailPage : ContentPage
    {
        private PMM_DokNaglowek item;
        PMM_DetailsViewModel viewModel;
        public MMDetailPage(PMM_DokNaglowek item)
        {
            InitializeComponent();
            this.item = item; 
            BindingContext = viewModel = new PMM_DetailsViewModel(item);
        }

        private void Initialize()
        {

            if (Parent is NavigationPage navigationPage)
            {
                navigationPage.BarBackgroundColor = Color.DarkCyan; // Ustaw kolor tła
                navigationPage.BarTextColor = Color.White; // Ustaw kolor tekstu
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Initialize();
            viewModel.LoadMmElementsCommand.Execute(item.Trn_Trnid);
        }

    }
}
