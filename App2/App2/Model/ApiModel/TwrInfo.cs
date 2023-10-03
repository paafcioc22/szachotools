using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace App2.Model.ApiModel
{
    [XmlType("Table")]
    public class TwrInfo
    {
        [XmlElement("twrkod")]
        public string Twr_Kod { get; set; }

        [XmlElement("TwrGidnumer")]
        public int Twr_Gidnumer { get; set; }
        [XmlIgnore]
        public int Stan_szt { get; set; }
        [XmlIgnore]
        public int Tre_Lp { get; set; }
        [XmlIgnore]
        public string  Twr_Grupa { get; set; }

        [XmlElement("url")]
        public string Twr_Url { get; set; }
        [XmlIgnore]
        public UriImageSource Twr_UrlImage { get; set; } // Pole przechowujące UriImageSource

        [XmlElement("nazwa")]
        public string Twr_Nazwa { get; set; }

        [XmlElement("symbol")]
        public string Twr_Symbol { get; set; }

        [XmlElement("ean")]
        public string Twr_Ean { get; set; }

        [XmlElement("cena")]
        public decimal Twr_Cena { get; set; }

        [XmlElement("cena1")]
        public decimal Twr_Cena1 { get; set; }
    }
}
