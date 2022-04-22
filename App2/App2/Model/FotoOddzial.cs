using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class FotoOddzial : ListaSklepow
    {
        public int IleFotek { get; set; }

        private bool isDone;

        public bool IsDone
        {
            get => isDone = IleFotek > 0;            
        }

    }
}
