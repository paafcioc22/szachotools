using App2.Model.ApiModel;
using App2.OptimaAPI;
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
        //public static ObservableCollection<Model.DokMM> dokMMs = new ObservableCollection<Model.DokMM>();
     

		public AddMMPage ()
		{
            this.Title = "Dodaj MM";
            StackLayout stackLayout = new StackLayout();

            _magDcl = new Entry();
            _magDcl.Keyboard = Keyboard.Text;
            _magDcl.Placeholder = "Sklep docelowy";
            _magDcl.HorizontalOptions = LayoutOptions.Center;
            _magDcl.Focused += _magDcl_Focused; 
            stackLayout.Children.Add(_magDcl);

            _opis = new Entry();
            _opis.Keyboard = Keyboard.Text;
            _opis.Placeholder = "Opis ( Nie wpisuj tu danych osobowych!)";
            stackLayout.Children.Add(_opis); 

            _btnSave = new Button();
            _btnSave.Text = "Utwórz MMkę";
            _btnSave.Clicked += BtnSaveMM_Click;
            _btnSave.ImageSource = "create48x2.png";
            _btnSave.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayout.Children.Add(_btnSave);
       
            stackLayout.VerticalOptions = LayoutOptions.Center;
     

            Content = stackLayout;
		}

        private   void _magDcl_Focused(object sender, FocusEventArgs e)
        {
  
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

       


        private async void BtnSaveMM_Click(object sender, EventArgs e) //zapisz mm
        {
            try
            {
                if (_magDcl.Text == null || _opis.Text == null)
                {
                    await DisplayAlert(null, "Nie wypełeniono wszystkich danych..", "OK");
                }
                else
                {
                    ServiceDokumentyApi dokumentyApi = new ServiceDokumentyApi();
                    CreateDokumentNaglowek create = new CreateDokumentNaglowek()
                    {
                        MagKod = _magDcl.Text,
                        Opis= $"Pakował(a): {View.LoginLista._nazwisko}, {_opis.Text}",
                        GidTyp= GidTyp.Mm,
                        UserName= View.LoginLista._nazwisko
                    };

                    var dokMm= await dokumentyApi.SaveDokument(create);
                    if (dokMm.IsSuccessful)
                    {
                        await DisplayAlert(null, "Utworzono MM dla : " + dokMm.Data.MagKod, "OK");
                    }
                    else
                    {
                        await DisplayAlert("Uwaga", dokMm.ErrorMessage, "OK");
                    }

                    //Model.DokMM dokMM = new Model.DokMM();
                    //dokMM.mag_dcl = _magDcl.Text;
                    //dokMM.opis = $"Pakował(a): {View.LoginLista._nazwisko}, {_opis.Text}";
                    //dokMM.fl_header = 1;

                    //dokMM.SaveMM(dokMM);
                    //DisplayAlert(null, "Utworzono MM dla : " + dokMM.mag_dcl, "OK");

                    await Navigation.PopModalAsync();
                
                    //dokMMs = new Model.DokMM().getMMki();
                    //Model.DokMM.dokElementy.Clear();
                    //var mm = dokMMs.OrderByDescending(x => x.gidnumer).First();


                    if (!List_AkcjeView.TypAkcji.Contains("Zwrot"))
                        await Navigation.PushModalAsync(new View.AddElementMMList(dokMm.Data));
                }
            }
            catch (Exception s)
            {
                 await DisplayAlert(null, s.Message, "OK");
            }

        }
    }
}