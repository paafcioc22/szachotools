using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App2.Model
{
    public class ListDiffrence
    {
        public string twrkod { get; set; }
        public string twrean { get; set; }
        public int ilosc { get; set; }
        public int IleZMM { get; set; }
        public int IleZeSkan { get; set; }
        public string typ { get; set; }
        public string NrDokumentu { get; set; }
        public string TwrNazwa { get; set; }
        public string Operator { get; set; }
        public string DataDokumentu { get; set; }
        public int GidMagazynu { get; set; }
        public int GidDokumentu { get; set; }
        public int XLGidMM { get; set; }

        public static ObservableCollection<ListDiffrence> listDiffrences = new ObservableCollection<ListDiffrence>();
    } 

}
