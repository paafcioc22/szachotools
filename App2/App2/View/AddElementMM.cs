using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static Xamarin.Forms.Button;
using static Xamarin.Forms.Button.ButtonContentLayout;

namespace App2.View
{
    //public class ContextActionsCell : ViewCell
    //{
    //    public ContextActionsCell()
    //    {
    //        var deleteAction = new MenuItem { Text = "Usuń towar", IsDestructive = true }; // red background
    //        deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
    //        deleteAction.Clicked += DeleteAction_Clicked;
    //        ContextActions.Add(deleteAction);
    //    }


    //    private void DeleteAction_Clicked(object sender, EventArgs e)
    //    {
    //        var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
    //        Model.DokMM.dokElementy.Remove(usunMM);
    //        Model.DokMM.DeleteElementMM(usunMM);
    //    }
    //}
        class AddElementMM : ContentPage
        {
         
            private Entry _opis;
            public static Entry _magDcl;
            private Label _gidnumer;
           // private Entry _ean;
           // private Entry _ilosc;

            private Button _btnAddElement;
          //  private Button _btnScanElement;
            private Button _btnSave;

            public Int32 gidnumer;

        public AddElementMM(Model.DokMM mmka)
        {
            this.Title = "Dodaj Element";

            StackLayout stackglowny = new StackLayout();
            StackLayout stackLayout = new StackLayout();
            StackLayout stackLayoutBtn = new StackLayout();
            StackLayout stackHeader1 = new StackLayout();
            StackLayout stackHeader = new StackLayout();
            gidnumer = mmka.gidnumer;

                _gidnumer = new Label();
                _gidnumer.Text = "Nr :" + mmka.gidnumer.ToString();
                stackLayout.Children.Add(_gidnumer);
            //---------------------------stack header 1-----------------------------------------------------

            Label lbl_mag = new Label();
                lbl_mag.Text = "Sklep :";
                lbl_mag.VerticalOptions = LayoutOptions.Center;
            //lbl_mag.IsEnabled = (mmka.nrdok == "" ? true : false);
            //stackHeader1.BackgroundColor = Color.FromHex("#FFDAB9"); 
            stackHeader1.Children.Add(lbl_mag);


            _magDcl = new Entry();    /// dcl_mag
            _magDcl.Text = mmka.mag_dcl + (mmka.nrdok == "" ? "" : " - " + mmka.nrdok) ;
            _magDcl.IsEnabled = (mmka.nrdok == "" ? true : false); 
            _magDcl.Focused += _magDcl_Focused;
            _magDcl.WidthRequest = (mmka.nrdok == "" ? 70 : 300);
            stackHeader1.Children.Add(_magDcl);
            stackHeader1.Orientation = StackOrientation.Horizontal;
            stackHeader1.Spacing = 1;
            stackHeader1.HorizontalOptions = LayoutOptions.StartAndExpand;
            stackLayout.Children.Add(stackHeader1);

            // wiersz 2----------------------------------------
            Label lbl_opis = new Label();
                lbl_opis.Text = "Opis :";
                lbl_opis.VerticalOptions = LayoutOptions.Center;
                stackHeader.Children.Add(lbl_opis);

            _opis = new Entry();
            _opis.Text = mmka.opis;
            _opis.IsEnabled = (mmka.nrdok == "" ? true : false);
            _opis.HorizontalOptions = LayoutOptions.StartAndExpand; //--------<<<<<<<<<<<<<<dodalem 
            _opis.WidthRequest = 300;
             
            stackHeader.Children.Add(_opis);
            stackHeader.Orientation = StackOrientation.Horizontal;
            //stackHeader.BackgroundColor = Color.FromHex("#7CFC00");
            stackHeader.Spacing = 1;
            stackLayout.Children.Add(stackHeader);
            //------------------------------------------------------------------------------------------//

            stackLayout.BackgroundColor = Color.FromHex( "#f9efff");
            stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;// AndExpand;
            stackLayout.VerticalOptions = LayoutOptions.Start;
            stackLayout.Spacing = 1;
            stackLayout.Orientation = StackOrientation.Vertical;

         stackglowny.Children.Add(stackLayout);
            //-------------------------------------- LIST VIEW------------------------------------------
            //ListView listView2 = new ListView();
            Model.DokMM DokMM = new Model.DokMM();
            //listView2.BindingContext = Model.DokMM.dokElementy;
            //listView2.ItemsSource= Model.DokMM.dokElementy;

            Label header = new Label
            {
                Text = "Lista towarów MM",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center 
            };
            stackglowny.Children.Add(header);


            ListView listView = new ListView
            {
                // Source of data items.
                ItemsSource = Model.DokMM.dokElementy,
                BindingContext = Model.DokMM.dokElementy,
                HasUnevenRows=true,
            // Define template for displaying each item.
            // (Argument of DataTemplate constructor is called for 
            //      each item; it must return a Cell derivative.)
                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Label lbl_twrkod = new Label();
                    lbl_twrkod.SetBinding(Label.TextProperty, "twrkod");
                    lbl_twrkod.TextColor = Color.FromHex("#EE82EE");

                    Label lbl_ilosc = new Label();
                    lbl_ilosc.SetBinding(Label.TextProperty, new Binding("szt", stringFormat: "{0} szt"));

                    
                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                      
                        View = new StackLayout
                        {
                            Padding = new Thickness(10, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                                {
                                new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            lbl_twrkod,
                                            lbl_ilosc
                                        }
                                    }
                                }
                        } 
                    };
                })
            };

            var deleteAction = new MenuItem { Text = "Usuń towar", IsDestructive = true }; // red background
            deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            deleteAction.Clicked += async (sender, e) => {
                var mi = ((MenuItem)sender);
                var action = await DisplayAlert(null, "Czy chcesz usunąć MM z listy?", "Tak", "Nie");
                if (action)
                {
                    var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
                    Model.DokMM.dokElementy.Remove(usunMM);
                    Model.DokMM.DeleteElementMM(usunMM);
                }
            };


            stackglowny.Children.Add(listView);

           
                //-------------------------------------- END LIST VIEW-------------------------------
                _btnSave = new Button();
            _btnSave.Text = "Zapisz Zmiany";
            //stackLayout.Children.Add(_btnSave);
            _btnSave.HorizontalOptions = LayoutOptions.FillAndExpand;
            _btnSave.Clicked += _btnSave_Clicked;
            _btnSave.Image = "save48x2.png";
            _btnSave.IsEnabled = (mmka.statuss == 0? true : false);
            _btnSave.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);


            stackLayoutBtn.Children.Add(_btnSave);

            _btnAddElement = new Button();
            _btnAddElement.Text = "Dodaj pozycję";
            //stackLayout.Children.Add(_btnSave);
            _btnAddElement.Clicked += _btnAddElement_Clicked;
            _btnAddElement.HorizontalOptions= LayoutOptions.FillAndExpand;
            _btnAddElement.Image = "add48x2.png";
            _btnAddElement.IsEnabled = (mmka.statuss == 0 ? true : false);
            _btnAddElement.ContentLayout = new ButtonContentLayout(ImagePosition.Right, 10);
            stackLayoutBtn.Children.Add(_btnAddElement);  

                stackLayoutBtn.VerticalOptions= LayoutOptions.EndAndExpand;
                stackLayoutBtn.HorizontalOptions= LayoutOptions.FillAndExpand; 
                //stackLayoutBtn.BackgroundColor = Color.Yellow;
                stackLayoutBtn.Spacing = 1;


            //stackglowny.Children.Add(stackLayout);
            stackglowny.Children.Add(stackLayoutBtn);
            //stackglowny.VerticalOptions = LayoutOptions.StartAndExpand;
            //stackglowny.HorizontalOptions = LayoutOptions.Center;
            stackglowny.Orientation = StackOrientation.Vertical;
            stackglowny.Spacing = 1;
            Content = stackglowny;
        }

        private async void _magDcl_Focused(object sender, FocusEventArgs e)
        {
            await Navigation.PushModalAsync(new ListaSklepowPage("edytuj"));
        }

        private void _btnSave_Clicked(object sender, EventArgs e) // update dane
        {
            Model.DokMM dokMM = new Model.DokMM();
            dokMM.gidnumer =gidnumer;
            dokMM.mag_dcl = _magDcl.Text;
            dokMM.opis = _opis.Text;
            dokMM.fl_header = 1;
            dokMM.UpdateMM(dokMM);
            dokMM.getMMki();
            Navigation.PopModalAsync();

        }

        private async void _btnAddElement_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new View.AddTwrPage(gidnumer));
        }

        //private async void aaa()
        //{
        //    var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
        //    deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
        //    deleteAction.Clicked += async (sender, e) =>
        //    {
        //        var mi = ((MenuItem)sender);

        //    };
        //    var action = await DisplayAlert(null, "Czy chcesz usunąć MM z listy?", "Tak", "Nie");
        //}   
    }
}
