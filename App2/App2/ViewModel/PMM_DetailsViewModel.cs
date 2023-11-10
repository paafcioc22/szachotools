using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.View.PrzyjmijMM;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
 

namespace App2.ViewModel
{
    public class PMM_DetailsViewModel : ViewModelBase
    {
        ServicePrzyjmijMM serviceApi;
        public ObservableRangeCollection<TwrInfo> MMElements { get; set; }
        public ICommand LoadMmElementsCommand { get; set; }
        public ICommand StartSkanDostawyCommand { get; set; }

        public PMM_DetailsViewModel(PMM_DokNaglowek dokument)
        {
            MMElements= new ObservableRangeCollection<TwrInfo>();
            serviceApi = new ServicePrzyjmijMM();
            LoadMmElementsCommand = new Command(async () => await ExecuteLoadElementsCommand(dokument.Trn_Trnid));
            StartSkanDostawyCommand = new Command(async () => await OpenScanningProcessWindow());
            Title = dokument.TrN_DokumentObcy;
        }

        private async Task OpenScanningProcessWindow()
        {
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ScanningResultListPage());
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
                    var url = new Uri(FilesHelper.ConvertUrlToOtherSize(element.Twr_Url, element.Twr_Kod, FilesHelper.OtherSize.home));
                    element.Twr_UrlImage = new Xamarin.Forms.UriImageSource { Uri = url };
                }

                if (mmWithElements?.Elementy != null && mmWithElements.Elementy.Any())
                {
                    MMElements.Clear();
                    MMElements.AddRange(mmWithElements.Elementy);
                }
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
    }
}
