using App2.Model;
using App2.ViewModel;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.PrzyjmijMM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiffrenceRaportPage : ContentPage
    {

        public DiffrenceRaportPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (BindingContext is PMM_DiffRaportViewModel viewModel)
                {
                    // viewModel.LoadDataCommand.Execute(null);
                    Debug.WriteLine("odpalam a onapreainh");

                    if (viewModel._isFirstLoad)
                    {
                        viewModel.LoadDataCommand.Execute(null);
                        viewModel._isFirstLoad = false; // Ustawienie flagi na false po pierwszym załadowaniu
                    }
                }
            }
            catch (Exception s)
            {
                await DisplayAlert("Uwaga", "Wystąpił nieoczekiwany błąd - wygeneruj raport jeszcze raz", "OK");
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is PMM_DiffRaportViewModel viewModel)
            {
                viewModel.Dispose();
            }
        }

        private async void btn_SendRaportMM_Clicked(object sender, EventArgs e)
        {
            btn_SendRaportMM.IsEnabled = false;
            var action = await DisplayAlert(null, "Czy chcesz wysłać raport?", "Tak", "Nie");
            try
            {

                if (action)
                {
                    if (BindingContext is PMM_DiffRaportViewModel viewModel)
                    {
                       
                        if (viewModel.GroupedDifferences.Count != 0)
                        {
                            var lista = FlattedList(viewModel.GroupedDifferences);
                            var result = await App.TodoManager.InsertDataNiezgodnosci(lista);

                            if (result.ToString() == "OK")
                            {
                                // btn_sendraport
                                await DisplayAlert("Info", "Raport został wysłany pomyślnie.", "OK");                                                               

                                var navigation = this.Navigation;
                                if (navigation.NavigationStack.Count > 2)
                                {
                                    var pageToRemove1 = navigation.NavigationStack[navigation.NavigationStack.Count - 2];
                                    var pageToRemove2 = navigation.NavigationStack[navigation.NavigationStack.Count - 3];

                                    navigation.RemovePage(pageToRemove1);
                                    navigation.RemovePage(pageToRemove2);
                                    await navigation.PopAsync();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Uwaga", "Raport został już wysłany.", "OK");

                            }
                        }
                        else
                        {
                            var magGidnumer = (Application.Current as App).MagGidNumer;
                            var lista2 = new List<ListDiffrence>()
                            {
                                new ListDiffrence
                                {
                                    twrkod = "Lista zgodna",
                                    IleZeSkan = 0,
                                    IleZMM = 0,
                                    ilosc = 0,
                                    NrDokumentu = viewModel.dokument.TrN_DokumentObcy,
                                    GidMagazynu = magGidnumer,
                                    DataDokumentu = viewModel.dokument.DataMM,
                                    Operator = View.LoginLista._user + " " + View.LoginLista._nazwisko,
                                    XLGidMM = viewModel.dokument.Trn_Gidnumer
                                }
                            };


                            var Maglista = await App.TodoManager.InsertDataNiezgodnosci(lista2);

                            if (Maglista.ToString() == "OK")
                            {
                                await DisplayAlert("Info", "Raport został wysłany pomyślnie.", "OK");
                                                               
                                var navigation = this.Navigation;
                                if (navigation.NavigationStack.Count > 2)
                                {
                                    var pageToRemove1 = navigation.NavigationStack[navigation.NavigationStack.Count - 2];
                                    var pageToRemove2 = navigation.NavigationStack[navigation.NavigationStack.Count - 3];

                                    navigation.RemovePage(pageToRemove1);
                                    navigation.RemovePage(pageToRemove2);
                                    await navigation.PopAsync();
                                }

                            }
                            else
                            {
                                await DisplayAlert("Uwaga", "Raport został już wysłany.", "OK");
                            }
                        }
                    }
                }
                else
                {
                    btn_SendRaportMM.IsEnabled = true;
                }
            }
            catch (Exception x)
            {

                await DisplayAlert(null, x.Message, "OK");
            }
        }

        private List<PMM_RaportElement> FlattedList(ObservableRangeCollection<PMM_DiffRaportViewModel.Grouping<string, PMM_RaportElement>> groupedDifferences)
        {
            var flattedList = new List<PMM_RaportElement>();

            foreach (var group in groupedDifferences)
            {
                foreach (var item in group)
                {
                    flattedList.Add(item);
                }
            }
            return flattedList;
        }


    }
}
