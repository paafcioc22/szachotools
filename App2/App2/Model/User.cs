using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class User
    {

        public string GID { get; set; }
        public string Ope_ident { get; set; }
        public string Prc_Akronim { get; set; }
        public string Haslo { get; set; }
    }
}
