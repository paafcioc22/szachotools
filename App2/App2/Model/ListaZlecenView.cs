using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class ListaZlecenView
    {
        public int Fmm_gidnumer { get; set; }
        public string liczbapaczek { get; set; }
        public string Fmm_nrlistu { get; set; }
        public string datautwrz { get; set; }
        public string Fmm_NrDokWydania { get; set; }
        public bool IsSelected { get; set; }


    }
}
