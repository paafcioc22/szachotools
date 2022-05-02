
using App2.Model;
using App2.ViewModel;
using Plugin.Media;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Foto2 : ContentPage
    {
        NagElem akcja;

        public string PhotoPath { get; private set; }
        PhotoViewModel viewModel;

        public Foto2(NagElem _nazwaAkcji, string sklep, bool isvisible)
        {
            InitializeComponent();

            BindingContext = viewModel = new PhotoViewModel(_nazwaAkcji, sklep);
            this.akcja = _nazwaAkcji;
            //viewModel.IsVisible = isvisible;
            //todo : odremuj
        }

        async Task TakePicture(string _nazwaAkcji)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                    CustomPhotoSize = 35,
                    Directory = "Sample",
                    Name = $"{_nazwaAkcji}.jpg",
                    CompressionQuality = 80,
                    SaveToAlbum = true
                });

                if (file == null)
                    return;

                PhotoPath = System.IO.Path.Combine(FileSystem.CacheDirectory, file.Path);

                var fileName = file.Path.Split("/");

                var czasnazwa = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

                var nazwa = $"{fileName.Last().Replace(".jpg", "")}_{czasnazwa}.jpg";

                viewModel.AddFoto(new Photo
                {
                    URL = PhotoPath,
                    Title = nazwa
                });
                viewModel.isDone = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        } 

        protected override bool OnBackButtonPressed()
        {
            if (!viewModel.isDone)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("uwaga!", "Nie wysłano zdjęć na serwer! Chcesz wyjść?", "Tak", "Nie");

                    if (result)
                    {
                       // System.Diagnostics.Process.GetCurrentProcess().(); // Or anything else
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        
                    }
                });
            }
            else
            {
                return false;
            }
            return true;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Photos.Count == 0)
                viewModel.LoadGaleryCommand.Execute(null);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await TakePicture(akcja.AkN_NazwaAkcji);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var toRemove = collectionView.SelectedItems;

            if (toRemove.Count > 0)
            {
                List<Photo> toRemovePhoto = new List<Photo>(toRemove.Select(x => (Photo)x));
                var AnyToRemove = toRemovePhoto.Where(s => s.URL.Contains("https://joartclickonce")).Any();
                if (AnyToRemove)
                {
                    await DisplayAlert("uwaga", "Nie można usunąć zdjęć znajdujących się na serwerze", "OK");
                }
                else
                {

                    foreach (Photo i in toRemove)
                    {
                        if (!i.URL.Contains("https://joartclickonce"))
                        {
                            viewModel.Photos.Remove(i);
                        }

                    }
                }
            }
        }

    }
}