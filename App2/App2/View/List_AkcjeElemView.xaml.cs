﻿using System;
using System.Collections;
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
    public partial class List_AkcjeElemView : ContentPage
    {
        public IEnumerable<Model.AkcjeNagElem> Items2 { get; set; }
        public ObservableCollection<Model.AkcjeNagElem> ListaZFiltrem { get; set; }
         
        public List_AkcjeElemView( int gidtyp)
        {
            InitializeComponent();

            GetAkcje( gidtyp);

        }

        private async void GetAkcje(int _gidtyp)
        {
            try
            {
                string Webquery2 = $@"cdn.PC_WykonajSelect N'select distinct AkN_GidNumer, AkN_GidTyp  , AkN_GidNazwa ,AkN_NazwaAkcji, AkN_DataStart,AkN_DataKoniec,Ake_FiltrSQL
                    from cdn.pc_akcjeNag INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                     where AkN_GidTyp={_gidtyp} and AkN_DataKoniec>=GETDATE() -10
                     '";
                var AkcjeElemLista = await App.TodoManager.GetGidAkcjeAsync(Webquery2);
                ListaZFiltrem = AkcjeElemLista;

                Items2 = AkcjeElemLista.GroupBy(dd => dd.AkN_GidNumer).Select(a => a.First());
                MyListView2.ItemsSource = Items2;
            }
            catch (Exception x)
            {

                await DisplayAlert(null, x.Message, "OK");
            }
        }


         void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            var pozycja = e.Item as Model.AkcjeNagElem;

            var nowa =ListaZFiltrem.Where(x => x.AkN_GidNumer == pozycja.AkN_GidNumer).ToList();

            Navigation.PushModalAsync(new View.List_AkcjeTwrList(nowa));

            // await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}