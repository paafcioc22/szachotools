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
        private bool bufor;
        public AddElementMMList(Model.DokMM mmka)
        {
            InitializeComponent();
            
            _magDcl.Text = mmka.mag_dcl + (mmka.nrdok == "" ? "" : " - " + mmka.nrdok);
            _magDcl.IsEnabled = (mmka.nrdok == "" ? true : false);
            _magDcl.TextColor = (mmka.nrdok == "" ? Color.Bisque : Color.LightCyan);
            //_magDcl.InputTransparent= (mmka.nrdok == "" ? true : false);
            _magDcl.WidthRequest = (mmka.nrdok == "" ? 70 : 300);
            _opis.Text = mmka.opis  ;
            _opis.IsEnabled = (mmka.nrdok == "" ? true : false);
            _opis.TextColor= (mmka.nrdok == "" ? Color.Bisque : Color.LightCyan);
            _opis.WidthRequest = 300;
            _btnAddElement.IsEnabled = (mmka.statuss == 0 ? true : false);
            _btnSave.IsEnabled = (mmka.statuss == 0 ? true : false);
            //lbl_listatwr.Text=()
            gidnumer = mmka.gidnumer;
            bufor = !Convert.ToBoolean(mmka.statuss);
            ListaElementowMM.ItemsSource = Model.DokMM.dokElementy;
            flaga=1;
            StartTimer(true);
            StartCreateMmPage.flagaMM = 0;
        }
        private int flaga;
            int _gidnumer;


        //public Item SelectedItem
        //{
        //    get
        //    {
        //        return _selectedItem;
        //    }
        //    set
        //    {
        //        _selectedItem = value;

        //        if (_selectedItem == null)
        //            return;

        //        SomeCommand.Execute(_selectedItem);

        //        SelectedItem = null;
        //    }
        //}




        public void StartTimer(bool status)
        {
            Device.StartTimer(System.TimeSpan.FromSeconds(7), () =>
            {
                Device.BeginInvokeOnMainThread(UpdateUserDataAsync);
                if (!status||flaga == 0)
                {
                    return false;
                }
                else {
                    _gidnumer = gidnumer;
                    return true;
                }
            });
        }
         

        private void UpdateUserDataAsync()
        {

            if (_gidnumer == gidnumer)
            {
                Model.DokMM dokMM = new Model.DokMM();
                flaga += 1;
                 
                ListaElementowMM.ItemsSource = dokMM.getElementy(gidnumer); 
            }
        }


        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            if (bufor)
            { 
                var odp = await  DisplayActionSheet(null, "Anuluj", "Usuń", "Edytuj","Pokaż zdjęcie");
                var kod = e.Item as Model.DokMM;
                switch (odp)
                {
                case "Usuń":
                        var action = await DisplayAlert(null, "Czy chcesz usunąć "+kod.twrkod+" z listy?", "Tak", "Nie");
                        if (action)
                        {
                            var usunMM =e.Item as Model.DokMM;
                          //  var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
                            flaga += 1;
                            if (flaga >= 1) { StartTimer(false); } 
                            Model.DokMM.dokElementy.Remove(usunMM);
                            Model.DokMM.DeleteElementMM(usunMM);
                        }
                        break;
                case "Edytuj":
                        // await DisplayAlert(null, odp, "OK");
                        flaga += 1;
                        if (flaga > 1) { StartTimer(false); }
                        var mmka = e.Item as Model.DokMM; 
                        ((ListView)sender).SelectedItem = null;
                        Model.DokMM dokMM = new Model.DokMM();
                        dokMM.getElementy(mmka.gidnumer);
                        await Navigation.PushModalAsync(new View.AddTwrPage(mmka));
                        break;
                    case "Pokaż zdjęcie":
                        flaga += 1;
                        if (flaga > 1) { StartTimer(false); }
                        var mmka2 = e.Item as Model.DokMM;
                        ((ListView)sender).SelectedItem = null;
                        Model.DokMM dokMM2 = new Model.DokMM();
                        dokMM2.getElementy(mmka2.gidnumer);
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

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert(null, "Czy chcesz usunąć towar z listy?", "Tak", "Nie");
            if (action)
            {
                var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
                 
                Model.DokMM.dokElementy.Remove(usunMM);
                Model.DokMM.DeleteElementMM(usunMM);
            }
        }

        private async void BtnAddPosition_Clicked(object sender, EventArgs e)
        {
            flaga += 1;
            if (flaga >= 1) { StartTimer(false); }
            await Navigation.PushModalAsync(new View.AddTwrPage(gidnumer));
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            Model.DokMM dokMM = new Model.DokMM();
            dokMM.gidnumer = gidnumer;
            dokMM.mag_dcl = _magDcl.Text;
            dokMM.opis = _opis.Text;
            dokMM.fl_header = 1;
            dokMM.UpdateMM(dokMM);
            dokMM.getMMki();
            flaga = 0;
            gidnumer = -1; StartTimer(false);
            Navigation.PopModalAsync();
            StartCreateMmPage startCreateMmPage = new StartCreateMmPage();
            startCreateMmPage.StartTimer(true);
            StartCreateMmPage.flagaMM = 1;

        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            flaga = 0;
            gidnumer = -1; StartTimer(false);
            Navigation.PopModalAsync();
            StartCreateMmPage startCreateMmPage = new StartCreateMmPage();
            startCreateMmPage.StartTimer(true);
            StartCreateMmPage.flagaMM = 1;
            return true;
        }
    }
}
