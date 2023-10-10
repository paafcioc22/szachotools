using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartCreateMmPage : ContentPage
	{

        ServiceDokumentyApi dokumentyApi;
        public StartCreateMmPage ()
		{
			InitializeComponent ();

            dokumentyApi = new ServiceDokumentyApi();

            BindingContext = dokumentyApi; //DokMMViewModel.dokMMs; 

            

            //ListaMMek.ItemsSource = ServiceDokumentyApi.DokNaglowekDtos;// DokMMViewModel.dokMMs;           
           
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            dokumentyApi.DokNaglowekDtos.Clear ();
            await dokumentyApi.GetDokAll(GidTyp.Mm, false);

        }


        private async void Btn_AddMm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new View.AddMMPage());

        
            //ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
  
            ListaMMek.ItemsSource = null;
            ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
        }

        private async void ListaMMek_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (await SettingsPage.SprConn())
            {
                var mmka = e.Item as DokNaglowekDto;
            
                ((ListView)sender).SelectedItem = null;

                //await dokumentyApi.GetDokWithElementsById(mmka.Id);
                await Navigation.PushModalAsync(new View.AddElementMMList(mmka));
    
            }
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert(null, "Czy chcesz usunąć MM z listy?", "Tak", "Nie");
            if (action)
            {
                var usunMM = (sender as MenuItem).CommandParameter as DokNaglowekDto;
                dokumentyApi.DokNaglowekDtos.Remove(usunMM);
                await dokumentyApi.DeleteDokument(usunMM.Id);
            }
        } 

         
        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
            ViewCell cell = (sender as CheckBox).Parent.Parent as ViewCell;

            var model = cell.BindingContext as DokNaglowekDto;

            //DokNaglowekDto dokmm = new DokNaglowekDto()
            //{
            //    GidNumerXl = gidnumer,
            //    MagKod = _magDcl.Text,
            //    Opis = _opis.Text,
            //};

            //model.

            //todo : sprawdz czy export dziala
            var odp = await dokumentyApi.UpdateDokMm(model.Id, model);

            //Model.DokMM dokMM = new Model.DokMM();
            //dokMM.gidnumer = model.gidnumer;
            //dokMM.mag_dcl = model.mag_dcl;
            //dokMM.opis = model.opis;
            //dokMM.IsExport = model.IsExport;
            //dokMM.fl_header = 1;
            //dokMM.UpdateMM(dokMM); 

        }
    }
}