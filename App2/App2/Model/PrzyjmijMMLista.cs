using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App2.Model
{
    public class PrzyjmijMMLista : PrzyjmijMMClass
    {
        public static ObservableCollection<PrzyjmijMMLista> przyjmijMMListas = new ObservableCollection<PrzyjmijMMLista>();
        [PrimaryKey, AutoIncrement]
        public int Lp { get; set; } 
    }
}
