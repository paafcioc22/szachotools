using App2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace App2.View.TworzPaczki
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePaczkaListaMM : ContentPage
    {
        public ObservableCollection<FedexPaczka> Items { get; set; }
        int fmm_gidnumer;
        int fmm_eleNumer;
        private MobileBarcodeScanningOptions opts;
        private ZXingScannerPage scanPage;
        private ZXingScannerView zxing;
        FedexPaczka fedexPaczka;
        CreatePaczkaReposytorySQL reposytorySQL;
        public CreatePaczkaListaMM(FedexPaczka fedex)
        {
            InitializeComponent();
            this.Title = $"Lista MM w kartonie nr. {fedex.Fmm_EleNumer}";
            fedexPaczka = new FedexPaczka();
            this.fmm_gidnumer = fedex.Fmm_GidNumer;
            this.fmm_eleNumer = fedex.Fmm_EleNumer;
            fedexPaczka = fedex;
            BindingContext = this;
            reposytorySQL = new CreatePaczkaReposytorySQL();

            if (!string.IsNullOrEmpty(fedex.Fmm_NrListu))
                btn_dodajMM.IsEnabled = false;
        }


        private async void GetListaMM(int fmm_gidnumer,int fmm_eleNumer)
        {
            var query = $@"  cdn.PC_WykonajSelect '
              select Fmm_GidNumer,Fmm_EleNumer,max(Fmm_NrListu)Fmm_NrListu, Fmm_NazwaPaczki,max(Fmm_Elmenty)Fmm_Elmenty, max(Fmm_DataZlecenia)Fmm_DataZlecenia, max(Fmm_MagDcl)Fmm_MagDcl ,max(Fmm_MagZrd)Fmm_MagZrd
                    from cdn.PC_FedexMM
                    where fmm_gidnumer={fmm_gidnumer} and Fmm_EleNumer={fmm_eleNumer} and Fmm_NazwaPaczki<>''''
                    group by Fmm_EleNumer,Fmm_GidNumer,Fmm_NazwaPaczki'
                    ";//and trn_Stan = 1

            var lista = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);
            Items = lista;

            MyListView.ItemsSource = Items;
        }


        private async Task<bool> DodajMMDoKartonu(string text)
        {
            
            
            fedexPaczka.Fmm_NazwaPaczki = text;

            var test = await reposytorySQL.IsMMOKToSave(text, fedexPaczka.Fmm_MagDcl);

            if (test)
            {
                if (await reposytorySQL.IsNotReadyAdded(text))
                {
                    await reposytorySQL.SaveMMToBase(fedexPaczka);
                }
                else
                {
                    await DisplayAlert("Uwaga", "Skanowana MM została już wcześniej dodana", "OK");
                }
            } 
            else
                await DisplayAlert("Uwaga", $"{text} nie jest zatwierdzona lub nie pasuje do magazynu docelowego", "OK");


            return true;
        }


        


        protected override void OnAppearing()
        {
            GetListaMM(fmm_gidnumer, fmm_eleNumer);
            base.OnAppearing();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void BtnAddMM_Clicked(object sender, EventArgs e)
        {

            

            if (SettingsPage.SelectedDeviceType == 1)
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>() {

                ZXing.BarcodeFormat.CODE_128,
                ZXing.BarcodeFormat.CODABAR,
                ZXing.BarcodeFormat.CODE_39,

                }

                };

                opts.TryHarder = true;

                zxing = new ZXingScannerView
                {

                    IsScanning = true,
                    IsTorchOn = false,
                    IsAnalyzing = false,
                    AutomationId = "zxingDefaultOverlay",//zxingScannerView
                    Opacity = 22,
                    Options = opts
                };

                var torch = new Switch
                {
                };

                torch.Toggled += delegate
                {
                    scanPage.ToggleTorch();
                };

                var grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };

                var Overlay = new ZXingDefaultOverlay
                {
                    TopText = "Włącz latarkę",
                    BottomText = "Skanowanie rozpocznie się automatycznie",
                    ShowFlashButton = true,
                    AutomationId = "zxingDefaultOverlay",

                };

                var customOverlay = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
                };

                grid.Children.Add(Overlay);
                Overlay.Children.Add(torch);
                Overlay.BindingContext = Overlay;

                scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
                {
                    DefaultOverlayTopText = "Zeskanuj MM ",
                    //DefaultOverlayBottomText = " Skanuj kod ";
                    DefaultOverlayShowFlashButton = true

                };
                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;
                    scanPage.AutoFocus();
                    Device.BeginInvokeOnMainThread(async() =>
                    {

                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                        {
                            if (scanPage.IsScanning) scanPage.AutoFocus();
                            return true;
                        });
                        await Navigation.PopModalAsync();
                        await DodajMMDoKartonu(result.Text);

                    });
                };
                await Navigation.PushModalAsync(scanPage);
            }else
            {
                string odpowiedz = await DisplayPromptAsync("Dodawanie mmki", "", "OK", "Anuluj", "Wprowadź/skanuj nr MM:");
                if (!string.IsNullOrEmpty(odpowiedz))
                {
                    await DodajMMDoKartonu(odpowiedz.ToUpper());
                    GetListaMM(fmm_gidnumer, fmm_eleNumer);
                }
               
            }

                
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            var item = theViewCell.BindingContext as FedexPaczka;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
                if (string.IsNullOrEmpty(item.Fmm_NrListu))
                {
                    var moreAction = new MenuItem { Text = "Usuń MM" };
                    moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    moreAction.Clicked += MenuItem_Clicked;
                    theViewCell.ContextActions.Add(moreAction);
                }
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert(null, "Czy chcesz usunąć MM z listy?", "Tak", "Nie");
            if (action)
            {
                var usunKarton = (sender as MenuItem).CommandParameter as Model.FedexPaczka;

                await reposytorySQL.RemovePaczka(usunKarton,"MM");
                GetListaMM(fmm_gidnumer, fmm_eleNumer);
            }
        }
    }
}
