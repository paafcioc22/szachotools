using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using static Xamarin.Forms.Button;
using static Xamarin.Forms.Button.ButtonContentLayout;

namespace App2.View
{
	public class AddMMPage : ContentPage
	{
        static public Entry _magDcl;
        private Entry _opis;
        private Button _btnSave;
        //private Button _btnListSklep;

		public AddMMPage ()
		{
            this.Title = "Dodaj MM";
            StackLayout stackLayout = new StackLayout();

            _magDcl = new Entry();
            _magDcl.Keyboard = Keyboard.Text;
            _magDcl.Placeholder = "Sklep docelowy";
            _magDcl.HorizontalOptions = LayoutOptions.Center;
            _magDcl.Focused += _magDcl_Focused;
            //_magDcl.Unfocus();
            stackLayout.Children.Add(_magDcl);

            _opis = new Entry();
            _opis.Keyboard = Keyboard.Text;
            _opis.Placeholder = "Opis (bez kto pakował - dodane z loginu)";
            stackLayout.Children.Add(_opis);

            //_btnListSklep = new Button();
            //_btnListSklep.Text = "Pokaż liste sklepów";
            //_btnListSklep.Clicked += _btnListSklep_Clicked;
            //stackLayout.Children.Add(_btnListSklep);

            _btnSave = new Button();
            _btnSave.Text = "Utwórz MMkę";
            _btnSave.Clicked += Button_Clicked;
            _btnSave.ImageSource = "create48x2.png";
            _btnSave.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayout.Children.Add(_btnSave);
            //stackLayout.HorizontalOptions = LayoutOptions.Center;
            stackLayout.VerticalOptions = LayoutOptions.Center;
            //stackLayout.HorizontalOptions = LayoutOptions.Center;

            Content = stackLayout;
		}

        private   void _magDcl_Focused(object sender, FocusEventArgs e)
        {
           // _magDcl.Unfocus();
            //  Navigation.PushModalAsync(new ListaSklepowPage("zapisz"));
            var page = new ListaSklepowPage("zapisz");
            page.ListViewSklepy.ItemSelected += (source, args) =>
            {
                var sklep = args.SelectedItem as Model.ListaSklepow;

                _magDcl.Text = sklep.mag_kod;
                Navigation.PopModalAsync();
            };

            Navigation.PushModalAsync(page);

        }

        private async void _btnListSklep_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ListaSklepowPage("zapisz"));
        }

        //ViewModel.DokMMViewModel DokMMViewModel;

        public static ObservableCollection<Model.DokMM> dokMMs = new ObservableCollection<Model.DokMM>();

        private  void Button_Clicked(object sender, EventArgs e) //zapisz mm
        {
            if (_magDcl.Text == null || _opis.Text == null)
            {
                DisplayAlert(null, "Nie wypełeniono wszystkich danych..", "OK");
            }
            else { 
            Model.DokMM dokMM = new Model.DokMM();
            dokMM.mag_dcl = _magDcl.Text;
            dokMM.opis = $"Pakował(a): {View.LoginLista._nazwisko}, {_opis.Text}";
            dokMM.fl_header = 1;

            dokMM.SaveMM(dokMM);
              DisplayAlert(null, "Utworzono MM dla : " + dokMM.mag_dcl, "OK");

            Navigation.PopModalAsync();
            //dokMM.getMMki();
            dokMMs = new Model.DokMM().getMMki();
            Model.DokMM.dokElementy.Clear();
            var mm = dokMMs.OrderByDescending(x => x.gidnumer).First();
             
            Navigation.PushModalAsync(new View.AddElementMMList(mm));
            }

        }
    }
}