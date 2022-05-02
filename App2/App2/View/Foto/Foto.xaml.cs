using Android.Graphics;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Plugin.Media;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.Foto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Foto : ContentPage
    {
        public string PhotoPath { get; private set; }
        string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=joartclickonce;AccountKey=N3+pUcRsgDlnpb5R0sv2m6moGzbCWlR/RQ1zomDu97t7Q1jn4ZP+7UbaGMV2HRNOOoQbdCgzt28mxNY1zjzSxQ==;EndpointSuffix=core.windows.net";

        
        BlobServiceClient client;
        BlobContainerClient containerClient;
        BlobClient blobClient;
        List<string> fotki = new List<string>();



        public Foto()
        {
            InitializeComponent();
        }



        protected override void OnAppearing()
        {
            //loadPicture();
            base.OnAppearing();
        }

        private async void loadPicture()
        {
            var blobname = "nazwaAkcji/boc/reultImage120220329134437.jpg";
            string containerName = $"galeria";

            client = new BlobServiceClient(storageConnectionString);

            containerClient = client.GetBlobContainerClient(containerName);

            blobClient = containerClient.GetBlobClient(blobname);

            BlobDownloadInfo downloadInfo = await blobClient.DownloadAsync();
            using MemoryStream memoryStream = new MemoryStream();


            await downloadInfo.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;




            if (blobClient.ExistsAsync().Result)
            {
                //using (var stream = await blobClient.OpenReadAsync())
                var url = new Uri("https://joartclickonce.blob.core.windows.net/galeria2/nazwaAkcji/boc/reultImage120220329134437.jpg");
                    reultImage1.Source = ImageSource.FromUri(url);
            }

                

        }

        private async void takeFoto_Clicked(object sender, EventArgs e)
        {
            //string containerName = $"quickstartblobs93280bd9-638c-4967-82e0-b1a3f157ccff";

            //client = new BlobServiceClient(storageConnectionString);
            //containerClient = client.GetBlobContainerClient(containerName);


            //blobClient = containerClient.GetBlobClient(fileName);

            //using MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));

            //await containerClient.UploadBlobAsync(fileName, memoryStream);

            string azureWeb= "https://joartclickonce.blob.core.windows.net/galeria2/";
            List<Photo> photos = new List<Photo>();

            client = new BlobServiceClient(storageConnectionString);

            containerClient = client.GetBlobContainerClient("galeria2");

            await ListBlobsHierarchicalListing(containerClient, $"nazwaAkcji/{lbl_nazwaSklepu.Text}", 1);


            foreach (var item in fotki)
            {
                photos.Add(new Photo
                {
                    URL = $"{azureWeb}{item}",
                    Title=item
                });
            }

            if (photos.Count > 0)
            {
                PhotoBrowserr photoBrowserr = new PhotoBrowserr(photos);
            }
            else
            {
                await DisplayAlert("info", "Brak zdjęć", "OK");
            }

        }

        private async Task SaveFileOnAzure(string nameFolder, string nameFile)
        {
            string containerName = $"galeria2";


            client = new BlobServiceClient(storageConnectionString);
            var dsada = client.GetAccountInfo();
           
            //containerClient = await client.CreateBlobContainerAsync(containerName);//utowrz folder 

            //containerClient = client.GetBlobContainerClient(containerName);//pobierz folder

            containerClient = client.GetBlobContainerClient(containerName);



            bool isExist = containerClient.Exists();

            if (!isExist)
            {

                containerClient.CreateIfNotExists();
            }

            //blobClient = containerClient.GetBlobClient(nameFile);

            using (var fileStream = File.OpenRead(PhotoPath))
            {
                //await blob.UploadFromStreamAsync(fileStream);

                //var nowy = Task.FromResult (await ResizeImage(fileStream)).Result;

                await containerClient.UploadBlobAsync($"nazwaAkcji/{lbl_nazwaSklepu.Text}/{nameFile}", fileStream);

            }



            //await containerClient.UploadBlobAsync(fileName, memoryStream);
        }

        private async void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        {
            var input = (Image)sender;

            await CrossMedia.Current.Initialize();

            string selected = input.StyleId.ToString();

            var time = DateTime.Now;

            var czasnazwa = time.ToString("yyyyMMddHHmmss"); 

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                    CustomPhotoSize = 35,
                    Directory = "Sample",
                    Name = "test.jpg",
                    CompressionQuality = 80
                });
                if (file == null)
                    return;


                input.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });

                PhotoPath = System.IO.Path.Combine(FileSystem.CacheDirectory, file.Path);
                await SaveFileOnAzure("", $"{selected}_{czasnazwa}.jpg");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



            // BtnPick_Clicked(input);
            return;

            //string selected = input.StyleId.ToString();

            //var time = DateTime.Now;

            //var czasnazwa= time.ToString("HHmmss");

            // var result = await MediaPicker.CapturePhotoAsync();


            //if (result != null)
            //{
            //    var stream = await result.OpenReadAsync();

            //    //await LoadPhotoAsync(result);
            //    PhotoPath=   System.IO.Path.Combine(FileSystem.CacheDirectory, result.FileName);
            //    input.Source = ImageSource.FromStream(() => stream);                


            //    SaveFileOnAzure("",$"{selected}_{czasnazwa}.jpg");
            //}

        }


        private async void BtnPick_Clicked(Image input)
        {
            await CrossMedia.Current.Initialize();

            string selected = input.StyleId.ToString();

            var time = DateTime.Now;

            var czasnazwa = time.ToString("yyyyMMddHHmmss");

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                    CustomPhotoSize = 35,
                    Directory = "Sample",
                    Name = "test.jpg",
                    CompressionQuality = 80
                });
                if (file == null)
                    return;


                input.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });

                PhotoPath = System.IO.Path.Combine(FileSystem.CacheDirectory, file.Path);

                SaveFileOnAzure("", $"{selected}{czasnazwa}.jpg");


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }




        protected async Task<Stream> ResizeImage(Stream stream)
        {
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }
            byte[] resizedImage = await ImageResizer.ResizeImage(imageData);


            var newStreamRes = new MemoryStream(resizedImage);

            return newStreamRes;
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
            var newFile = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }

        

        private async Task ListBlobsHierarchicalListing(BlobContainerClient container,string prefix,int? segmentSize)
        {


            try
            {
                // Call the listing operation and return pages of the specified size.
                var resultSegment = container.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/")
                    .AsPages(default, segmentSize);

                // Enumerate the blobs returned for each page.
                await foreach (Azure.Page<BlobHierarchyItem> blobPage in resultSegment)
                {
                    // A hierarchical listing may return both virtual directories and blobs.
                    foreach (BlobHierarchyItem blobhierarchyItem in blobPage.Values)
                    {
                        if (blobhierarchyItem.IsPrefix)
                        {
                            // Write out the prefix of the virtual directory.
                            Console.WriteLine("Virtual directory prefix: {0}", blobhierarchyItem.Prefix);
                            //resultsLabel.Text += $"{blobhierarchyItem.Prefix}{Environment.NewLine}";
                            // Call recursively with the prefix to traverse the virtual directory.
                            await ListBlobsHierarchicalListing(container, blobhierarchyItem.Prefix, null);
                        }
                        else
                        {
                            // Write out the name of the blob.
                            Console.WriteLine("Blob name: {0}", blobhierarchyItem.Blob.Name);
                            //resultsLabel.Text += $"{blobhierarchyItem.Blob.Name}{Environment.NewLine}";
                            fotki.Add(blobhierarchyItem.Blob.Name);
                        }
                    }

                    Console.WriteLine();
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
    }




    public static class ImageResizer
    {
        static ImageResizer()
        {
        }

        public static async Task<byte[]> ResizeImage(byte[] imageData, float width, float height)
        {
            return ResizeImageAndroid(imageData, width, height);
        }

        public static async Task<byte[]> ResizeImage(byte[] imageData)
        {
            return ResizeImageAndroid(imageData);
        }

        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }

        public static byte[] ResizeImageAndroid(byte[] imageData)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)(originalImage.Width * 0.5), (int)(originalImage.Height * 0.5), true);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 80, ms);
                return ms.ToArray();
            }
        }
    }
}