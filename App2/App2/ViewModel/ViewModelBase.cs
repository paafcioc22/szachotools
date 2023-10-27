using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace App2.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {
        private bool isVisible;
     

        public string Sklep { get; set; }
        public string FullTitle => $"{Title} - {Sklep}";
        public bool IsVisible 
        { 
            get => isVisible; 
            set => isVisible =  SetProperty(ref isVisible, value); }

    }
}
