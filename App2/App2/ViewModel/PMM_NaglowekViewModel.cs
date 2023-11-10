using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.View.PrzyjmijMM;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.Profile;

namespace App2.ViewModel
{
    public class PMM_NaglowekViewModel : ViewModelBase
    {
        ServicePrzyjmijMM serviceApi;
        public ObservableRangeCollection<PMM_DokNaglowek> DokumentMMs { get; set;}
        public ICommand LoadMMNaglowkiCommand { get; set; }
        public ICommand ItemTappedCommand { get;  set; }
                         

        private int _pastDays=10;
        private bool _czyZatwierdzone;

        public bool CzyZatwierdzone
        {
            get => _czyZatwierdzone;
            set => SetProperty(ref _czyZatwierdzone, value);
        }

        public int PastDays
        {
            get => _pastDays;
            set => SetProperty(ref _pastDays, value);
        }
     

        public PMM_NaglowekViewModel()
        {
            DokumentMMs= new ObservableRangeCollection<PMM_DokNaglowek>();
            serviceApi = new ServicePrzyjmijMM();
            LoadMMNaglowkiCommand =new Command(async () => await ExecuteLoadMMCommand(CzyZatwierdzone, PastDays));
            ItemTappedCommand = new Command<PMM_DokNaglowek>(OnItemTapped);
        }

        public async Task ExecuteLoadMMCommand(bool CzyZatwierdzone, int pastDays = 100)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            ApiResponse<List<DokumentMM>> listaMM=null;

            try
            {                
                listaMM = await serviceApi.GetDokMmList(CzyZatwierdzone, pastDays);

                if (listaMM.IsSuccessful)
                {

                    if (listaMM.Data != null)
                    {
                        DokumentMMs.Clear();

                        foreach (var dok in listaMM.Data.OrderByDescending(s => s.DataMM))
                        {
                            //todo : dodaj pobieranie statusu
                            //var tmpMM = await PobierzSatus(przyjmijMM);

                            var pmm = new PMM_DokNaglowek
                            {
                                Trn_Trnid = dok.Trn_Trnid,
                                DataMM = dok.DataMM,
                                TrN_DokumentObcy= dok.TrN_DokumentObcy,
                                Trn_Gidnumer= dok.Trn_Gidnumer,
                                Trn_MagZrdId= dok.Trn_MagZrdId,
                                TrN_Opis= dok.TrN_Opis
                               
                            };
                            //pmm.IsPrzyjetaMM = await PobierzSatus(przyjmijMM);   
                            DokumentMMs.Add(pmm);
                        }

                    }

                }

            }
            catch (Exception s)
            {
                var app = Application.Current as App;
                var properties = new Dictionary<string, string>
                {
                    { "conn", "/api/przyjmijMM/GetMmListNag" },
                    { "query", listaMM.ErrorMessage},
                    {"magkod",app.BazaProd },
                    {"user", App.SessionManager.CurrentSession.UserName }
                };

                Crashes.TrackError(s, properties);
                throw;
            }
            finally
            {
                IsBusy = false;
            }

        }

        private async void OnItemTapped(PMM_DokNaglowek item)
        {
 
            if (item != null)
            { 
                await Application.Current.MainPage.Navigation.PushAsync(new MMDetailPage(item));
            }
        }


    }
}
