using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.View.CreateMM;
using App2.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartCreateMmPage : ContentPage
	{
        private Timer _timer;

        ServiceDokumentyApi dokumentyApi;
        public StartCreateMmPage ()
		{
			InitializeComponent ();

            dokumentyApi = new ServiceDokumentyApi();

            BindingContext = dokumentyApi;  
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timer = new Timer(UpdateList, null, 0, 7000);

        }

        private void UpdateList(object state)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Logika aktualizacji listy, na przykład:
                //await LoadMMExec(FinishedToo);
            await dokumentyApi.LoadMMExec(dokumentyApi.FinishedToo);
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
            }

            // Inne operacje, które muszą zostać wykonane, gdy strona znika
        }

        private async void Btn_AddMm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddMMPage()); 
        }

        //private void Button_Clicked_3(object sender, EventArgs e)
        //{
  
        //    ListaMMek.ItemsSource = null;
        //    ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
        //}

        private async void ListaMMek_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (await SettingsPage.SprConn())
                {
                    var mmka = e.Item as DokNaglowekDto;

                    ((ListView)sender).SelectedItem = null;

                    //await dokumentyApi.GetDokWithElementsById(mmka.Id);
                    await Navigation.PushModalAsync(new View.AddElementMMList(mmka));

                }
            }
            catch (Exception s)
            {

                await DisplayAlert("Błąd",s.Message,"OK");
            }
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var usunMM = (sender as MenuItem).CommandParameter as DokNaglowekDto;
            
            var action = await DisplayAlert(null, $"Czy chcesz usunąć MM (Id : {usunMM.Id}) z listy?", "Tak", "Nie");
            if (action)
            {
                dokumentyApi.DokNaglowekDtos.Remove(usunMM);
                await dokumentyApi.DeleteDokument(usunMM.Id);
            }
        } 

         
        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
            ViewCell cell = (sender as CheckBox).Parent.Parent as ViewCell;

            var model = cell.BindingContext as DokNaglowekDto;

            try
            {
                var odp = await dokumentyApi.UpdateDokMm(model.Id, model);
            }
            catch (Exception s)
            {
                await DisplayAlert("Błąd:", s.Message, "OK");
            }   

        }

        
    }
}