﻿using App2.Model;
using App2.OptimaAPI;
using FFImageLoading;
using FFImageLoading.Cache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class List_AkcjeAfterFiltr : ContentPage
    {
        public ObservableCollection<AkcjeNagElem> Items { get; set; }
        IList<Model.AkcjeNagElem> _listatwr;
       
        //to jest lista ostatnia z towarami po kliku przechodzi do skanowania
        public List_AkcjeAfterFiltr(IList<AkcjeNagElem> nowa)
        {
          
            InitializeComponent();
            Items = new ObservableCollection<AkcjeNagElem>();
            //Items = Convert(nowa);
            //BindingContext = this;
            _listatwr = nowa;
             
         
            //czyscPamiec();
            //MyListView.ItemsSource = Items;
            ToggleScreenLock();
        }


        public void ToggleScreenLock()
        {
            DeviceDisplay.KeepScreenOn = !DeviceDisplay.KeepScreenOn;
        }


        async void czyscPamiec()
        {
            await ImageService.Instance.InvalidateCacheAsync(CacheType.All);
          //  CachedImage.InvalidateCache(string key, Cache.CacheType cacheType, bool removeSimilar = false)
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = this;

            //var tmp= Items.OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();
            ////MyListView.ItemsSource = Convert(tmp); 

            //Items = Convert(tmp);

            //Items = new ObservableCollection<AkcjeNagElem>();

            var tmp = _listatwr.OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();

            Items.Clear();
            foreach (var item in tmp)
            {
                Items.Add(item);
            }

        }


         
        private async Task SendDataSkan(IList<AkcjeNagElem> sumaList)
        {
            if (await SettingsPage.SprConn())
            {
                ServicePrzyjmijMM api = new ServicePrzyjmijMM();
                var magazyn = await api.GetSklepMagNumer();
                var magGidnumer = (short)magazyn.Id;

                string ase_operator = View.LoginLista._user + " " + View.LoginLista._nazwisko;
                var responsStatus = await App.TodoManager.InsertDataSkan(sumaList, magGidnumer, ase_operator);
                if (!responsStatus.Any())
                    await DisplayAlert(null, "Błąd synchronizacji danych z cetralą", "OK"); 
            }
        }


        public ObservableCollection<T> Convert<T>(IList<T> original)
        {
            return new ObservableCollection<T>(original);
        }


        private bool _istapped;
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (_istapped)
                return;

            _istapped = true;

            var pozycja = e.Item as AkcjeNagElem;

            await Navigation.PushModalAsync(new List_ScanPage(pozycja));

            _istapped = false;

            ((ListView)sender).SelectedItem = null;
        }


        public   IEnumerable<AkcjeNagElem> SzukajTowar(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return _listatwr;
            return _listatwr.Where(c => c.TwrEan.Contains(searchText.ToUpper()));
        
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyListView.ItemsSource = SzukajTowar(e.NewTextValue);
        }


        protected override bool OnBackButtonPressed()
        {
            ToggleScreenLock();

            return base.OnBackButtonPressed();
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_istapped)
                return;

            _istapped = true;
            var currentItem = e.CurrentSelection.FirstOrDefault() as AkcjeNagElem; // Zamień TwojTyp na faktyczny typ Twojego obiektu

            if (currentItem != null)
            {
                // Otwórz nowe okno i przekaż currentItem jako parametr
                Navigation.PushAsync(new List_ScanPage(currentItem));
            }
            _istapped = false;

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
