using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App2.OptimaAPI
{
    public class BaseService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isExport { set; get; }
        public Boolean IsExport
        {
            get { return _isExport; }
            set
            {
                _isExport = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExport)));
            }
        }

        protected void OnPropertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
