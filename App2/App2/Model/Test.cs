using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace App2.Model
{
    public class Test:INotifyPropertyChanged
    {
        public int gidnumer { set; get; }
        public string mag_dcl { set; get; }
        public string twrkod { set; get; }

        private int _iloscOK;
        public int ilosc
        {
            get { return _iloscOK; }
            set
            {
                //if (_iloscOK == value)
                //    return;
                _iloscOK = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ilosc)));
            }
        }
         

        public event PropertyChangedEventHandler PropertyChanged;


        

    }
}
