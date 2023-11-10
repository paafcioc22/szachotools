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
    public partial class PMM_NaglowekAndMenuView : FlyoutPage
    {
        readonly PMM_NaglowekViewModel viewModel;
        public event EventHandler FlyoutClosed;
        public PMM_NaglowekAndMenuView()
        {
            InitializeComponent();
            BindingContext = viewModel = new PMM_NaglowekViewModel();

            FlyoutPage.BindingContext = viewModel;
            ((PMM_NaglowekAndMenuViewDetail)((NavigationPage)Detail).CurrentPage).BindingContext = viewModel;

            this.PropertyChanged += (sender, e) => {
                if (e.PropertyName == "IsPresented" && !this.IsPresented)
                {
                    // Menu zostało zamknięte, wywołaj metodę w ViewModelu
                    var commandParams = new { viewModel.CzyZatwierdzone, viewModel.PastDays };

                    // Wywołanie komendy z parametrami
                    if (viewModel.LoadMMNaglowkiCommand.CanExecute(commandParams))
                    {
                        viewModel.LoadMMNaglowkiCommand.Execute(commandParams);
                    }
                }
            };

        }

        

    }
}