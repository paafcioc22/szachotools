using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{

    [XmlType("Table")]
    public class ListaSklepow
    {
        
        public string mag_kod { set; get; }
        public string mag_nazwa { set; get; }
        public string mag_gidnumer { set; get; }
        public string mag_region { set; get; }
    }             
}
