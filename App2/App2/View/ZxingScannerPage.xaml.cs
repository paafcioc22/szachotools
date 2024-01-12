using App2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZxingScannerPage : ContentPage
    {

        public ZxingScannerPage()
        {
            InitializeComponent();
        }

        private void OnTorchToggled(object sender, ToggledEventArgs e)
        {
            scannerView.ToggleTorch();
            //scannerView.IsTorchOn = e.Value;
        }

        //protected override async void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    await Task.Delay(100);
        //    if (BindingContext is ZXingViewModel viewModel)
        //    {
        //        viewModel.StopScanning();
        //    }
        //}
    }
}