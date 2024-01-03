using App2.Model;
using App2.Model.ApiModel;
using App2.Services;
using App2.View.PrzyjmijMM;
using MvvmHelpers;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class PMM_SkanListViewModel : ViewModelBase
    {
        public ObservableRangeCollection<InventoriedItem> SkanElements { get; set; }
        private SQLiteRepository _repository;
        private readonly PMM_DokNaglowek dokument;

        public ICommand LoadSkanElementsCommand { get; set; }
        public ICommand OpenAddElementPageCommand { get; set; } //dodaj element - widok skoanowania
        public ICommand CreateDiffReportCommand { get; set; }//przycisk generuj raport różnic
        public ICommand OpenSelctedItemCommand { get; set; }//przycisk generuj raport różnic
        public ICommand DeleteItemCommand { get; set; }//przycisk generuj raport różnic

        public PMM_SkanListViewModel(PMM_DokNaglowek dokument)
        {
            _repository = App.Database;
            SkanElements = new ObservableRangeCollection<InventoriedItem>();
            LoadSkanElementsCommand = new Command(async() => await OpenScannedList());
            OpenAddElementPageCommand = new Command(async () => await AddSkanElementPage());
            CreateDiffReportCommand = new Command(async () => await OpenDiffRaportPage());//new Command<PMM_DokNaglowek>(OnItemTapped);
            OpenSelctedItemCommand = new Command<InventoriedItem>(OnItemTapped);
            DeleteItemCommand = new Command<InventoriedItem>(async (obj)=> await DeleteItem(obj));
            this.dokument = dokument;
        }

        private async Task DeleteItem(InventoriedItem item)
        {
            var odp = await Application.Current.MainPage.DisplayAlert("Uwaga", "Czy na pewno chcesz usunąć ten towar z listy?", "TAK", "Nie");

            if (odp)
            {
                var deleted = await _repository.DeleteItemAsync(item);

                if (deleted > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SkanElements.Remove(item);
                    });
                }

            }

            //await OpenScannedList();
        }

        private async void OnItemTapped(InventoriedItem item)
        {             
            var addItemViewModel = new PMM_AddItemViewModel(_repository, dokument, item);
            var addItemPage = new AddSkanElementPage
            {
                BindingContext = addItemViewModel
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(addItemPage);
        }

        private async Task OpenDiffRaportPage()
        {
            var raportViewModel = new PMM_DiffRaportViewModel(SkanElements.ToList(),dokument);
            var diffRaportPage = new DiffrenceRaportPage
            {
                BindingContext = raportViewModel
            };

            await Application.Current.MainPage.Navigation.PushAsync(diffRaportPage);
        }

        private async Task AddSkanElementPage()
        {
             
            var addItemViewModel = new PMM_AddItemViewModel(_repository, dokument);
            var addItemPage = new AddSkanElementPage
            {
                BindingContext = addItemViewModel
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(addItemPage);

        }

        private async Task OpenScannedList()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                SkanElements.Clear();
                var items = await _repository.GetItemsAsync(dokument.Trn_Trnid);

                SkanElements.AddRange(items);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., log the error)
            }
            finally
            {
                IsBusy = false;
            }
        }

         
    }
}
