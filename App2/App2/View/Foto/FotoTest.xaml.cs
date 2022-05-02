
using App2.Model;
using App2.ViewModel;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using NativeMedia;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FotoTest : ContentPage
    {
        NagElem akcja;

        public string PhotoPath { get; private set; }
        PhotoViewModel viewModel;

        public FotoTest(NagElem _nazwaAkcji, string sklep, bool isvisible)
        {
            InitializeComponent();
            Crashes.SetEnabledAsync(true);
            BindingContext = viewModel = new PhotoViewModel(_nazwaAkcji, sklep);
            this.akcja = _nazwaAkcji;
            viewModel.IsVisible = isvisible;
            //todo : odremuj
           
        }
        async Task PickImages(string _nazwaAkcji)
        {
            var cts = new CancellationTokenSource();
            IMediaFile[] files = null;

            try
            {
                var request = new MediaPickRequest(3, MediaFileType.Image)
                {
                    PresentationSourceBounds = System.Drawing.Rectangle.Empty,
                    UseCreateChooser = true,
                    Title = "Select" 
                };

                cts.CancelAfter(TimeSpan.FromMinutes(5));

                var results = await MediaGallery.PickAsync(request, cts.Token);
                files = results?.Files?.ToArray();
            }
            catch (OperationCanceledException)
            {
                // handling a cancellation request
            }
            catch (Exception)
            {
                // handling other exceptions
            }
            finally
            {
                cts.Dispose();
            }


            if (files == null)
                return;

            foreach (var file in files)
            {
                var fileName = file.NameWithoutExtension; //Can return an null or empty value
                var extension = file.Extension;
                var contentType = file.ContentType;
                using var stream = await file.OpenReadAsync();

                var path = await GetPath(file);
               
                var czasnazwa = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

                var nazwa = $"{_nazwaAkcji}_{czasnazwa}.jpg";
             
                viewModel.AddFoto(new Photo()
                {
                    URL = path,
                    Title = nazwa
                });
                viewModel.isDone = true;

                file.Dispose();
            }
        }

        async Task PickImages2(string _nazwaakcji)
        {
            int cntFile=0;
             
            var options = new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 35,
                CompressionQuality = 80
            };

            var multi = new MultiPickerOptions {
                MaximumImagesCount = 3,
                LoadingTitle = "wybirz"
                
            };
            var files = await Plugin.Media.CrossMedia.Current.PickPhotosAsync(options, multi);
         

            if (files == null)
                return;
             

            foreach (var file in files)
            {
                PhotoPath = file.Path;
                cntFile++;
                var fileName = file.Path.Split("/");

                var czasnazwa = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

                var nazwa = $"{_nazwaakcji} {cntFile}_{czasnazwa}.jpg";

                viewModel.AddFoto(new Photo()
                {
                    URL = PhotoPath,
                    Title = nazwa
                });
            }
            

            viewModel.isDone = true;

        }
        Task<string> GetPath(IMediaFile file)
        {
            string path="";
            return Task.Run(async () =>
            {
                IsBusy = true;
                try
                {
                    using var stream = await file.OpenReadAsync();

                    var name = (string.IsNullOrWhiteSpace(file.NameWithoutExtension)
                        ? Guid.NewGuid().ToString()
                        : file.NameWithoutExtension)
                        + $".{file.Extension}";

                    path= await FilesHelper.SaveToCacheAsync(stream, name);
                    stream.Position = 0;
                    
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(null,ex.Message,"OK");
                }
                IsBusy = false;
            return path;
            });
        }
        async Task TakePicture(string _nazwaAkcji)
        {

            //try
            //{
            //    var photo = await MediaPicker.CapturePhotoAsync();
            //    //var photo = await Device.InvokeOnMainThreadAsync(async () => await MediaPicker.CapturePhotoAsync());
            //    await LoadPhotoAsync(photo);
            //    Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            //}
            //catch (FeatureNotSupportedException fnsEx)
            //{
            //    // Feature is not supported on the device
            //}
            //catch (PermissionException pEx)
            //{
            //    // Permissions not granted
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            //}
            Analytics.TrackEvent("My custom event");
            bool isEnabled = await Crashes.IsEnabledAsync();

            #region oldMediaPicture
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
                Crashes.TrackError(ex);
            }
            #endregion
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }
        protected override bool OnBackButtonPressed()
        {
            if (!viewModel.isDone)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("uwaga!", "Nie wysłano zdjęć na serwer! Chcesz wyjść?", "Tak", "Nie");

                    if (result)                    {
                        
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
            //ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync();

            await TakePicture(akcja.AkN_NazwaAkcji);
            //await PickImages2(akcja.AkN_NazwaAkcji);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                //Crashes.GenerateTestCrash();
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
            catch (Exception s)
            {
                Crashes.TrackError(s);
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
             await PickImages2(akcja.AkN_NazwaAkcji);
        }
    }
}