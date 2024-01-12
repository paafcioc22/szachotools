using App2.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App2.OptimaAPI
{
    public class BaseService : ViewModelBase //INotifyPropertyChanged
    {

     
        private bool _isExport;
        public bool IsExport
        {
            get => _isExport;
            set => _isExport = SetProperty(ref _isExport, value);
        }


        

    }
}
