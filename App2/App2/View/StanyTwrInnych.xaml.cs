using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StanyTwrInnych : ContentPage
    {

         
        public  StanyTwrInnych(string TwrGidnumer)
        {
            InitializeComponent();
            PobierzDaneDoListy(TwrGidnumer);  
        }

        private async void PobierzDaneDoListy(string twrGidnumer)
        {
            string query = "CDN.StanyTowaruWOddzialach_int " + twrGidnumer;
            var Maglista = await App.TodoManager.GetTodoItemsAsync(query);

            MyListView.ItemsSource = Maglista;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }


    }
}

 