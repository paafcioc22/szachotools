using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : PopupPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (passwordEntry.Text == "haslo")
            {
                await Navigation.PushAsync(new View.Foto.FotoRelacjeListView());
                await PopupNavigation.Instance.PopAllAsync();
            }
            else
                await DisplayAlert("uwaga", "podaj poprawne hasło", "OK");
        }
    }
}