using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2.View
{
    public class SplashPage: ContentPage
    {
        Image splashImage;
        Label szachoLabel;
        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = "szacho.png",
                WidthRequest = 150,
                HeightRequest = 150
            };

            szachoLabel = new Label
            {
                Text = "SzachoTools",
                FontSize=20,
                HorizontalTextAlignment = TextAlignment.Center
            };
            AbsoluteLayout.SetLayoutFlags(splashImage,
               AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage,
             new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(szachoLabel,
                AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(szachoLabel,
                new Rectangle(.5, 1, .5, .1));

            splashImage.Opacity = 0.05;
            
            sub.Children.Add(splashImage);
            sub.Children.Add(szachoLabel);

            this.BackgroundColor = Color.FromHex("#F5FFFF");
            this.Content = sub;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // await splashImage.ScaleTo(1, 2000); //Time-consuming processes such as initialization
            //    szachoLabel.ScaleTo(0.8, 1500, Easing.Linear);
            //await splashImage.ScaleTo(0.8, 1500, Easing.Linear);


            await splashImage.FadeTo(0.8, 1000, Easing.Linear);

            szachoLabel.FadeTo(0, 1000, Easing.Linear); 
           await splashImage.FadeTo(0, 1000, Easing.Linear);
            //await splashImage.ScaleTo(0, 1500, Easing.Linear);
            Application.Current.MainPage = new NavigationPage(new StartPage());    //After loading  MainPage it gets Navigated to our new Page
        }
    }
}

