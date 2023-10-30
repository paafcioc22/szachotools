using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class StatusTable
    {
        public string Statuss { get; set; }
        public int AknNumer { get; set; }
        public int MagNumer { get; set; }
        public int TwrNumer { get; set; }
    }
}
