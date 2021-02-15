using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2.View
{
    public  class widoktestowy : ContentPage
    {
        
        
        
        
        public widoktestowy()
        {
            InitializeWidok();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void InitializeWidok()
        {
            var abosulview = new AbsoluteLayout();
            var stacklayut = new StackLayout();

            Label lbl_naglowek = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Dodawanie pozycji (raport przyjęć)",
                FontSize = 20,
                TextColor = Color.Bisque,
                BackgroundColor = Color.DarkCyan
            };
            AbsoluteLayout.SetLayoutBounds(lbl_naglowek, new Rectangle(0, 0, 1, .1));
            AbsoluteLayout.SetLayoutFlags(lbl_naglowek, AbsoluteLayoutFlags.All);

            #region Boxview
            var topBox = new BoxView { Color = Color.Blue };
            AbsoluteLayout.SetLayoutBounds(topBox, new Rectangle(0, 0, 1, .5));
            AbsoluteLayout.SetLayoutFlags(topBox, AbsoluteLayoutFlags.All);

            var bottomBox = new BoxView { Color = Color.Yellow };
            AbsoluteLayout.SetLayoutBounds(bottomBox, new Rectangle(0, 1, 1, .5));
            AbsoluteLayout.SetLayoutFlags(bottomBox, AbsoluteLayoutFlags.All); 
            #endregion


            var label1 = new Label()
            {
                Text = "Text 1",
                HorizontalOptions = LayoutOptions.Center
            };

            var label2 = new Label()
            {
                Text = "Text 2",
                HorizontalOptions = LayoutOptions.Center
            };

            var label3 = new Label()
            {
                Text = "Text 3",
                HorizontalOptions = LayoutOptions.Center
            };

            var entry = new Entry()
            {
                Placeholder = "wpusz EAN",
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center

            };

            var image = new Image()
            {
                Source= "http://serwer.szachownica.com.pl:81/zdjecia/KURTKI/230ZDTC-12.JPG",
                Aspect=Aspect.AspectFill
            };
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0, 0.1, 1, .5));
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.All);


            stacklayut.Children.Add(label1);
            stacklayut.Children.Add(label2);
            stacklayut.Children.Add(label3);
            stacklayut.Children.Add(entry);

            AbsoluteLayout.SetLayoutBounds(stacklayut, new Rectangle(0, 1, 1, .45));
            AbsoluteLayout.SetLayoutFlags(stacklayut, AbsoluteLayoutFlags.All);


            var button = new Button()
            {
                Text="Wykonaj akcje",
                CornerRadius=10,
                BorderColor = Color.DarkCyan,
                BorderWidth = 4


            };
            AbsoluteLayout.SetLayoutBounds(button, new Rectangle(0.5, 1, .9, 50));
            AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            var button2 = new Button()
            {
                Text = "Wykonaj akcje2",
                CornerRadius = 10,
                BorderColor=Color.DarkCyan,
                BorderWidth=4

            };
            AbsoluteLayout.SetLayoutBounds(button2, new Rectangle(0.5, .9, .9, 50));
            AbsoluteLayout.SetLayoutFlags(button2, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);


            //abosulview.Children.Add(topBox);
            abosulview.Children.Add(image);
            abosulview.Children.Add(lbl_naglowek);
            //abosulview.Children.Add(bottomBox);
            abosulview.Children.Add(stacklayut);
            abosulview.Children.Add(button);
            abosulview.Children.Add(button2);

            Content = abosulview;
        }
    }
}
