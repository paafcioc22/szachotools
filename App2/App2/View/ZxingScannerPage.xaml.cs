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
        }
    }
}