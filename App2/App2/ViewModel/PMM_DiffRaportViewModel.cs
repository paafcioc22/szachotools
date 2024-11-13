using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using App2.View.PrzyjmijMM;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class PMM_DiffRaportViewModel : MvvmHelpers.BaseViewModel
    {
        private bool _isRaportZgodny=false;
        public readonly PMM_DokNaglowek dokument;

        public ObservableRangeCollection<Grouping<string, PMM_RaportElement>> GroupedDifferences { get; set; } = new ObservableRangeCollection<Grouping<string, PMM_RaportElement>>();
        public bool IsRaportZgodny { get => _isRaportZgodny; set => SetProperty(ref _isRaportZgodny, value); }
        public ICommand LoadDataCommand { get; }
        public ICommand OpenSelctedItemCommand { get; set; }
        public List<InventoriedItem> SkanElements { get; }
        public bool _isFirstLoad = true;

        public PMM_DiffRaportViewModel(List<InventoriedItem> skanElements, 
                PMM_DokNaglowek dokument)
        {
            LoadDataCommand = new Command(async() => await GenerateDifferenceReport(dokument, skanElements));
            OpenSelctedItemCommand = new Command<PMM_RaportElement>(OnItemTapped);
            Title = $"Raport {dokument.TrN_DokumentObcy}";
            SkanElements = skanElements;
            this.dokument = dokument;

            MessagingCenter.Subscribe<PMM_AddItemViewModel, bool>(this, "RefreshReport", async (sender, changesMade) =>
            {
                if (changesMade)
                {
                    var items = await App.Database.GetItemsAsync(dokument.Trn_Trnid);
                    await GenerateDifferenceReport(dokument, items);
                    _isFirstLoad = false;
                    Debug.WriteLine("odpalam z mediacenter");
                }
            });
        }
        public void Dispose()
        {
            // Unsubscribe kiedy ViewModel jest niszczone
            MessagingCenter.Unsubscribe<PMM_AddItemViewModel>(this, "RefreshReport");
        }

        private async void OnItemTapped(PMM_RaportElement element)
        {
            var _repository = App.Database;
            InventoriedItem item = new InventoriedItem()
            {
                Twr_Kod= element.Twr_Kod,
                Twr_Ean= element.Twr_Ean,
                Twr_Nazwa= element.Twr_Nazwa,
                ImageUrl= element.Twr_Url,
                DokumentId=dokument.Trn_Trnid,
                ActualQuantity= element.RzeczywistaIlosc,
                ID=element.Id,
                ItemOrder=element.Lp 
            };

            var addItemViewModel = new PMM_AddItemViewModel(_repository, dokument, item,element, GroupedDifferences);
            var addItemPage = new AddSkanElementPage
            {
                BindingContext = addItemViewModel
            };

            await Application.Current.MainPage.Navigation.PushAsync(addItemPage);

        }

        public async Task GenerateDifferenceReport(DokumentMM dokument, List<InventoriedItem> inventoriedItems)
        {
            try
            {
                if (inventoriedItems == null)
                {
                    throw new ArgumentNullException(nameof(inventoriedItems), "Lista inwentaryzacji jest pusta.");
                }

                if (dokument == null || dokument.Elementy == null)
                {
                    throw new ArgumentNullException(nameof(dokument), "Dokument lub jego elementy są puste.");
                }

                var differences = new List<PMM_RaportElement>();

                string ase_operator = View.LoginLista._user + " " + View.LoginLista._nazwisko;

                ServicePrzyjmijMM api = new ServicePrzyjmijMM();
                var magazyn = await api.GetSklepMagNumer();

                var magGidnumer = (Application.Current as App).MagGidNumer;

                if (magGidnumer == 0 || magGidnumer != (short)magazyn.Id)
                {
                    
                    magGidnumer = (short)magazyn.Id;

                }

                // Sprawdzenie zeskanowanych elementów
                foreach (var scannedItem in inventoriedItems)
                {

                    var documentItem = dokument.Elementy.FirstOrDefault(e => e.Twr_Ean == scannedItem.Twr_Ean);
                    if (documentItem != null)
                    {
                        // Jeśli element jest na liście dokumentu, porównaj ilości
                        var difference = scannedItem.ActualQuantity - documentItem.Stan_szt;
                        if (difference != 0)
                        {
                            differences.Add(new PMM_RaportElement
                            {
                                Twr_Ean = scannedItem.Twr_Ean,
                                Twr_Kod = scannedItem.Twr_Kod,
                                Twr_Nazwa = scannedItem.Twr_Nazwa,
                                OczekiwanaIlosc = documentItem.Stan_szt,
                                RzeczywistaIlosc = scannedItem.ActualQuantity,
                                Twr_Url = FilesHelper.ConvertUrlToOtherSize(scannedItem.ImageUrl, scannedItem.Twr_Kod, FilesHelper.OtherSize.small),
                                DataMM = dokument.DataMM,
                                MagNumer = magGidnumer,
                                TrnDokumentObcy = dokument.TrN_DokumentObcy,
                                TrnGidNumer = dokument.Trn_Gidnumer,
                                Operator = ase_operator,
                                Id = scannedItem.ID,
                                Lp = scannedItem.ItemOrder


                            });
                        }
                    }
                    else
                    {
                        // Element jest zeskanowany, ale nie ma go na dokumencie
                        differences.Add(new PMM_RaportElement
                        {
                            Twr_Ean = scannedItem.Twr_Ean,
                            Twr_Nazwa = scannedItem.Twr_Nazwa,
                            Twr_Kod = scannedItem.Twr_Kod,
                            OczekiwanaIlosc = 0,
                            RzeczywistaIlosc = scannedItem.ActualQuantity,
                            Twr_Url = FilesHelper.ConvertUrlToOtherSize(scannedItem.ImageUrl, scannedItem.Twr_Kod, FilesHelper.OtherSize.small),
                            DataMM = dokument.DataMM,
                            MagNumer = magGidnumer,
                            TrnDokumentObcy = dokument.TrN_DokumentObcy,
                            TrnGidNumer = dokument.Trn_Gidnumer,
                            Operator = ase_operator,
                            Id = scannedItem.ID,
                            Lp = scannedItem.ItemOrder
                        });
                    }
                }

                // Sprawdzenie elementów na dokumencie, które nie zostały zeskanowane
                foreach (var documentItem in dokument.Elementy)
                {
                    var scannedItem = inventoriedItems.FirstOrDefault(s => s.Twr_Ean == documentItem.Twr_Ean);
                    if (scannedItem == null)
                    {
                        // Element jest na dokumencie, ale nie został zeskanowany
                        differences.Add(new PMM_RaportElement
                        {
                            Twr_Ean = documentItem.Twr_Ean,
                            Twr_Kod = documentItem.Twr_Kod,
                            Twr_Nazwa = documentItem.Twr_Nazwa,
                            OczekiwanaIlosc = documentItem.Stan_szt,
                            RzeczywistaIlosc = 0,
                            Twr_Url = FilesHelper.ConvertUrlToOtherSize(documentItem.Twr_Url, documentItem.Twr_Kod, FilesHelper.OtherSize.small),
                            DataMM = dokument.DataMM,
                            MagNumer = magGidnumer,
                            TrnDokumentObcy = dokument.TrN_DokumentObcy,
                            TrnGidNumer = dokument.Trn_Gidnumer,
                            Operator = ase_operator
                        });
                    }
                    // Jeśli element został już wcześniej porównany, nie dodawaj go ponownie
                }

                // Grupowanie różnic
                //var braki = differences.Where(x => x.Difference < 0).OrderBy(x=>x.Difference);
                //var nadstan = differences.Where(x => x.Difference > 0).OrderByDescending(x => x.Difference);

                var braki = differences
                    .Where(x => x.Difference < 0)
                    .OrderBy(x => x.Difference)
                    .Select(x => { x.GroupName = "Braki"; return x; });

                var nadstan = differences
                    .Where(x => x.Difference > 0)
                    .OrderByDescending(x => x.Difference)
                    .Select(x => { x.GroupName = "Nadstany"; return x; });

                GroupedDifferences.Clear();
                if (braki.Any())
                {
                    GroupedDifferences.Add(new Grouping<string, PMM_RaportElement>("Braki", braki, Color.FromHex("#F44336")));
                }
                if (nadstan.Any())
                {
                    GroupedDifferences.Add(new Grouping<string, PMM_RaportElement>("Nadstany", nadstan, Color.FromHex("#4CAF50")));
                }


                if (GroupedDifferences.Count == 0)
                {
                    IsRaportZgodny = true;
                }
            }
            catch (Exception s)
            {
                await Application.Current.MainPage.DisplayAlert("błąd",s.Message,"OK");

                var properties = new Dictionary<string, string>
                {
                    { "conn", "/api/przyjmijMM/generowanie raportu" },
                    { "SkanElements", SkanElements.Count.ToString()},
                    { "dokumn",dokument.Elementy.Count.ToString()},
                    { "user", App.SessionManager.CurrentSession.UserName }
                };

                Crashes.TrackError(s, properties);

            }
        }




        public class Grouping<TKey, TItem> : ObservableRangeCollection<TItem>
        {
            public TKey Key { get; private set; }
            public Color BackgroundColor { get; private set; }
            public Grouping(TKey key, IEnumerable<TItem> items, Color backgroundColor)
            {
                Key = key;
                AddRange(items);
                BackgroundColor = backgroundColor;
            }
        }
    }
}
