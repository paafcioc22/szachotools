using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class CreateDokumentNaglowek
    {
        public GidTyp GidTyp { get; set; }

        public string MagKod { get; set; }

        public string Opis { get; set; }

        public string UserName { get; set; }
    }

    public enum GidTyp
    {
        Mm = 1,
        Inw = 2
    }
}
