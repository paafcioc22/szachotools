using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model
{
    public class PMM_RaportElement
    {
        public string Twr_Ean { get; set; }
        public string Twr_Url { get; set; }
        public string Twr_Kod { get; set; }
        public string Twr_Nazwa { get; set; }
        public int OczekiwanaIlosc { get; set; }
        public int RzeczywistaIlosc { get; set; }
        public int Difference => RzeczywistaIlosc - OczekiwanaIlosc;
        public string Status { get; set; }
        public int MagNumer { get; set; }
        public int TrnGidNumer { get; set; }
        public string TrnDokumentObcy { get; set; }
        public string DataMM { get; set; }
        public string Operator { get; set; }
    }
}
