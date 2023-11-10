using App2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.PrzyjmijMM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PMM_NaglowekAndMenuViewDetail : ContentPage
    {
        
        public PMM_NaglowekAndMenuViewDetail()
        {
            InitializeComponent();
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
            var viewModel = BindingContext as PMM_NaglowekViewModel;
            if (viewModel != null && viewModel.LoadMMNaglowkiCommand.CanExecute(null))
            {
                var commandParams = new { viewModel.CzyZatwierdzone, viewModel.PastDays };
                //await viewModel.ExecuteLoadMMCommand(viewModel.CzyZatwierdzone); // Zmień `false` na odpowiednią wartość logiczną.
                viewModel.LoadMMNaglowkiCommand.Execute(commandParams); ; // Zmień `false` na odpowiednią wartość logiczną.
            }
        }

      
 
    }
}