﻿using App2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePaczkaListaZlecen : ContentPage
    {
        public ObservableCollection<ListaZlecenView> Zlecenia { get; set; }

        public CreatePaczkaListaZlecen()
        {
            InitializeComponent();
            Zlecenia = new ObservableCollection<ListaZlecenView>();
            //var lista = Task.Run(() => GetList()).Result;

            BindingContext = this;
            
        }


        async void GetList()
        {
            var app = Application.Current as App;
            DateTime data;

            var querystring = $@"  cdn.PC_WykonajSelect '
                    select  Fmm_gidnumer, count(distinct fmm_elenumer)liczbapaczek, Fmm_nrlistu, max(fmm_datazlecenia) datautwrz, max(Fmm_NrDokWydania)Fmm_NrDokWydania
                    from cdn.pc_fedexmm 
                    where fmm_magzrd={app.MagGidNumer}
                    group by fmm_gidnumer,fmm_nrlistu '";//and trn_Stan = 1

            var zlecenia= await App.TodoManager.PobierzDaneZWeb<ListaZlecenView>(querystring);

            var tmp = zlecenia;

            foreach (var item in tmp)
            {
                data = Convert.ToDateTime(item.datautwrz);
                item.datautwrz = data.ToString("yyyy-MM-dd HH:mm:ss");
                Zlecenia.Add(item);
                
            }

            ListaZlecen.ItemsSource = Zlecenia;
            //return Zlecenia;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var pozycja = e.Item as ListaZlecenView;

            await DisplayAlert("Item Tapped", $"Kliknalem {pozycja.Fmm_gidnumer} blba bla", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            GetList();

            base.OnAppearing();
        }

        private async void BtnCreateNewZlecenie_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddDaneZleceniaPage());
        }
    }
}
