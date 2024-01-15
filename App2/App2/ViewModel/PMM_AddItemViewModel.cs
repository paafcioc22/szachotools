using App2.Model;
using App2.Model.ApiModel;
using App2.Services;
using App2.View;
using App2.View.PrzyjmijMM;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebApiLib.Serwis;
using Xamarin.Essentials;
using Xamarin.Forms;
using static App2.View.PrzyjmijMM.AddSkanElementPage;

namespace App2.ViewModel
{
    public class PMM_AddItemViewModel : ViewModelBase
    {
        private readonly SQLiteRepository _repository;
        private PMM_DokNaglowek _dokument;
        private TwrInfo _selectedTwrInfo;
        private InventoriedItem _existingItem;
        //private readonly PMM_RaportElement element;

        private PMM_RaportElement _element;
        public PMM_RaportElement Element
        {
            get => _element;
            set
            {
                SetProperty(ref _element, value);
                OnPropertyChanged(nameof(IsDeleteButtonVisible)); // Powiadom o zmianie widoczności przycisku
            }
        }


        private readonly ObservableRangeCollection<PMM_DiffRaportViewModel.Grouping<string, PMM_RaportElement>> groupedDifferences;
        private string? _scannedEan;
        private int? _inventoriedQuantity;
        private int _itemOrder;
        private bool _isEditMode;
        private int _valueFromDok;
        private bool _isEntryIloscEnabled;
        private UserDecision userDecision;
        private InventoriedItem item;
        private bool isScanning = false;
        public bool IsFirstLoad = true;

        public bool IsWarrningEnable => !_isEntryIloscEnabled && _isEditMode;

        public string? ScannedEan
        {
            get => _scannedEan;
            set => SetProperty(ref _scannedEan, value);
        }


        public TwrInfo SelectedTwrInfo
        {
            get => _selectedTwrInfo;
            set => SetProperty(ref _selectedTwrInfo, value);
        }
        public int ItemOrder
        {
            get => _itemOrder;
            set => SetProperty(ref _itemOrder, value);
        }
        public int? InventoriedQuantity
        {
            get => _inventoriedQuantity;
            set
            {
                SetProperty(ref _inventoriedQuantity, value);
                SaveCommand.RaiseCanExecuteChanged();// Informuje Command o zmianie możliwości wykonania
            }
        }
        public bool IsEntryIloscEnabled
        {
            get => _isEntryIloscEnabled;
            set => SetProperty(ref _isEntryIloscEnabled, value);
        }

        public int ValueFromDok
        {
            get => _valueFromDok;
            set { _valueFromDok = value; }
        }
        public bool IsValueFromDokPositive => ValueFromDok > 0;

