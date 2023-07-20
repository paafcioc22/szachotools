﻿using App2.Model;
using FFImageLoading;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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
        public ObservableCollection<Model.AkcjeNagElem> Items;
        IList<Model.AkcjeNagElem> _listatwr;
        SqlConnection connection;

        public List_AkcjeAfterFiltr(IList<Model.AkcjeNagElem> nowa)
        {
            Items = new ObservableCollection<Model.AkcjeNagElem>();
          
            InitializeComponent();
            BindingContext = this;
            _listatwr = nowa;
            Items = Convert(nowa);

            var app = Application.Current as App;
            connection = new SqlConnection
            {
                ConnectionString = "SERVER=" + app.Serwer +
                ";DATABASE=" + app.BazaProd +
                ";TRUSTED_CONNECTION=No;UID=" + app.User +
                ";PWD=" + app.Password
            };

            //czyscPamiec();
            MyListView.ItemsSource = Items;
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
             
            var tmp= Items.OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();
            MyListView.ItemsSource = Convert(tmp); 
           
        }


        Int16 magnumer;
        private async void SendDataSkan(IList<AkcjeNagElem> sumaList)
        {
            if (SettingsPage.SprConn())
            {
                SqlCommand command = new SqlCommand();

                connection.Open();
                command.CommandText = $@"SELECT  [Mag_GIDNumer]
                                  FROM  [CDN].[Magazyny]
                                  where mag_typ=1 
								  and [Mag_GIDNumer] is not null
								  and mag_nieaktywny=0";


                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;

                rs = query.ExecuteReader();
                while (rs.Read())
                {
                    magnumer = System.Convert.ToInt16(rs["Mag_GIDNumer"]);
                }

                rs.Close();
                rs.Dispose();
                connection.Close();
                string ase_operator = View.LoginLista._user + " " + View.LoginLista._nazwisko;
                var odp = await App.TodoManager.InsertDataSkan(sumaList, magnumer, ase_operator);
                if (odp != "OK")
                    await DisplayAlert(null, odp, "OK"); 
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
    }
}