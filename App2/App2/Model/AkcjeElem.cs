using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class AkcjeElem
    {
        public int AkN_GidNumer { get; set; }
        public int MagNumer { get; set; }
        public string TwrGrupa { get; set; }
        public string TwrDep { get; set; }
        public int TwrGidNumer { get; set; }
        public int TwrStan { get; set; }
        public int TwrSkan { get; set; }


    }
}
