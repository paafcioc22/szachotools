using App2.Model;
using App2.ViewModel;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View.PrzyjmijMM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSkanElementPage : ContentPage
    {

        public AddSkanElementPage()
        {
            InitializeComponent(); 

        }

        public enum UserDecision
        {
            AddQuantity,
            ReplaceQuantity,
            Cancel
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<PMM_AddItemViewModel, InventoriedItem>(this, "AskUserDecision", async (sender, args) =>
            {

                var decision = await DisplayActionSheet(
                    $"Element już istnieje: {args.ActualQuantity} szt, co chcesz zrobić?",
                    "Anuluj",
                    "DODAJ",
                    "Dodaj ilość", "Zastąp ilość");

                UserDecision odp;
                switch (decision)
                {
                    case "Dodaj ilość":
                        odp = UserDecision.AddQuantity;
                        break; // Tutaj dodajemy break
                    case "Zastąp ilość":
                        odp = UserDecision.ReplaceQuantity;
                        break; // I tutaj
                    case "DODAJ":
                        odp = UserDecision.AddQuantity;
                        break; // I tutaj
                    case "Anuluj":
                        odp = UserDecision.Cancel;
                        break; // I tutaj
                    default:
                        odp = UserDecision.Cancel; // Domyślnie, jeśli użytkownik kliknie poza, to anuluj
                        break; // I tutaj
                }
                // Wyślij wiadomość z powrotem do ViewModel z decyzją użytkownika
                MessagingCenter.Send(this, "UserDecisionMade", odp);
            });

            if (BindingContext is PMM_AddItemViewModel viewModel)
            {
                viewModel.RequestDelete += ViewModel_RequestDelete;

                viewModel.PropertyChanged += ViewModel_PropertyChanged;

                var app = Application.Current as App; 

                if (app.Skanowanie == 0)
                {
                    if (!viewModel.IsEntryIloscEnabled)
                        entrySkanEan.Focus();
                }
            }

        }

        private async void ViewModel_RequestDelete(object sender, InventoriedItem e)
        {
            if (e.ActualQuantity > 0)
            {

                var answer = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć ten element?", "Tak", "Nie");
                if (answer)
                {
                    // Wywołaj metodę usunięcia w ViewModel
                    await ((PMM_AddItemViewModel)BindingContext).DeleteExceute(e);

                    var navigation = this.Navigation;
                    //{App2.View.PrzyjmijMM.AddSkanElementPage}
                    if (navigation.NavigationStack.Count > 0  )
                    {
                        var last = navigation.NavigationStack[navigation.NavigationStack.Count - 2];

                        if (last.GetType().Name == "DiffrenceRaportPage")
                        {
                            var pageToRemove1 = navigation.NavigationStack[navigation.NavigationStack.Count - 1];

                            navigation.RemovePage(pageToRemove1);

                        }

                        await navigation.PopAsync();
                    }

                    //await Navigation.PopModalAsync();
                    //await Navigation.PopModalAsync();
                }
            }
            else
            {
                await DisplayAlert("Ostrzeżenie", "Produkt nie był skanowany - nie możesz go usunąć", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is PMM_AddItemViewModel viewModel)
            {
                viewModel.ClearExistingItem();
                viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                if (!viewModel.IsEditMode)
                {
                    MessagingCenter.Unsubscribe<AddSkanElementPage, UserDecision>(this, "UserDecisionMade");
                }
            }
            // Pamiętaj, aby zrezygnować z subskrypcji, gdy strona znika

            MessagingCenter.Unsubscribe<PMM_AddItemViewModel, InventoriedItem>(this, "AskUserDecision");

        }


        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BindingContext is PMM_AddItemViewModel viewModel)
            {
                if (e.PropertyName == nameof(PMM_AddItemViewModel.IsEntryIloscEnabled))
                {
                    if (viewModel.IsEntryIloscEnabled)
                        entryIlosc.Focus();
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Ostrzeżenie", "Towar nie był skanowany\nzamiast edycji zeskanuj na wcześniejszym oknie ", "OK");

        }
    }
}