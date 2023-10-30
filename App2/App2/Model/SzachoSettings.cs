using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class SzachoSettings
    {
        public string VersionApp { get; set; }
        public bool IsCena1Enabled { get; set;}
    }
}
