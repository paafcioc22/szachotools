using App2.Model.ApiModel;
using App2.OptimaAPI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddElementMMList : ContentPage
    {
        //public ObservableCollection<string> Items { get; set; }
       
        public Int32 gidnumer;
        public Int32 Id;
        private bool bufor;
        ServiceDokumentyApi apiService;
        public AddElementMMList(DokNaglowekDto mmka)
        {
            InitializeComponent();
            apiService= new ServiceDokumentyApi();  
            _magDcl.Text = mmka.MagKod + (mmka.NumerDokumentu == "" ? "" : " - " + mmka.NumerDokumentu);
            _magDcl.IsEnabled = (mmka.NumerDokumentu == "" ? true : false);
            _magDcl.TextColor = (mmka.NumerDokumentu == "" ? Color.Bisque : Color.LightCyan);
            //_magDcl.InputTransparent= (mmka.nrdok == "" ? true : false);
            _magDcl.WidthRequest = (mmka.NumerDokumentu == "" ? 70 : 300);
            _opis.Text = mmka.Opis  ;
            _opis.IsEnabled = (mmka.NumerDokumentu == "" ? true : false);
            _opis.TextColor= (mmka.NumerDokumentu == "" ? Color.Bisque : Color.LightCyan);
            _opis.WidthRequest = 300;
            _btnAddElement.IsEnabled = ((bool)!mmka.IsFinish ? true : false);
            _btnSave.IsEnabled = ((bool)!mmka.IsFinish ? true : false);
            //lbl_listatwr.Text=()
            gidnumer = mmka.GidNumerXl;
            Id = mmka.Id;
            bufor = !Convert.ToBoolean(mmka.IsFinish);
            ListaElementowMM.ItemsSource = ServiceDokumentyApi.DokElementsDtos;
       
          
        } 
         

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            if (bufor)
            { 
                var odp = await  DisplayActionSheet(null, "Anuluj", "Usuń", "Edytuj","Pokaż zdjęcie");
                var kod = e.Item as DokElementDto;
                switch (odp)
                {
                case "Usuń":
                        var action = await DisplayAlert(null, "Czy chcesz usunąć "+kod.TwrKod+" z listy?", "Tak", "Nie");
                        if (action)
                        {
                            var usunMM =e.Item as DokElementDto;

                            ServiceDokumentyApi.DokElementsDtos.Remove(usunMM);
                            await apiService.DeleteElement(usunMM);
                        }
                        break;
                case "Edytuj":

                        //var mmka = e.Item as Model.DokMM; 
                        //((ListView)sender).SelectedItem = null;
                        //Model.DokMM dokMM = new Model.DokMM();
                        //dokMM.getElementy(mmka.gidnumer);
                        //await Navigation.PushModalAsync(new View.AddTwrPage(mmka));

                        var mmka = e.Item as DokElementDto;
                        ((ListView)sender).SelectedItem = null;
                        await Navigation.PushModalAsync(new View.AddTwrPage(mmka));

                        break;
                    case "Pokaż zdjęcie":
                      
                        var mmka2 = e.Item as DokElementDto;
                        ((ListView)sender).SelectedItem = null;
              
                        //todo :po co to jest?????
                        //dokMM2.getElementy(mmka2.gidnumer);
                        await Navigation.PushModalAsync(new View.AddTwrPage(mmka2,"foto"));
                        break;
                }
            }



            ((ListView)sender).SelectedItem = null;
        }

        private   void _magDcl_Focused(object sender, FocusEventArgs e)
        {
            //await Navigation.PushModalAsync(new ListaSklepowPage("edytuj"));

            var page = new ListaSklepowPage("edytuj");
            page.ListViewSklepy.ItemSelected += (source, args) =>
            {
                var sklep = args.SelectedItem as Model.ListaSklepow;
                
                _magDcl.Text = sklep.mag_kod;
                Navigation.PopModalAsync();
            };

              Navigation.PushModalAsync(page);
        }

        //private async void Delete_Clicked(object sender, EventArgs e)
        //{
        //    var action = await DisplayAlert(null, "Czy chcesz usunąć towar z listy?", "Tak", "Nie");
        //    if (action)
        //    {
        //        var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
                 
        //        Model.DokMM.dokElementy.Remove(usunMM);
        //        Model.DokMM.DeleteElementMM(usunMM);
        //    }
        //}

        private async void BtnAddPosition_Clicked(object sender, EventArgs e)
        {
           
            await Navigation.PushModalAsync(new View.AddTwrPage(gidnumer));
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
         
            DokNaglowekDto dokmm = new DokNaglowekDto()
            {
                GidNumerXl= gidnumer,
                MagKod= _magDcl.Text,
                Opis= _opis.Text,
            };

         
            var odp = await apiService.UpdateDokMm(Id, dokmm);

            await apiService.GetDokAll(GidTyp.Mm,false); 
 
            await Navigation.PopModalAsync();

            StartCreateMmPage startCreateMmPage = new StartCreateMmPage();
        
             

        }

        protected override bool OnBackButtonPressed()
        {
           
            Navigation.PopModalAsync();
            StartCreateMmPage startCreateMmPage = new StartCreateMmPage();
       
         
            return true;
        }
    }
}