        public AsyncCommand SaveCommand { get; private set; }
        public ICommand ScanCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand StartCameraCommand { get; private set; }
        public static IszachoApi Api => DependencyService.Get<IszachoApi>();

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetProperty(ref _isEditMode, value);
                OnPropertyChanged(nameof(IsDeleteButtonVisible)); // Powiadom o zmianie widoczności przycisku
            }
        }
        public bool IsDeleteButtonVisible => IsEditMode && (Element?.GroupName == "Nadstany" || Element == null);


        public event EventHandler<InventoriedItem> RequestDelete;

        public PMM_AddItemViewModel(SQLiteRepository repository, PMM_DokNaglowek dokument)
        {

            _dokument = dokument;
            Title ="Dodawanie nowego elementu";
            _repository = repository;
            //SaveCommand = new Command(async () => await Save());
            SaveCommand = new AsyncCommand(Save, _ => CanSave());
            //CheckAndSaveItemCommand = new Command<InventoriedItem>(async item => await CheckAndSaveItem(item));
            ScanCommand = new Xamarin.Forms.Command<string>(async (ean) => await ScanForProduct(ean));
            StartCameraCommand = new Xamarin.Forms.Command(async () => await InitiateScan());

            MessagingCenter.Subscribe<AddSkanElementPage, UserDecision>(this, "UserDecisionMade", (sender, decision) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await HandleUserDecision(decision);
                });
            });
        }

        public PMM_AddItemViewModel(SQLiteRepository repository, PMM_DokNaglowek dokument, InventoriedItem item
            , PMM_RaportElement element = null
            , MvvmHelpers.ObservableRangeCollection<PMM_DiffRaportViewModel.Grouping<string, PMM_RaportElement>> groupedDifferences = null)
        {
            _repository = repository;
            _dokument = dokument;
            _existingItem = item;
            this.Element = element;
            this.groupedDifferences = groupedDifferences;
            SaveCommand = new AsyncCommand(Save, _ => CanSave());
            DeleteCommand = new Xamarin.Forms.Command(() => OnDeleteRequested(item));
            Title = item.Twr_Kod;
            SelectedTwrInfo = new TwrInfo()
            {
                Twr_Url = FilesHelper.ConvertUrlToOtherSize(item.ImageUrl, item.Twr_Kod, FilesHelper.OtherSize.large),
                Twr_Ean = item.Twr_Ean,
                Twr_Nazwa = item.Twr_Nazwa,
                Twr_Kod = item.Twr_Kod,
            };

            var pozycjaZDokumentu = dokument.Elementy.FirstOrDefault(s => s.Twr_Ean == item.Twr_Ean);
            if (pozycjaZDokumentu != null)
            {
                SelectedTwrInfo.Twr_Cena = pozycjaZDokumentu.Twr_Cena;
                ValueFromDok = pozycjaZDokumentu.Stan_szt;
            }

            if (item.ActualQuantity > 0)
                IsEntryIloscEnabled = true;

            IsEditMode = true;
            InventoriedQuantity = item.ActualQuantity;

        }

        public  async Task InitiateScan()
        {
            if (isScanning) return; // Zapobiegaj ponownemu uruchomieniu
            isScanning = true;

            if (  await CheckCameraPermissionAsync())
            {
               
                var zxingiewModel = new ZXingViewModel();
                var addItemPage = new ZxingScannerPage
                {
                    BindingContext = zxingiewModel
                };

                await Task.Delay(100);
                await Application.Current.MainPage.Navigation.PushModalAsync(addItemPage);

                zxingiewModel.ScanCompleted += async (sender, result) =>
                {
                    // Logika obsługi wyniku skanowania
                    ScannedEan = result;
                    await Task.Delay(100);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    await ScanForProduct(ScannedEan);
                    zxingiewModel.StopScanning();
                };

                isScanning = false;

            }
            else
            {
                // Uprawnienia nie przyznane, pokaż komunikat
                await Application.Current.MainPage.DisplayAlert("Błąd", "Brak uprawnień do aparatu.", "ΟΚ");
                isScanning = false;
            }
        }

        private void OnDeleteRequested(InventoriedItem item)
        {
            // Przekaż prośbę o usunięcie do widoku
            RequestDelete?.Invoke(this, item);
        }
        public async Task DeleteExceute(InventoriedItem item)
        {
            await _repository.DeleteItemAsync(item);

            if (groupedDifferences != null)
            {
                foreach (var group in groupedDifferences)
                {
                    if (group.Contains(Element))
                    {
                        group.Remove(Element);

                        // Jeśli grupa jest teraz pusta, możesz zdecydować się na jej usunięcie
                        if (!group.Any())
                        {
                            groupedDifferences.Remove(group);
                        }

                        break; // Przerwij pętlę po usunięciu elementu
                    }
                }
            }

        }





        private bool CanSave()
        {
            // Możesz dodać dodatkową logikę walidacji jeśli potrzebujesz
            return InventoriedQuantity.HasValue && InventoriedQuantity.Value > 0;
        }

        UserDecision MapujDecyzje(string decision)
        {
            switch (decision)
            {
                case "Dodaj ilość":
                    return UserDecision.AddQuantity;
                case "Zastąp ilość":
                    return UserDecision.ReplaceQuantity;
                case "Anuluj":
                    return UserDecision.Cancel; // Możesz zdecydować co zrobić w przypadku anulowania
                default:
                    return UserDecision.Cancel; // Domyślnie, jeśli użytkownik kliknie poza, to anuluj
            }
        }

        private async Task HandleUserDecision(UserDecision decision)
        {
            if (_existingItem == null)
            {
                // Jeśli nie ma _existingItem, to nie można kontynuować
                return;
            }

            userDecision = decision;
            try
            {
                switch (decision)
                {
                    case UserDecision.AddQuantity:
                        if (InventoriedQuantity.HasValue)
                        {
                            _existingItem.ActualQuantity += InventoriedQuantity.Value;
                            await _repository.SaveItemAsync(_existingItem);
                        }
                        break;
                    case UserDecision.ReplaceQuantity:
                        if (InventoriedQuantity.HasValue)
                        {
                            _existingItem.ActualQuantity = InventoriedQuantity.Value;
                            await _repository.SaveItemAsync(_existingItem);
                        }
                        break;
                    case UserDecision.Cancel:
                        // Logika dla anulowania operacji
                        break;
                }

                // Po obsłużeniu decyzji, wyczyść _existingItem
                _existingItem = null;

                if (_existingItem == null || userDecision == UserDecision.Cancel)
                {
                    // Zakończ operację zapisu i wróć do poprzedniego okna
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                // Obsługa wyjątków, np. wyświetlenie komunikatu o błędzie
                Debug.WriteLine($"Wystąpił błąd podczas zapisywania: {ex.Message}");
                // Możesz tu wyświetlić komunikat o błędzie użytkownikowi
            }

        }

        private async Task Save()
        {
            try
            {
                _existingItem = await _repository.GetItemAsync(_dokument.Trn_Trnid, SelectedTwrInfo.Twr_Ean);
                if (_existingItem != null)
                {
                    _existingItem.ImageUrl = FilesHelper.ConvertUrlToOtherSize(_existingItem.ImageUrl, _existingItem.Twr_Kod, FilesHelper.OtherSize.small);
                    if (!IsEditMode)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send(this, "AskUserDecision", _existingItem);
                        });
                    }
                    else
                    {
                        if (InventoriedQuantity.HasValue)
                        {

                            _existingItem.ActualQuantity = InventoriedQuantity.Value;
                            await _repository.SaveItemAsync(_existingItem);

                            _existingItem = null;

                            MessagingCenter.Send(this, "RefreshReport", true);

                            //await Application.Current.MainPage.Navigation.PopAsync();

                        }
                    }
                }
                else
                {
                    if (InventoriedQuantity.HasValue)
                    {
                        if (InventoriedQuantity == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Uwaga", "Jeśli chcesz usunąć element z raportu użyj przycisku na dole okna", "OK");
                        }
                        else
                        {
                            var inventaryProduct = new InventoriedItem()
                            {
                                Twr_Ean = SelectedTwrInfo.Twr_Ean,
                                ActualQuantity = InventoriedQuantity ?? 0,
                                //ImageUrl = SelectedTwrInfo.Twr_Url,
                                ImageUrl = FilesHelper.ConvertUrlToOtherSize(SelectedTwrInfo.Twr_Url, SelectedTwrInfo.Twr_Kod, FilesHelper.OtherSize.small),
                                DokumentId = _dokument.Trn_Trnid,
                                Twr_Kod = SelectedTwrInfo.Twr_Kod,
                                Twr_Nazwa = SelectedTwrInfo.Twr_Nazwa,
                                ScannedTime = DateTime.Now,
                                ItemOrder = ItemOrder
                            };

                            await _repository.SaveItemAsync(inventaryProduct);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Uwaga", "Nie wpisano ilości", "OK");
                    }
                }

                if (_existingItem == null || userDecision == UserDecision.Cancel)
                {
                    // Zakończ operację zapisu i wróć do poprzedniego okna
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception s)
            {


            }
        }

        private async Task<int> CalculateItemOrder(int trnGidnumer)
        {
            // Logika do obliczenia numeru porządkowego dla danego Twr_Gidnumer
            var lastItem = await _repository.GetLastItemOrderForTwrGidnumer(trnGidnumer);
            return lastItem?.ItemOrder + 1 ?? 1; // Jeśli nie ma jeszcze żadnego elementu, zacznij od 1
        }

        private async Task ScanForProduct(string ean)
        {
            // Przeszukaj listę elementów w _dokument.Elementy, aby znaleźć produkt z odpowiadającym EAN
            try
            {
                if (!string.IsNullOrEmpty(ean))
                {

                    var searchedItem = _dokument.Elementy.FirstOrDefault(s => s.Twr_Ean == ean);

                    if (searchedItem != null)
                    {
                        searchedItem.Twr_Url = FilesHelper.ConvertUrlToOtherSize(searchedItem.Twr_Url, searchedItem.Twr_Kod, FilesHelper.OtherSize.home, true);
                        // Jeśli znaleziono, ustaw jako wybrany produkt
                        IsEntryIloscEnabled = true;
                        SelectedTwrInfo = searchedItem;
                        ItemOrder = await CalculateItemOrder(_dokument.Trn_Trnid);
                    }
                    else
                    {
                        // Jeśli nie znaleziono, możesz wykonać strzał do API
                        SelectedTwrInfo = await FetchProductFromApi(ean);
                        if (SelectedTwrInfo != null)
                        {
                            IsEntryIloscEnabled = true;
                            ItemOrder = await CalculateItemOrder(_dokument.Trn_Trnid);
                        }
                    }
                }
            }
            catch (Exception s)
            {

                await Application.Current.MainPage.DisplayAlert("błąd", s.Message, "OK");
                ScannedEan = string.Empty;

            }
        }

        public void ClearExistingItem()
        {
            _existingItem = null;
        }

        private async Task<TwrInfo> FetchProductFromApi(string ean)
        {
            // Tutaj logika do pobierania informacji o produkcie z zewnętrznego API

            string Webquery = $"cdn.pc_pobierztwr '{ean}', 2";
            var produkt = await Api.ExecSQLCommandAsync<TwrInfo>(Webquery);
            //var daneFromCentrala = await App.TodoManager.PobierzTwrAsync(Webquery);

            if (produkt.Any())
            {
                var daneFromCentrala = produkt.First();

                daneFromCentrala.Twr_Url = FilesHelper.ConvertUrlToOtherSize(daneFromCentrala.Twr_Url, daneFromCentrala.Twr_Kod, FilesHelper.OtherSize.home);
                return daneFromCentrala;
            }
            else
            {
                throw new Exception("nie znaleziono produktu o tym eanie");
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
