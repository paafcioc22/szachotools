using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class NagElem
    {
        string datastart;
        string datakoniec;
        //AkN_DataStart = datastart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        //AkN_DataKoniec = datakoniec.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 

        public int AkN_GidNumer { get; set; }
        public int AkN_GidTyp { get; set; }
        public string AkN_GidNazwa { get; set; }
        public string AkN_NazwaAkcji { get; set; }

        public string AkN_DataStart
        {
            get => datastart;
            set => datastart = ((DateTime)Convert.ToDateTime(value)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public string AkN_DataKoniec
        {
            get => datakoniec;
            set => datakoniec = ((DateTime)Convert.ToDateTime(value)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public string AkN_ZakresDat
        {
            get => String.Format($"{AkN_DataStart} - {AkN_DataKoniec}"); 
        }

        public string Ake_FiltrSQL { get; set; }
        public bool IsSendData { get; set; }

    }

}
