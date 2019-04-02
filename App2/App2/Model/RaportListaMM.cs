using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App2.Model
{
    public class RaportListaMM :PrzyjmijMMClass
    {

        public static  ObservableCollection<RaportListaMM> RaportListaMMs = new ObservableCollection<RaportListaMM>();

        [PrimaryKey, AutoIncrement]
        public int Lp { get; set; }
        [Indexed]
        public int IdElement { get; set; }
        public int TwrGidnumer { get; set; } 
        public bool Sended { get; set; }
    }
}
