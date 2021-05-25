using App2.Model;
using App2.View.TworzPaczki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamarin.Forms.Button;
using static Xamarin.Forms.Button.ButtonContentLayout;

namespace App2.View
{
	public class AddDaneZleceniaPage : ContentPage
	{
        static public Entry _magDcl;
        static public string MagDcl;
        private Entry _opis;
        private Button _btnSave;
        //private Button _btnListSklep;

		public AddDaneZleceniaPage()
		{
            this.Title = "Dodaj Zlecenie";
            StackLayout stackLayout = new StackLayout();

            _magDcl = new Entry();
            _magDcl.Keyboard = Keyboard.Text;
            _magDcl.Placeholder = "Magazyn docelowy";
            _magDcl.HorizontalOptions = LayoutOptions.Center;
            _magDcl.Focused += _magDcl_Focused;
            //_magDcl.Unfocus();
            stackLayout.Children.Add(_magDcl);

            _opis = new Entry();
            _opis.Keyboard = Keyboard.Text;
            _opis.Placeholder = "Opis ( Nie wpisuj tu danych osobowych!)";
            stackLayout.Children.Add(_opis);

            //_btnListSklep = new Button();
            //_btnListSklep.Text = "Pokaż liste sklepów";
            //_btnListSklep.Clicked += _btnListSklep_Clicked;
            //stackLayout.Children.Add(_btnListSklep);

            _btnSave = new Button();
            _btnSave.Text = "Utwórz Zlecenie";
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
                MagDcl = sklep.mag_gidnumer;
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

        private async void Button_Clicked(object sender, EventArgs e) //zapisz mm
        {
            var app = Application.Current as App;
            try
            {
                if (_magDcl.Text == null || _opis.Text == null)
                {
                    await DisplayAlert(null, "Nie wypełeniono wszystkich danych..", "OK");
                }
                else
                {
                    //if (_magDcl.Text.IndexOf("MS")>=0)
                    //{
                        CreatePaczkaReposytorySQL reposytorySQL = new CreatePaczkaReposytorySQL();
                        FedexPaczka fedex = new FedexPaczka();
                        fedex.Fmm_MagDcl = MagDcl;
                        fedex.Fmm_MagZrd = app.MagGidNumer.ToString();
                        fedex.Fmm_Opis = _opis.Text;


                        //ListaZlecenView listaZlecen = new ListaZlecenView();
                        //listaZlecen.Fmm_MagDcl = MagDcl;
                        //listaZlecen.Fmm_Opis = _opis.Text;
                        //listaZlecen.Fmm_MagZrd = app.MagGidNumer.ToString();


                         
                        var id = Task.Run(() => reposytorySQL.GetLastGidNUmer()).Result;

                        await reposytorySQL.SaveZlecenieToBase(fedex, id, 0);
                        await DisplayAlert(null, "Dodano zlecenie dla : " + _magDcl.Text, "OK");
                     //   await NavigationPage.PushAsync(new CreatePaczkaListaPaczek(listaZlecen));
                        await Navigation.PopModalAsync();
                      
                    //}
                    //else
                    //{
                    //    await DisplayAlert(null, "Nie obsługiwany magazyn", "OK");
                    //}
                    
                   
                }
            }
            catch (Exception s)
            {
                 await DisplayAlert(null, s.Message, "OK");
            }

        }
    }
}