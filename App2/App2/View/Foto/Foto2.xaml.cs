
using App2.ViewModel;
using Plugin.Media;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Foto2 : ContentPage
    {
        
        public string PhotoPath { get; private set; }
        PhotoViewModel viewModel;

        public Foto2()
        {
            InitializeComponent();

            BindingContext = viewModel = new PhotoViewModel();  
      
        }

        async void TakePicture()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                    CustomPhotoSize = 35,
                    Directory = "Sample",
                    Name = "test.jpg",
                    CompressionQuality = 80
                });
                if (file == null)
                    return;


                //input.Source = ImageSource.FromStream(() =>
                //{
                //    var imageStram = file.GetStream();
                //    return imageStram;
                //});

                PhotoPath = System.IO.Path.Combine(FileSystem.CacheDirectory, file.Path);

                viewModel.Photos.Add(new Photo
                {
                    URL = PhotoPath
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnAppearing()
        {
            

            base.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            TakePicture();
           
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var toRemove = collectionView.SelectedItems;

            collectionView.SelectedItems.Remove(toRemove);
        }
    }
}