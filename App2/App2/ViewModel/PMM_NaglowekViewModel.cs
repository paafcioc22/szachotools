using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.View;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.Profile;

namespace App2.ViewModel
{
    public class PMM_NaglowekViewModel : ViewModelBase
    {
        private string _searchQuery;
        private int _pastDays=10;
        private bool _czyZatwierdzone;
        private List<PMM_DokNaglowek> originalList;

        ServicePrzyjmijMM serviceApi;
        public ObservableRangeCollection<PMM_DokNaglowek> DokumentMMs { get; set;}
        public ICommand LoadMMNaglowkiCommand { get; set; }
        public ICommand ItemTappedCommand { get;  set; }
        public ICommand ToggleSearchCommand { get; }
        public ICommand FilterCommand { get; }
        public bool IsSearchVisible { get; set; }
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(nameof(SearchQuery));
                    FilterCommand.Execute(null); // Wywołaj filtrowanie przy każdej zmianie zapytania
                }
            }
        } 
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
            originalList = new List<PMM_DokNaglowek>();
            DokumentMMs = new ObservableRangeCollection<PMM_DokNaglowek>();
            serviceApi = new ServicePrzyjmijMM();
            LoadMMNaglowkiCommand =new  Command(async () => await ExecuteLoadMMCommand(CzyZatwierdzone, PastDays));
            ItemTappedCommand = new Command<PMM_DokNaglowek>(OnItemTapped);
            ToggleSearchCommand = new Command(ToggleSearch);
            FilterCommand = new Command(FilterDokumentMMs);

        }

        private void FilterDokumentMMs()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // Przywróć oryginalną listę, jeśli pole wyszukiwania jest puste
                DokumentMMs.ReplaceRange(originalList);
            }
            else
            {
                // Filtrowanie listy na podstawie SearchQuery
                var filteredList = originalList.Where(dok => dok.TrN_DokumentObcy.Contains(SearchQuery)).ToList();
                DokumentMMs.ReplaceRange(filteredList);
            }
        }
        private void ToggleSearch()
        {
            IsSearchVisible = !IsSearchVisible;
            OnPropertyChanged(nameof(IsSearchVisible));
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
                var app = Application.Current as App;
                if (listaMM.IsSuccessful)
                {

                    if (listaMM.Data != null)
                    {
                        originalList.Clear();
                        
                        var tmpMM = await serviceApi.GetRaportyZCentrali(app.MagGidNumer);

                        foreach (var dok in listaMM.Data.OrderByDescending(s => s.DataMM))
                        {
                            

                            var pmm = new PMM_DokNaglowek
                            {
                                Trn_Trnid = dok.Trn_Trnid,
                                DataMM = dok.DataMM,
                                TrN_DokumentObcy= dok.TrN_DokumentObcy,
                                Trn_Gidnumer= dok.Trn_Gidnumer,
                                Trn_MagZrdId= dok.Trn_MagZrdId,
                                TrN_Opis= dok.TrN_Opis
                               
                            };
                            pmm.IsPrzyjetaMM = tmpMM.Any(s=>s.RrD_MMGidNumer==dok.Trn_Gidnumer);   
                            originalList.Add(pmm);
                        }

                    }

                }
                DokumentMMs.ReplaceRange(originalList);

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

        private async Task InitiateScan()
        {
            if (await CheckCameraPermissionAsync())
            {
                var zxingiewModel = new ZXingViewModel();
                var addItemPage = new ZxingScannerPage
                {
                    BindingContext = zxingiewModel
                };

                await Application.Current.MainPage.Navigation.PushModalAsync(addItemPage); 
              
            }
            else
            {
                // Uprawnienia nie przyznane, pokaż komunikat
                await Application.Current.MainPage.DisplayAlert("Błąd", "Brak uprawnień do aparatu.", "ΟΚ");
            }
        }
        private async Task<bool> CheckCameraPermissionAsync()
        {
            var granted = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (granted != PermissionStatus.Granted)
            {
                granted = await Permissions.RequestAsync<Permissions.Camera>();
            }
            return granted == PermissionStatus.Granted;
        }

    }
}
