using App2.Model;
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
    public partial class ScanningResultListPage : ContentPage
    {
        PMM_SkanListViewModel viewModel;

        public ScanningResultListPage(Model.PMM_DokNaglowek dokument)
        {
            InitializeComponent();
            BindingContext = viewModel= new PMM_SkanListViewModel(dokument);
      
        } 

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadSkanElementsCommand.Execute(this);
        }

        
    }
}
