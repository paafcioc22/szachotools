using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.View.PrzyjmijMM;
using MvvmHelpers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class PMM_DetailsViewModel : ViewModelBase
    {
        ServicePrzyjmijMM serviceApi;
        private readonly PMM_DokNaglowek dokument;

        public ObservableRangeCollection<TwrInfo> MMElements { get; set; }
        public ICommand LoadMmElementsCommand { get; set; }
        public ICommand StartSkanDostawyCommand { get; set; }
        public ICommand CopyEanCommand { get; private set; }

        public PMM_DetailsViewModel(PMM_DokNaglowek dokument)
        {
            MMElements= new ObservableRangeCollection<TwrInfo>();
            serviceApi = new ServicePrzyjmijMM();
            LoadMmElementsCommand = new Command(async () => await ExecuteLoadElementsCommand(dokument.Trn_Trnid));
            StartSkanDostawyCommand = new Command(async () => await OpenScanningProcessWindow());
            CopyEanCommand = new Command<string>(async (ean) => await ExecuteCopyEanCommand(ean));
            Title = dokument.TrN_DokumentObcy;
            this.dokument = dokument;
        }

        private async Task OpenScanningProcessWindow()
        {
           
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ScanningResultListPage(dokument));
        }

        private async Task ExecuteCopyEanCommand(string ean)
        {
            if (!string.IsNullOrWhiteSpace(ean))
            {
                #if DEBUG
                await Clipboard.SetTextAsync(ean);
                Debug.WriteLine($"kopiuje ean: {ean}");
                #endif
                // Możesz tu również dodać powiadomienie dla użytkownika
            }
        }

        private async Task ExecuteLoadElementsCommand(int dokumentId)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                var mmWithElements = await serviceApi.GetDokMMWithElements(dokumentId);

                foreach (var element in mmWithElements.Elementy)
                {
                    var url = new Uri(FilesHelper.ConvertUrlToOtherSize(element.Twr_Url, element.Twr_Kod, FilesHelper.OtherSize.small));
                    element.Twr_UrlImage = new Xamarin.Forms.UriImageSource { Uri = url };
                }

                if (mmWithElements?.Elementy != null && mmWithElements.Elementy.Any())
                {
                    MMElements.Clear();
                    MMElements.AddRange(mmWithElements.Elementy);
                    dokument.Elementy = mmWithElements.Elementy;
                }
            }
            catch (Exception s)
            {

                throw s;
            }
            finally
            {
                IsBusy = false;
            }


        }
    }
}
