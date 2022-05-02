using App2.View.Foto;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class PhotoViewModel : ViewModelBase
    {
        string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=joartclickonce;AccountKey=N3+pUcRsgDlnpb5R0sv2m6moGzbCWlR/RQ1zomDu97t7Q1jn4ZP+7UbaGMV2HRNOOoQbdCgzt28mxNY1zjzSxQ==;EndpointSuffix=core.windows.net";


        BlobServiceClient client;
        internal BlobContainerClient containerClient;
        BlobClient blobClient;
        

        public List<string> fotki = new List<string>();
        readonly IList<Photo> source;
        public ObservableCollection<Photo> Photos { get; private set; }

        public Command AzureCommand => new Command(SendToAzure, CanExcecAzure);
        public Command LoadGaleryCommand => new Command(LoadGalery); 
        public ICommand GaleriaCommand => new Command(OpenGalery);

        public bool isDone=false;

        public PhotoViewModel(Model.NagElem _nazwaAkcji, string sklep="")
        {
            client = new BlobServiceClient(storageConnectionString);

            containerClient = client.GetBlobContainerClient("galeria2");

            source = new List<Photo>();
            Title = _nazwaAkcji.AkN_NazwaAkcji;
            if(!string.IsNullOrEmpty(sklep))
            Sklep = sklep;
            Photos = new ObservableCollection<Photo>(source);
            
        }

        public bool CanExcecAzure(object arg) 
        {
            return IsVisible;
        }        

        public void  AddFoto(Photo photo)
        {
            Photos.Add(photo);
            AzureCommand.ChangeCanExecute();
        }
     
        private async void OpenGalery()
        {
            fotki.Clear();

            string azureWeb = "https://joartclickonce.blob.core.windows.net/galeria2/";
            List<Photo> photos = new List<Photo>(); 

            await ListBlobsHierarchicalListing(containerClient, $"{Title}/{Sklep}", 1);

            foreach (var item in fotki)
            {
                photos.Add(new Photo
                {
                    URL = $"{azureWeb}{item}",
                    Title = item
                });
            }

            if (photos.Count > 0)
            {
                PhotoBrowserr photoBrowserr = new PhotoBrowserr(photos);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("info", "Folder ze zdjęciami jest pusty", "OK");
            } 

        }

        private async void LoadGalery(object obj)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                string azureWeb = "https://joartclickonce.blob.core.windows.net/galeria2/";
                List<Photo> photos = new List<Photo>();

                await ListBlobsHierarchicalListing(containerClient, $"{Title}/{Sklep}", 1);

                foreach (var item in fotki)
                {
                    AddFoto(new Photo
                    {
                        URL = $"{azureWeb}{item}",
                        Title = item
                    });
                }

                if (fotki.Count > 0)
                    isDone = true;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void SendToAzure(object obj)
        {
            if (IsBusy)
                return;

            int errors = 0;
            int sended = 0;

            IsBusy = true;

            try
            {
                foreach (Photo i in Photos)
                {
                    if (!i.URL.Contains("https://joartclickonce"))
                    {
                        var isOk = await SaveFileOnAzure(Sklep, i.URL, i.Title);
                        if (isOk)
                        {
                            isDone = true;
                            sended++;
                            //DoDeleteFile(i.URL);
                        }
                        else
                        {
                            isDone = false;
                            errors++;
                        }
                    }
                }

                if (errors > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("uwaga", "Nie wszystkie zdjęcia zostały zapisane - powtórz", "OK");
                    sended = 0;
                }
                else if(sended > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("info", $"Zdjęcia zostały zapisane na serwerze", "OK");
                    sended = 0;
                }

            }
            catch (Exception s)
            {
                await Application.Current.MainPage.DisplayAlert("error", s.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
            
        }                     

        public async Task ListBlobsHierarchicalListing(BlobContainerClient container, string prefix, int? segmentSize)
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

        private async Task<bool> SaveFileOnAzure(string sklep, string pathSource, string nameFile)
        {
            string containerName = $"galeria2";

            try
            {

                client = new BlobServiceClient(storageConnectionString);
                var dsada = client.GetAccountInfo();

                containerClient = client.GetBlobContainerClient(containerName);

                bool isExist = containerClient.Exists();

                if (!isExist)
                {
                    containerClient.CreateIfNotExists();
                }

                var blob = containerClient.GetBlobClient($"{Title}/{sklep}/{nameFile}");
                if (!blob.Exists())
                {
                    using (var fileStream = File.OpenRead(pathSource))
                    {
                       var response = await containerClient.UploadBlobAsync($"{Title}/{sklep}/{nameFile}", fileStream);
                      
                       //var isOK = !response.GetRawResponse().IsError;
                       //if (response.GetRawResponse().IsError)
                            return !response.GetRawResponse().IsError;
                       
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
             
        }
        void DoDeleteFile(string localPath)
        {
            if (File.Exists(localPath))
                File.Delete(localPath);
        }
        void RemovePhoto(List<Photo> photo)
        {   
            foreach (var item in photo)
            {
                Photos.Remove(item);
            }
        }

         
        
    }
}
